// ==================== IMPORTACIONES NECESARIAS ====================
// Importaciones para configurar toda la infraestructura de la API
using Microsoft.EntityFrameworkCore;  // Para configurar Entity Framework y base de datos
using Microsoft.AspNetCore.Authentication.JwtBearer;  // Para autenticación JWT
using Microsoft.IdentityModel.Tokens;  // Para configuración de tokens JWT
using System.Text;  // Para codificación de texto en JWT
using MarketingDataSystem.Infrastructure.Data;  // Contextos de base de datos
using MarketingDataSystem.Core.Interfaces;  // Interfaces del dominio (contratos)
using MarketingDataSystem.Infrastructure.Repositories;  // Implementaciones de repositorios
using MarketingDataSystem.Application.Services;  // Servicios de aplicación (lógica de negocio)
using Serilog;  // Para logging estructurado y persistente
using MarketingDataSystem.Application.Interfaces;  // Interfaces de servicios de aplicación
using MarketingDataSystem.Infrastructure.UnitOfWork;  // Patrón Unit of Work para transacciones
using FluentValidation;  // Para validaciones automáticas de DTOs
using Microsoft.Extensions.Diagnostics.HealthChecks;  // Para health checks del sistema
using Microsoft.OpenApi.Models;  // Para configuración de Swagger/OpenAPI

// Crear el builder de la aplicación web - punto de entrada principal
var builder = WebApplication.CreateBuilder(args);

// ==================== CONFIGURACIÓN DE LOGGING ====================
// Serilog para logging estructurado - fundamental para diagnóstico y monitoreo
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  // Logs en consola para desarrollo y debugging
    .WriteTo.File("logs/marketing-data-system-.txt", rollingInterval: RollingInterval.Day)  // Archivos diarios de logs
    .CreateLogger();

// Integrar Serilog como proveedor principal de logging
builder.Host.UseSerilog();

// ==================== CONFIGURACIÓN DE BASE DE DATOS ====================
// Configurar MarketingDataContext - Contexto principal para todos los datos de marketing
builder.Services.AddDbContext<MarketingDataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),  // String de conexión desde appsettings.json
        b => b.MigrationsAssembly("MarketingDataSystem.API")  // Las migraciones están en el proyecto API
    ));

// Mantener ApplicationDbContext para autenticación - mantiene compatibilidad con sistema legacy
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("MarketingDataSystem.API")  // Misma configuración para consistencia
    ));

// ==================== CONFIGURACIÓN DE CORS ====================
// CORS permite que aplicaciones frontend (React, Angular, Vue) accedan a nuestra API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()    // Permitir cualquier origen (para desarrollo)
                   .AllowAnyMethod()    // Permitir GET, POST, PUT, DELETE, PATCH, etc.
                   .AllowAnyHeader();   // Permitir cualquier header HTTP personalizado
        });
});

// ==================== CONFIGURACIÓN DE AUTENTICACIÓN JWT ====================
// JWT (JSON Web Tokens) para autenticación stateless y segura - no requiere sesiones
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("La clave JWT no está configurada.");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,              // Verificar quién emitió el token
            ValidateAudience = true,            // Verificar para quién está destinado el token
            ValidateLifetime = true,            // Verificar que el token no haya expirado
            ValidateIssuerSigningKey = true,    // Verificar la firma digital del token
            ValidIssuer = builder.Configuration["Jwt:Issuer"],      // Emisor válido (nuestro sistema)
            ValidAudience = builder.Configuration["Jwt:Audience"],  // Audiencia válida (nuestros clientes)
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))  // Clave para verificar firma
        };
    });

// ==================== REGISTRO DE REPOSITORIOS Y PATRONES ====================
// Repositorio genérico - implementa operaciones CRUD básicas (Create, Read, Update, Delete)
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Unit of Work - patrón para coordinar multiple repositorios en una sola transacción
builder.Services.AddScoped<IUnitOfWork, MarketingDataSystem.Infrastructure.UnitOfWork.UnitOfWork>();

// ==================== CONFIGURACIÓN DE MAPEO Y VALIDACIÓN ====================
// AutoMapper - mapea automáticamente entre DTOs (Data Transfer Objects) y Entidades
builder.Services.AddAutoMapper(typeof(Program));

// FluentValidation - validaciones automáticas de DTOs con reglas declarativas
builder.Services.AddValidatorsFromAssembly(typeof(MarketingDataSystem.Application.Validators.ClienteDtoValidator).Assembly);

// ==================== SERVICIOS PRINCIPALES DE AUTENTICACIÓN ====================
// Servicio de autenticación - maneja login, registro y generación de tokens JWT
builder.Services.AddScoped<IAuthService, AuthService>();

// ==================== SERVICIOS DE DOMINIO (LÓGICA DE NEGOCIO) ====================
// Estos servicios implementan todas las reglas de negocio según el SRS
builder.Services.AddScoped<IClienteService, ClienteService>();          // Gestión completa de clientes
builder.Services.AddScoped<IProductoService, ProductoService>();        // Catálogo y gestión de productos
builder.Services.AddScoped<IVentaService, VentaService>();              // Procesamiento de ventas
builder.Services.AddScoped<IStockService, StockService>();              // Control de inventario y stock
builder.Services.AddScoped<IFuenteDeDatosService, FuenteDeDatosService>(); // Configuración de fuentes de datos
builder.Services.AddScoped<IReporteService, ReporteService>();          // Lógica de generación de reportes
builder.Services.AddScoped<IUsuarioMarketingService, UsuarioMarketingService>(); // Gestión de usuarios del sistema

// ==================== REPOSITORIOS ESPECÍFICOS SEGÚN SRS ====================
// Repositorios especializados para entidades específicas del dominio
builder.Services.AddScoped<IReporteRepository, ReporteRepository>();            // Persistencia de reportes generados
builder.Services.AddScoped<IIngestionLogRepository, IngestionLogRepository>();  // Logs del proceso ETL

// ==================== SERVICIOS ETL (EXTRACT, TRANSFORM, LOAD) ====================
// Pipeline completo de procesamiento de datos - corazón del sistema de datos
builder.Services.AddScoped<IValidadorService, ValidadorService>();        // Validación de datos crudos
builder.Services.AddScoped<ITransformadorService, TransformadorService>(); // Transformación de formatos y tipos
builder.Services.AddScoped<IEnriquecedorService, EnriquecedorService>();  // Enriquecimiento con datos externos
builder.Services.AddScoped<IDeduplicadorService, DeduplicadorService>();  // Eliminación de registros duplicados
builder.Services.AddScoped<ILoggerService, LoggerService>();              // Logging especializado para ETL
builder.Services.AddScoped<PipelineETLService>();                         // Orquestador principal del pipeline ETL

// ==================== SERVICIOS DE INFRAESTRUCTURA CRÍTICA ====================
// Servicio de alertas - notificaciones automáticas para eventos críticos
builder.Services.AddHttpClient<IAlertaService, AlertaService>();  // Cliente HTTP para webhooks (Slack, Teams)
builder.Services.AddScoped<IAlertaService, AlertaService>();

// Servicio de backups automáticos - protección de datos críticos
builder.Services.AddScoped<IBackupService, BackupService>();

// ==================== SERVICIOS SINGLETON Y ESPECIALIZADOS ====================
// EventBus - Patrón Publisher/Subscriber para comunicación desacoplada entre servicios
builder.Services.AddSingleton<IEventBus, EventBus>();
// Generador de reportes Excel profesionales con múltiples hojas y formateo
builder.Services.AddScoped<IGeneradorReporteService, GeneradorReporteService>();

// ==================== SERVICIO PROGRAMADO (BACKGROUND SERVICE) ====================
// Scheduler que ejecuta automáticamente el pipeline ETL a las 02:00 AM todos los días
builder.Services.AddHostedService<MarketingDataSystem.API.Services.ETLSchedulerHostedService>();

// ==================== HEALTH CHECKS (MONITOREO DEL SISTEMA) ====================
// Endpoints para verificar el estado de salud de componentes críticos
builder.Services.AddHealthChecks()
    .AddDbContextCheck<MarketingDataContext>("database")  // Verifica conectividad con base de datos
    .AddCheck("etl_service", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("ETL Service operativo"))
    .AddCheck("alertas_service", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("Sistema de alertas operativo"));

// ==================== CONFIGURACIÓN MVC Y DOCUMENTACIÓN ====================
builder.Services.AddControllers();        // Habilitar controladores MVC para API REST
builder.Services.AddEndpointsApiExplorer(); // Generar documentación automática de endpoints

// Configurar Swagger con soporte para JWT Authorization
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Marketing Data System API", 
        Version = "v1",
        Description = "API para gestión de datos de marketing con ETL automático y reportes Excel"
    });
    
    // Configurar autenticación JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header usando el esquema Bearer. 
                      Ingrese 'Bearer' [espacio] y luego su token en el campo de texto.
                      Ejemplo: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// Construir la aplicación con toda la configuración definida
var app = builder.Build();

// ==================== SEED DATA (DATOS INICIALES CRÍTICOS) ====================
// Crear usuario administrador por defecto para acceso inicial al sistema
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<MarketingDataContext>();
        // Verificar si ya existe un usuario admin
        var existingAdmin = await context.UsuariosMarketing.FirstOrDefaultAsync(u => u.Username == "admin");
        
        if (existingAdmin == null)
        {
            // Crear usuario admin con credenciales por defecto
            var adminUser = new MarketingDataSystem.Core.Entities.UsuarioMarketing
            {
                Username = "admin",
                Email = "admin@marketingdata.com", 
                Nombre = "Administrador del Sistema",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),  // Hash seguro de contraseña
                Role = "Admin",
                FechaCreacion = DateTime.Now,
                Activo = true
            };
            
            context.UsuariosMarketing.Add(adminUser);
            await context.SaveChangesAsync();
            Console.WriteLine("✅ Usuario admin creado: admin@marketingdata.com / admin123");
        }
        else
        {
            Console.WriteLine("✅ Usuario admin ya existe en la base de datos.");
        }
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error creating seed data");
    }
}

// ==================== CONFIGURACIÓN DE EVENTOS (EVENT BUS) ====================
// Configurar comunicación por eventos entre servicios - arquitectura reactiva
using (var scope = app.Services.CreateScope())
{
    var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
    var generadorReporte = scope.ServiceProvider.GetRequiredService<IGeneradorReporteService>();
    // Cuando el ETL termine ('CargaFinalizada'), automáticamente generar reporte Excel
    eventBus.Suscribir("CargaFinalizada", generadorReporte.GenerarReporte);
}

// ==================== CONFIGURACIÓN DEL PIPELINE HTTP ====================
// Configurar documentación solo en desarrollo (no exponer en producción por seguridad)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();      // Generar especificación OpenAPI JSON
    app.UseSwaggerUI();    // Interfaz web interactiva para probar la API
}

// IMPORTANTE: El orden de los middleware es crítico
app.UseHttpsRedirection();  // Forzar redireccionamiento a HTTPS
app.UseCors("AllowAll");    // Habilitar CORS para permitir acceso desde frontend
app.UseAuthentication();    // Procesar y validar tokens JWT
app.UseAuthorization();     // Verificar permisos y atributos [Authorize]

// ==================== MAPEO DE ENDPOINTS ====================
// Endpoint de monitoreo para verificar estado del sistema
app.MapHealthChecks("/health");

// Mapear automáticamente todos los controladores definidos
app.MapControllers();

// ==================== INICIAR LA APLICACIÓN ====================
app.Run();

// ==================== CONFIGURACIÓN PARA TESTING ====================
// Hacer la clase Program accesible para pruebas de integración
public partial class Program { }
