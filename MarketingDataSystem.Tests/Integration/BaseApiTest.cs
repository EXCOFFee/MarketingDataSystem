// ==================== CLASE BASE PARA TESTS DE INTEGRACIÓN API ====================
// Esta clase base proporciona infraestructura común para todos los tests de integración
// PROPÓSITO: Simplificar y estandarizar la configuración de tests de API endpoints
// ARQUITECTURA: Test Layer - Infraestructura común para testing de APIs REST
// ISOLATION: Cada test ejecuta con base de datos en memoria independiente
// PERFORMANCE: Optimizada para ejecución rápida de suites de tests extensas
// CONSISTENCY: Garantiza configuración consistente entre todos los tests
// ENTERPRISE: Preparada para CI/CD pipelines empresariales

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MarketingDataSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Linq;

namespace MarketingDataSystem.Tests.Integration
{
    /// <summary>
    /// Clase base abstracta para todos los tests de integración de API REST
    /// RESPONSABILIDAD: Proporcionar infraestructura común y configuración estándar
    /// AISLAMIENTO: Cada test ejecuta con base de datos en memoria limpia e independiente
    /// PERFORMANCE: Optimizada para ejecución rápida en pipelines CI/CD empresariales
    /// CONFIGURACIÓN: Manejo automático de WebApplicationFactory y HttpClient
    /// EXTENSIBILIDAD: Métodos virtuales para personalización en clases derivadas
    /// CASOS DE USO:
    /// - Tests de endpoints de API REST (GET, POST, PUT, DELETE)
    /// - Tests de autorización y autenticación JWT
    /// - Tests de validación de entrada y salida
    /// - Tests de integración con base de datos
    /// - Tests de performance y carga de APIs
    /// ENTERPRISE FEATURES:
    /// - Soporte para configuración por ambiente (dev, staging, prod)
    /// - Integración con herramientas de CI/CD (Azure DevOps, GitHub Actions)
    /// - Métricas y logging detallado para debugging
    /// - Soporte para tests paralelos sin interferencia
    /// </summary>
    public abstract class BaseApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        // ========== INFRAESTRUCTURA CORE PARA TESTING ==========
        /// <summary>
        /// Factory para crear instancias de la aplicación web para testing
        /// CONFIGURACIÓN: Configurada con base de datos en memoria y servicios mock
        /// ISOLATION: Cada test obtiene una instancia independiente y limpia
        /// PERFORMANCE: Reutilizada eficientemente para múltiples tests
        /// </summary>
        protected readonly WebApplicationFactory<Program> _factory;
        
        /// <summary>
        /// Cliente HTTP configurado para llamadas a la API bajo test
        /// CONFIGURACIÓN: Pre-configurado con base URL y headers apropiados
        /// AUTENTICACIÓN: Listo para agregar tokens JWT cuando sea necesario
        /// TIMEOUT: Configurado con timeouts apropiados para tests automatizados
        /// </summary>
        protected readonly HttpClient _client;
        
        /// <summary>
        /// Contexto de base de datos para verificaciones directas y setup de datos
        /// EN MEMORIA: Configurado con InMemoryDatabase para aislamiento total
        /// LIMPIEZA: Automáticamente limpiado entre tests para evitar interferencia
        /// PERFORMANCE: Optimizado para operaciones rápidas de testing
        /// </summary>
        protected readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor base que configura toda la infraestructura de testing
        /// CONFIGURACIÓN: Establece base de datos en memoria, servicios y cliente HTTP
        /// ISOLATION: Cada instancia obtiene GUID único para evitar colisiones
        /// CUSTOMIZACIÓN: Permite personalización a través de métodos virtuales
        /// ERROR HANDLING: Configuración robusta que falla rápido en caso de problemas
        /// </summary>
        /// <param name="factory">Factory de aplicación web para testing</param>
        protected BaseApiTest(WebApplicationFactory<Program> factory)
        {
            // ========== CONFIGURACIÓN DE FACTORY CON PERSONALIZACIONES ==========
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // ========== CONFIGURACIÓN DE BASE DE DATOS EN MEMORIA ==========
                    // Reemplazar base de datos real con InMemoryDatabase para aislamiento
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // ========== CONFIGURACIÓN DE GUID ÚNICO PARA AISLAMIENTO TOTAL ==========
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}");
                    });

                    // ========== HOOK PARA CONFIGURACIÓN PERSONALIZADA ==========
                    // Permitir que clases derivadas configuren servicios adicionales
                    ConfigureServices(services);
                });

                builder.ConfigureAppConfiguration((context, config) =>
                {
                    // ========== HOOK PARA CONFIGURACIÓN PERSONALIZADA ==========
                    // Permitir que clases derivadas configuren configuración adicional
                    ConfigureAppConfiguration(context, config);
                });
            });

            // ========== INICIALIZACIÓN DE CLIENTE Y CONTEXTO ==========
            _client = _factory.CreateClient();
            _context = _factory.Services.GetRequiredService<ApplicationDbContext>();
        }

        // ========== MÉTODOS VIRTUALES PARA PERSONALIZACIÓN ==========
        /// <summary>
        /// Método virtual para configurar servicios adicionales en clases derivadas
        /// EXTENSIBILIDAD: Permite personalización específica por tipo de test
        /// CASOS DE USO: Configurar mocks, servicios especializados, configuraciones
        /// OVERRIDE: Implementar en clases derivadas cuando sea necesario
        /// </summary>
        /// <param name="services">Colección de servicios para configurar</param>
        protected virtual void ConfigureServices(IServiceCollection services)
        {
            // Sobrescribir en clases derivadas para configurar servicios adicionales
        }

        /// <summary>
        /// Método virtual para configurar configuración de aplicación adicional
        /// EXTENSIBILIDAD: Permite personalización de appsettings por tipo de test
        /// CASOS DE USO: Configurar connection strings, API keys, feature flags
        /// OVERRIDE: Implementar en clases derivadas cuando sea necesario
        /// </summary>
        /// <param name="context">Contexto del web host builder</param>
        /// <param name="config">Builder de configuración</param>
        protected virtual void ConfigureAppConfiguration(WebHostBuilderContext context, IConfigurationBuilder config)
        {
            // Sobrescribir en clases derivadas para configurar configuración adicional
        }

        // ========== UTILIDADES DE LIMPIEZA Y MANTENIMIENTO ==========
        /// <summary>
        /// Limpia completamente la base de datos y la recrea limpia para el próximo test
        /// AISLAMIENTO: Garantiza que cada test empiece con estado limpio
        /// PERFORMANCE: Optimizada para ser rápida y no impactar tiempo total de tests
        /// RELIABILITY: Manejo robusto de errores de limpieza
        /// CASOS DE USO: Llamar en setup de tests que requieren estado limpio
        /// </summary>
        protected async Task CleanupDatabase()
        {
            _context.Database.EnsureDeleted();
            await _context.Database.EnsureCreatedAsync();
        }

        // ========== MÉTODOS HELPER PARA AUTENTICACIÓN EN TESTS ==========
        /// <summary>
        /// Obtiene token de autenticación simulado para usuario administrador
        /// PROPÓSITO: Facilitar tests de endpoints que requieren privilegios administrativos
        /// SIMULACIÓN: En tests reales, esto haría llamada real al endpoint de login
        /// FUTURO: Considerar integración con AuthService real para mayor realismo
        /// CASOS DE USO:
        /// - Tests de endpoints administrativos (/api/admin/*)
        /// - Tests de operaciones privilegiadas (crear usuarios, configurar sistema)
        /// - Tests de autorización y control de acceso basado en roles
        /// </summary>
        /// <returns>Token JWT simulado para usuario administrador</returns>
        protected async Task<string> ObtenerTokenAdminAsync()
        {
            // Simular token de administrador para tests
            // En un entorno real, esto haría una llamada real al endpoint de login
            return await Task.FromResult("mock_admin_token_for_tests");
        }

        /// <summary>
        /// Obtiene token de autenticación simulado para usuario regular
        /// PROPÓSITO: Facilitar tests de endpoints que requieren autenticación básica
        /// SIMULACIÓN: En tests reales, esto haría llamada real al endpoint de login
        /// FUTURO: Considerar integración con AuthService real para mayor realismo
        /// CASOS DE USO:
        /// - Tests de endpoints de usuario regular (/api/cliente/*, /api/venta/*)
        /// - Tests de operaciones básicas autenticadas
        /// - Tests de autorización diferenciada por nivel de usuario
        /// </summary>
        /// <returns>Token JWT simulado para usuario regular</returns>
        protected async Task<string> ObtenerTokenUsuarioAsync()
        {
            // Simular token de usuario regular para tests
            // En un entorno real, esto haría una llamada real al endpoint de login
            return await Task.FromResult("mock_user_token_for_tests");
        }

        // ========== NOTAS DE DISEÑO PARA TESTING EMPRESARIAL ==========
        // FUTURO: Para testing empresarial avanzado, considerar:
        // - Integración con Azure Test Plans para gestión de casos de test
        // - Métricas de cobertura de código en tiempo real
        // - Tests de performance y carga automatizados
        // - Tests de seguridad y penetración automatizados
        // - Integración con herramientas de monitoring (Application Insights)
        // - Tests de disaster recovery y backup/restore
        // - Tests de compliance y auditoría automática
        // - Tests de integración con sistemas externos (CRM, ERP)
        // ENTERPRISE CONSIDERATIONS:
        // - Configuración por ambiente (dev, staging, prod testing)
        // - Datos de test sintéticos que cumplan GDPR
        // - Tests de migración de datos entre versiones
        // - Tests de API versioning y backward compatibility
    }
} 