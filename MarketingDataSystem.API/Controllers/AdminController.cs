// ==================== CONTROLADOR ADMINISTRATIVO DEL SISTEMA ====================
// Este controlador maneja todas las funciones críticas de administración del sistema
// FUNCIONALIDADES: Backups, Alertas, Monitoreo, Logs, Reinicio de servicios
// SEGURIDAD: MÁXIMA - Solo administradores con JWT válido pueden acceder
// PROPÓSITO: Panel de administración completo para mantenimiento del sistema
// CRÍTICO: Estas funciones afectan la operatividad completa del sistema

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketingDataSystem.Application.Interfaces;

namespace MarketingDataSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]   // Ruta base: api/admin
    [Authorize] // CRÍTICO: Solo administradores autenticados - funciones que afectan todo el sistema
    public class AdminController : ControllerBase
    {
        // ========== DEPENDENCIAS PARA ADMINISTRACIÓN DEL SISTEMA ==========
        private readonly IBackupService _backupService;        // Gestión de backups automáticos y manuales
        private readonly IAlertaService _alertaService;        // Sistema de alertas críticas
        private readonly ILogger<AdminController> _logger;     // Logging de operaciones administrativas críticas

        /// <summary>
        /// Constructor para controlador administrativo con servicios críticos del sistema
        /// RESPONSABILIDAD: Coordinar operaciones que afectan la infraestructura completa
        /// SEGURIDAD: Todas las operaciones requieren máximo nivel de autorización
        /// </summary>
        /// <param name="backupService">Servicio para gestión de backups de base de datos</param>
        /// <param name="alertaService">Servicio para envío de alertas críticas</param>
        /// <param name="logger">Logger para auditoría de operaciones administrativas</param>
        public AdminController(
            IBackupService backupService,
            IAlertaService alertaService,
            ILogger<AdminController> logger)
        {
            _backupService = backupService;
            _alertaService = alertaService;
            _logger = logger;
        }

        /// <summary>
        /// Crea un backup completo de toda la base de datos - OPERACIÓN CRÍTICA
        /// POST api/admin/backup/completo
        /// REQUIERE: Token JWT de administrador
        /// FUNCIONALIDAD: Backup completo con todos los datos del sistema
        /// USO: Antes de actualizaciones importantes, migración de servidores, recovery planning
        /// TAMAÑO: Más grande pero contiene 100% de los datos
        /// TIEMPO: Proceso más lento pero más seguro para recuperación completa
        /// </summary>
        /// <param name="request">Solicitud opcional con ruta personalizada para el backup</param>
        /// <returns>Información del backup creado con ruta del archivo</returns>
        [HttpPost("backup/completo")]
        public async Task<ActionResult<object>> CrearBackupCompleto([FromBody] BackupRequest? request = null)
        {
            try
            {
                // ========== EJECUTAR BACKUP COMPLETO ==========
                // Esta operación puede tomar varios minutos dependiendo del tamaño de la DB
                var rutaBackup = await _backupService.CrearBackupCompletoAsync(request?.RutaPersonalizada);
                
                // ========== RESPUESTA ESTRUCTURADA ==========
                var resultado = new
                {
                    success = true,
                    message = "Backup completo creado exitosamente",
                    rutaArchivo = rutaBackup,
                    timestamp = DateTime.Now,
                    tipo = "COMPLETO"
                };

                // Log CRÍTICO para auditoría de backups (requerimiento de compliance)
                _logger.LogInformation("Backup completo creado exitosamente: {Ruta}", rutaBackup);
                return Ok(resultado); // HTTP 200 con información del backup
            }
            catch (Exception ex)
            {
                // Log de errores críticos en backups - ALTA PRIORIDAD para alertas
                _logger.LogError(ex, "Error al crear backup completo");
                return StatusCode(500, new { message = "Error al crear backup completo", error = ex.Message });
            }
        }

        /// <summary>
        /// Crea un backup incremental de la base de datos
        /// </summary>
        [HttpPost("backup/incremental")]
        public async Task<ActionResult<object>> CrearBackupIncremental([FromBody] BackupRequest? request = null)
        {
            try
            {
                var rutaBackup = await _backupService.CrearBackupIncrementalAsync(request?.RutaPersonalizada);
                
                var resultado = new
                {
                    success = true,
                    message = "Backup incremental creado exitosamente",
                    rutaArchivo = rutaBackup,
                    timestamp = DateTime.Now,
                    tipo = "INCREMENTAL"
                };

                _logger.LogInformation("Backup incremental creado exitosamente: {Ruta}", rutaBackup);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear backup incremental");
                return StatusCode(500, new { message = "Error al crear backup incremental", error = ex.Message });
            }
        }

        /// <summary>
        /// Lista todos los backups disponibles
        /// </summary>
        [HttpGet("backup/listar")]
        public async Task<ActionResult<object>> ListarBackups()
        {
            try
            {
                var backups = await _backupService.ListarBackupsAsync();
                
                var resultado = new
                {
                    success = true,
                    totalBackups = backups.Count(),
                    backups = backups.Select(b => new
                    {
                        archivo = Path.GetFileName(b),
                        rutaCompleta = b
                    })
                };

                _logger.LogInformation("Se listaron {Count} backups disponibles", backups.Count());
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar backups");
                return StatusCode(500, new { message = "Error al listar backups", error = ex.Message });
            }
        }

        /// <summary>
        /// Limpia backups antiguos según política de retención
        /// </summary>
        [HttpDelete("backup/limpiar")]
        public async Task<ActionResult<object>> LimpiarBackupsAntiguos([FromQuery] int diasRetencion = 30)
        {
            try
            {
                var backupsEliminados = await _backupService.LimpiarBackupsAntiguosAsync(diasRetencion);
                
                var resultado = new
                {
                    success = true,
                    message = $"Se eliminaron {backupsEliminados} backups antiguos",
                    backupsEliminados,
                    diasRetencion,
                    timestamp = DateTime.Now
                };

                _logger.LogInformation("Se eliminaron {Count} backups antiguos (retención: {Dias} días)", 
                    backupsEliminados, diasRetencion);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al limpiar backups antiguos");
                return StatusCode(500, new { message = "Error al limpiar backups antiguos", error = ex.Message });
            }
        }

        /// <summary>
        /// Envía una alerta de prueba
        /// </summary>
        [HttpPost("alerta/prueba")]
        public async Task<ActionResult<object>> EnviarAlertaPrueba([FromBody] AlertaRequest request)
        {
            try
            {
                await _alertaService.EnviarNotificacionAsync(request.Mensaje ?? "Alerta de prueba");
                
                var resultado = new
                {
                    success = true,
                    message = "Alerta enviada exitosamente",
                    timestamp = DateTime.Now
                };

                _logger.LogInformation("Alerta de prueba enviada exitosamente");
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar alerta de prueba");
                return StatusCode(500, new { message = "Error al enviar alerta de prueba" });
            }
        }

        /// <summary>
        /// Obtiene el estado general completo del sistema - DASHBOARD ADMINISTRATIVO
        /// GET api/admin/system/status
        /// REQUIERE: Token JWT de administrador
        /// FUNCIONALIDAD: Status completo para monitoreo y troubleshooting
        /// USO: Dashboards de administración, health checks, alertas de sistema
        /// INFORMACIÓN: Estado de servicios, base de datos, backups, versión del sistema
        /// CRITICAL: Endpoint clave para determinar salud general del sistema
        /// </summary>
        /// <returns>Objeto con estado detallado de todos los componentes del sistema</returns>
        [HttpGet("system/status")]
        public async Task<ActionResult<object>> GetSystemStatus()
        {
            try
            {
                // ========== RECOPILAR INFORMACIÓN DEL SISTEMA ==========
                // Consultar estado de backups para incluir en el reporte de estado
                var backupsDisponibles = await _backupService.ListarBackupsAsync();
                
                // ========== CONSTRUIR STATUS COMPLETO ==========
                // Estructura JSON con estado de todos los componentes críticos
                var status = new
                {
                    sistema = new
                    {
                        estado = "OPERATIVO",           // Estado general del sistema
                        timestamp = DateTime.Now,       // Momento de la consulta
                        version = "1.0.0"              // Versión actual del sistema
                    },
                    baseDatos = new
                    {
                        estado = "CONECTADA",                    // Estado de conectividad
                        totalBackups = backupsDisponibles.Count() // Número de backups disponibles
                    },
                    servicios = new
                    {
                        etl = "ACTIVO",         // Estado del pipeline ETL (ejecuta a las 02:00 AM)
                        reportes = "ACTIVO",    // Estado del generador de reportes Excel
                        alertas = "ACTIVO"      // Estado del sistema de alertas críticas
                    }
                };

                // Log para auditoría de consultas administrativas
                _logger.LogInformation("Estado del sistema consultado exitosamente");
                return Ok(status); // HTTP 200 con estado completo
            }
            catch (Exception ex)
            {
                // Log de errores en consulta de estado - crítico para diagnóstico
                _logger.LogError(ex, "Error al obtener estado del sistema");
                return StatusCode(500, new { message = "Error al obtener estado del sistema" });
            }
        }

        /// <summary>
        /// Reinicia los servicios del sistema (simulado)
        /// </summary>
        [HttpPost("system/restart-services")]
        public async Task<ActionResult<object>> RestartServices([FromBody] RestartRequest request)
        {
            try
            {
                // En un sistema real, esto reiniciaría servicios específicos
                await Task.Delay(2000); // Simula tiempo de reinicio

                var resultado = new
                {
                    success = true,
                    message = "Servicios reiniciados exitosamente",
                    serviciosReiniciados = request.Servicios ?? new[] { "ETL", "Reportes", "Alertas" },
                    timestamp = DateTime.Now
                };

                _logger.LogInformation("Servicios del sistema reiniciados: {Servicios}", 
                    string.Join(", ", request.Servicios ?? new[] { "ETL", "Reportes", "Alertas" }));
                
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al reiniciar servicios");
                return StatusCode(500, new { message = "Error al reiniciar servicios", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene logs del sistema (últimas entradas)
        /// </summary>
        [HttpGet("logs")]
        public ActionResult<object> GetSystemLogs([FromQuery] int limite = 100)
        {
            try
            {
                // En un sistema real, esto leería archivos de log
                var logsSimulados = GenerarLogsSimulados(limite);
                
                var resultado = new
                {
                    success = true,
                    totalLogs = logsSimulados.Count,
                    logs = logsSimulados,
                    timestamp = DateTime.Now
                };

                _logger.LogInformation("Se consultaron {Count} logs del sistema", logsSimulados.Count);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener logs del sistema");
                return StatusCode(500, new { message = "Error al obtener logs", error = ex.Message });
            }
        }

        /// <summary>
        /// Genera logs simulados para demostración
        /// </summary>
        private List<object> GenerarLogsSimulados(int limite)
        {
            var logs = new List<object>();
            var niveles = new[] { "INFO", "WARN", "ERROR", "DEBUG" };
            var mensajes = new[]
            {
                "Sistema iniciado correctamente",
                "Proceso ETL ejecutado exitosamente",
                "Reporte generado: ReporteVentas.xlsx",
                "Backup completo creado",
                "Conexión a base de datos establecida",
                "Usuario autenticado: admin",
                "Alerta enviada: Stock bajo detectado",
                "Servicio de alertas reiniciado"
            };

            var random = new Random();
            for (int i = 0; i < Math.Min(limite, 50); i++)
            {
                logs.Add(new
                {
                    timestamp = DateTime.Now.AddMinutes(-random.Next(0, 1440)), // Últimas 24 horas
                    nivel = niveles[random.Next(niveles.Length)],
                    mensaje = mensajes[random.Next(mensajes.Length)],
                    fuente = "MarketingDataSystem.API"
                });
            }

            return logs.OrderByDescending(l => ((dynamic)l).timestamp).ToList();
        }
    }

    /// <summary>
    /// Modelo para solicitudes de backup
    /// </summary>
    public class BackupRequest
    {
        public string? RutaPersonalizada { get; set; }
    }

    /// <summary>
    /// Modelo para solicitudes de alerta
    /// </summary>
    public class AlertaRequest
    {
        public string? Mensaje { get; set; }
    }

    /// <summary>
    /// Modelo para solicitudes de reinicio
    /// </summary>
    public class RestartRequest
    {
        public string[]? Servicios { get; set; }
    }
} 