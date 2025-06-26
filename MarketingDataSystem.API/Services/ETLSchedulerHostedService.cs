// ==================== SERVICIO PROGRAMADO ETL (BACKGROUND SERVICE) ====================
// Este servicio ejecuta automáticamente el pipeline ETL todos los días a las 02:00 AM
// Incluye: Sistema de reintentos, logging detallado, alertas críticas y manejo de errores
// PATRÓN: Background Service de .NET para tareas programadas de larga duración

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using MarketingDataSystem.Application.Services;
using MarketingDataSystem.Application.Interfaces;

namespace MarketingDataSystem.API.Services
{
    /// <summary>
    /// Servicio en segundo plano que programa y ejecuta automáticamente el proceso ETL
    /// Hereda de BackgroundService - patrón estándar de .NET para servicios de larga duración
    /// Se registra en Program.cs como HostedService y se ejecuta automáticamente al iniciar la API
    /// </summary>
    public class ETLSchedulerHostedService : BackgroundService
    {
        // ========== DEPENDENCIAS INYECTADAS ==========
        private readonly IServiceProvider _serviceProvider;           // Para crear scopes y resolver servicios
        private readonly ILogger<ETLSchedulerHostedService> _logger;  // Logging estructurado para monitoreo

        // ========== CONFIGURACIÓN DE EJECUCIÓN ==========
        private readonly TimeSpan _scheduledTime = new TimeSpan(2, 0, 0); // 02:00 AM - Ventana crítica óptima (baja actividad)
        private const int MaxRetries = 3;                // Máximo 3 intentos si falla el ETL
        private const int RetryDelayMinutes = 10;       // Esperar 10 minutos entre reintentos

        /// <summary>
        /// Constructor con inyección de dependencias
        /// NOTA: Solo inyectamos IServiceProvider porque este servicio es singleton
        /// pero necesita crear scopes para servicios scoped como PipelineETLService
        /// </summary>
        /// <param name="serviceProvider">Proveedor de servicios para crear scopes</param>
        /// <param name="logger">Logger para auditoría y debugging</param>
        public ETLSchedulerHostedService(IServiceProvider serviceProvider, ILogger<ETLSchedulerHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <summary>
        /// Método principal que se ejecuta automáticamente en segundo plano
        /// Override del método abstracto de BackgroundService
        /// FUNCIONAMIENTO: Calcula próxima ejecución → Espera → Ejecuta ETL → Repite infinitamente
        /// </summary>
        /// <param name="stoppingToken">Token para cancelación graceful del servicio</param>
        /// <returns>Task que representa la ejecución continua del servicio</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // ========== LOOP INFINITO DE PROGRAMACIÓN ==========
            // Este loop se ejecuta mientras la aplicación esté activa
            while (!stoppingToken.IsCancellationRequested)
            {
                // ========== CÁLCULO DE PRÓXIMA EJECUCIÓN ==========
                var now = DateTime.Now;
                var nextRun = now.Date.Add(_scheduledTime);  // Hoy a las 02:00 AM
                
                // Si ya pasaron las 02:00 AM de hoy, programar para mañana
                if (now > nextRun)
                    nextRun = nextRun.AddDays(1);
                
                var delay = nextRun - now;  // Tiempo hasta próxima ejecución
                _logger.LogInformation($"ETL Scheduler esperando hasta {nextRun} para ejecutar el proceso ETL.");
                
                // ========== ESPERA HASTA LA HORA PROGRAMADA ==========
                // Task.Delay respeta el CancellationToken para shutdown graceful
                await Task.Delay(delay, stoppingToken);

                // ========== SISTEMA DE REINTENTOS CON RESILIENCIA ==========
                int attempt = 0;
                bool success = false;
                
                // Loop de reintentos hasta éxito o máximo de intentos
                while (attempt < MaxRetries && !success && !stoppingToken.IsCancellationRequested)
                {
                    attempt++;
                    try
                    {
                        // ========== EJECUCIÓN DEL PIPELINE ETL ==========
                        // Crear scope para servicios scoped (Entity Framework, etc.)
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var etlService = scope.ServiceProvider.GetRequiredService<PipelineETLService>();
                            _logger.LogInformation($"Iniciando proceso ETL. Intento {attempt}.");
                            
                            // Ejecutar el pipeline completo: Extract → Transform → Load
                            await etlService.EjecutarETLAsync();
                            
                            _logger.LogInformation("Proceso ETL completado exitosamente.");
                            success = true;  // Marcar como exitoso para salir del loop
                        }
                    }
                    catch (Exception ex)
                    {
                        // ========== MANEJO DE ERRORES Y REINTENTOS ==========
                        _logger.LogError(ex, $"Error en el proceso ETL. Intento {attempt} de {MaxRetries}.");
                        
                        if (attempt < MaxRetries)
                        {
                            // Esperar antes del siguiente intento
                            _logger.LogInformation($"Reintentando en {RetryDelayMinutes} minutos...");
                            await Task.Delay(TimeSpan.FromMinutes(RetryDelayMinutes), stoppingToken);
                        }
                        else
                        {
                            // ========== ALERTA CRÍTICA DESPUÉS DE FALLOS ==========
                            _logger.LogError("Se alcanzó el número máximo de reintentos para el proceso ETL.");
                            
                            // Enviar alerta crítica al administrador del sistema
                            using (var alertScope = _serviceProvider.CreateScope())
                            {
                                var alertaService = alertScope.ServiceProvider.GetService<IAlertaService>();
                                if (alertaService != null)
                                {
                                    await alertaService.EnviarAlertaCriticaAsync(
                                        "FALLO CRÍTICO: Proceso ETL falló después de todos los reintentos",
                                        $"El proceso ETL falló {MaxRetries} veces consecutivas. Última excepción: {ex.Message}"
                                    );
                                }
                            }
                        }
                    }
                }
            }
        }
    }
} 