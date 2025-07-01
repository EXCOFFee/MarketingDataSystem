// ==================== CONTROLADOR REST CRÍTICO DE INGESTA ETL MANUAL ====================
// Este controlador es el gateway HTTP para ejecutar y monitorear el pipeline ETL
// CAPA: Presentation (Web API) - Expone endpoints REST para control manual del ETL
// CRITICIDAD: MÁXIMA - Controla el procesamiento de TODOS los datos empresariales
// PIPELINE ETL: Punto de entrada HTTP para orquestar el proceso completo de datos
// CLEAN ARCHITECTURE: Capa externa que llama a PipelineETLService (Application Layer)
// SCHEDULING: Complementa la ejecución automática diaria (2:00 AM) con control manual

using Microsoft.AspNetCore.Authorization;  // Autorización JWT para operaciones críticas
using Microsoft.AspNetCore.Mvc;           // Framework MVC para controladores REST
using MarketingDataSystem.Application.Services;
using MarketingDataSystem.Core.Interfaces;

namespace MarketingDataSystem.API.Controllers
{
    /// <summary>
    /// Controlador REST para control manual y monitoreo del pipeline ETL empresarial
    /// RESPONSABILIDAD: Exponer API REST para ejecutar y monitorear procesos ETL críticos
    /// ARQUITECTURA: Capa Presentation en Clean Architecture - punto de entrada HTTP
    /// CRITICIDAD: MÁXIMA - Controla el procesamiento de TODOS los datos empresariales
    /// ENDPOINTS EXPUESTOS:
    /// - GET /api/ingestion/status - Estado actual del último proceso ETL
    /// - POST /api/ingestion/start - Ejecuta pipeline ETL de forma manual
    /// PIPELINE ETL COMPLETO (4 ETAPAS):
    /// 1. ValidadorService: Filtra datos válidos de todas las fuentes
    /// 2. TransformadorService: Convierte formatos (JSON/CSV/XML) a estructura normalizada
    /// 3. EnriquecedorService: Agrega valor comercial y context empresarial
    /// 4. DeduplicadorService: Elimina duplicados y consolida información
    /// CASOS DE USO EMPRESARIALES:
    /// - Ejecución manual urgente fuera del schedule nocturno
    /// - Testing y validación después de cambios en fuentes
    /// - Recuperación de datos después de errores de sistema
    /// - Procesamiento ad-hoc para reportes ejecutivos urgentes
    /// - Monitoreo de salud del pipeline para operaciones
    /// - Troubleshooting y diagnóstico de problemas ETL
    /// SCHEDULE AUTOMÁTICO VS MANUAL:
    /// - Automático: ETLSchedulerHostedService ejecuta a las 2:00 AM diariamente
    /// - Manual: Este controlador permite ejecución inmediata bajo demanda
    /// MONITOREO Y AUDITORÍA:
    /// - IngestionLogRepository: Registra todas las ejecuciones (auto y manual)
    /// - Estados: EN_PROCESO, COMPLETADO, ERROR, CANCELADO
    /// - Métricas: Duración, registros procesados, errores, performance
    /// CONCURRENCIA:
    /// - Previene múltiples ejecuciones simultáneas (conflict detection)
    /// - Un solo proceso ETL activo por vez para integridad de datos
    /// EJECUCIÓN ASÍNCRONA:
    /// - Task.Run para no bloquear response HTTP
    /// - Cliente recibe confirmación inmediata de inicio
    /// - Proceso real continúa en background thread
    /// CLEAN ARCHITECTURE: Solo conoce PipelineETLService (Application Layer)
    /// SEGURIDAD: JWT requerido para proteger ejecución de procesos críticos
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // CRÍTICO: Protegido según SRS - operaciones que afectan todos los datos empresariales
    public class IngestionController : ControllerBase
    {
        // ========== DEPENDENCIAS PARA CONTROL DEL PIPELINE ETL ==========
        private readonly PipelineETLService _pipelineETL;                    // Orquestador principal del pipeline
        private readonly IIngestionLogRepository _ingestionLogRepository;    // Auditoría y monitoreo de ejecuciones
        private readonly ILogger<IngestionController> _logger;              // Logging para operaciones críticas

        /// <summary>
        /// Constructor con inyección de dependencias para control del pipeline ETL crítico
        /// PATRÓN: Dependency Injection - servicios inyectados como interfaces testeable
        /// CLEAN ARCHITECTURE: Controller conoce Service (Application) y Repository (Infrastructure)
        /// LOGGING: Logger especializado para auditoría de operaciones ETL críticas
        /// SOLID: Principio D - depende de abstracciones, no implementaciones concretas
        /// </summary>
        /// <param name="pipelineETL">Servicio orquestador del pipeline ETL completo</param>
        /// <param name="ingestionLogRepository">Repositorio para auditoría de ejecuciones ETL</param>
        /// <param name="logger">Logger para auditoría de operaciones críticas del pipeline</param>
        public IngestionController(
            PipelineETLService pipelineETL,
            IIngestionLogRepository ingestionLogRepository,
            ILogger<IngestionController> logger)
        {
            _pipelineETL = pipelineETL;                      // Orquestador del pipeline completo
            _ingestionLogRepository = ingestionLogRepository; // Auditoría de ejecuciones
            _logger = logger;                                // Logging de operaciones críticas
        }

        /// <summary>
        /// Obtiene estado actual del último proceso ETL ejecutado - MONITOREO OPERACIONAL
        /// ENDPOINT: GET /api/ingestion/status
        /// AUTENTICACIÓN: JWT requerido (monitoreo de procesos críticos)
        /// CASO DE USO: Dashboard operacional, health checks, troubleshooting, alertas
        /// INFORMACIÓN RETORNADA:
        /// - Estado actual (EN_PROCESO, COMPLETADO, ERROR, CANCELADO)
        /// - Fechas de inicio y fin de la última ejecución
        /// - Número de registros procesados exitosamente
        /// - Mensajes de error en caso de fallas
        /// - Duración total del proceso
        /// - Indicador si hay proceso ejecutándose actualmente
        /// CASOS ESPECIALES:
        /// - Sin ejecuciones previas: Sistema recién instalado
        /// - Proceso en curso: Ejecución activa desde schedule automático o manual
        /// - Error: Falla en última ejecución (requiere investigación)
        /// AUDITORÍA: Consulta IngestionLogRepository para historial completo
        /// RESPUESTA: Objeto JSON con estado detallado para monitoreo operacional
        /// </summary>
        /// <returns>200 OK con estado detallado del ETL, 500 si error interno</returns>
        [HttpGet("status")]
        public async Task<ActionResult<object>> GetIngestionStatus()
        {
            try
            {
                // ========== CONSULTA DE HISTORIAL DE EJECUCIONES ==========
                var logs = await _ingestionLogRepository.GetAllAsync();
                var ultimoLog = logs.OrderByDescending(l => l.FechaInicio).FirstOrDefault();

                // ========== VALIDACIÓN DE EXISTENCIA DE EJECUCIONES ==========
                if (ultimoLog == null)
                {
                    return Ok(new
                    {
                        estado = "sin_ejecuciones",
                        mensaje = "No se han encontrado ejecuciones previas del proceso ETL"
                    });
                }

                // ========== CONSTRUCCIÓN DE RESPUESTA DETALLADA ==========
                var response = new
                {
                    estado = ultimoLog.Estado,                                    // Estado actual del proceso
                    fechaInicio = ultimoLog.FechaInicio,                         // Timestamp de inicio
                    fechaFin = ultimoLog.FechaFin,                              // Timestamp de finalización (null si en curso)
                    registrosProcesados = ultimoLog.RegistrosProcesados,        // Número de registros procesados
                    mensajeError = ultimoLog.MensajeError,                      // Detalles de error si aplica
                    duracion = ultimoLog.FechaFin.HasValue                      // Duración calculada del proceso
                        ? (TimeSpan?)(ultimoLog.FechaFin.Value - ultimoLog.FechaInicio) 
                        : (TimeSpan?)null,
                    enEjecucion = ultimoLog.FechaFin == null && ultimoLog.Estado == "EN_PROCESO"  // Indicador de proceso activo
                };

                // ========== AUDITORÍA DE CONSULTA DE ESTADO ==========
                _logger.LogInformation("Consultado estado de ingestión: {Estado}", ultimoLog.Estado);
                
                // ========== RESPUESTA HTTP 200 OK CON ESTADO DETALLADO ==========
                return Ok(response); // Estado completo para dashboard operacional
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING PARA MONITOREO CRÍTICO ==========
                _logger.LogError(ex, "Error al consultar estado de ingestión");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Ejecuta pipeline ETL manual - OPERACIÓN CRÍTICA DE PROCESAMIENTO DE DATOS
        /// ENDPOINT: POST /api/ingestion/start
        /// AUTENTICACIÓN: JWT requerido (ejecución de procesos críticos empresariales)
        /// CASO DE USO: Ejecución urgente, testing, recuperación, procesamiento ad-hoc
        /// PIPELINE COMPLETO EJECUTADO:
        /// 1. VALIDACIÓN: ValidadorService filtra datos válidos de todas las fuentes
        /// 2. TRANSFORMACIÓN: TransformadorService normaliza formatos heterogéneos
        /// 3. ENRIQUECIMIENTO: EnriquecedorService agrega valor comercial
        /// 4. DEDUPLICACIÓN: DeduplicadorService elimina duplicados y consolida
        /// 5. REPORTE: GeneradorReporteService crea Excel ejecutivo automáticamente
        /// VALIDACIONES CRÍTICAS:
        /// - Verificar que no hay otro proceso ETL en ejecución (concurrencia)
        /// - Generar ID único para tracking del proceso manual
        /// - Registrar auditoría completa en IngestionLogRepository
        /// EJECUCIÓN ASÍNCRONA:
        /// - Task.Run ejecuta el pipeline en background thread
        /// - Response HTTP inmediata con confirmación de inicio
        /// - Proceso real continúa independientemente de la conexión HTTP
        /// PARÁMETROS OPCIONALES:
        /// - SourceId: Procesar solo fuente específica (futuro)
        /// - FechaDesde: Procesar solo datos desde fecha específica (futuro)
        /// - Tipo: Tipo específico de procesamiento (futuro)
        /// IMPACTO EMPRESARIAL:
        /// - Actualiza TODOS los datos de ventas, stock, clientes
        /// - Genera reportes ejecutivos actualizados automáticamente
        /// - Dispara alertas automáticas si se detectan problemas
        /// - Afecta dashboards y métricas en tiempo real
        /// </summary>
        /// <param name="request">Parámetros opcionales para configurar la ejecución ETL</param>
        /// <returns>200 OK con confirmación de inicio, 409 si proceso en curso, 500 si error</returns>
        [HttpPost("start")]
        public async Task<ActionResult<object>> StartIngestion([FromBody] StartIngestionRequest? request = null)
        {
            try
            {
                // ========== INICIO DE PROCESO MANUAL CRÍTICO ==========
                _logger.LogInformation("Iniciando proceso manual de ingestión ETL");

                // ========== VERIFICACIÓN DE CONCURRENCIA ==========
                // Crítico: Solo un proceso ETL puede ejecutarse a la vez para integridad de datos
                var logs = await _ingestionLogRepository.GetAllAsync();
                var procesoEnCurso = logs.Where(l => l.Estado == "EN_PROCESO" && l.FechaFin == null)
                                       .OrderByDescending(l => l.FechaInicio)
                                       .FirstOrDefault();

                if (procesoEnCurso != null)
                {
                    // ========== RESPUESTA DE CONFLICTO (409) ==========
                    return Conflict(new 
                    { 
                        mensaje = "Ya hay un proceso de ingestión en ejecución",
                        fechaInicio = procesoEnCurso.FechaInicio
                    });
                }

                // ========== GENERACIÓN DE ID ÚNICO PARA TRACKING ==========
                var procesoId = $"manual-{DateTime.Now:yyyyMMdd-HHmmss}";

                // ========== EJECUCIÓN ASÍNCRONA DEL PIPELINE ETL ==========
                // IMPORTANTE: Task.Run permite response HTTP inmediata
                // mientras el pipeline continúa en background
                _ = Task.Run(async () =>
                {
                    try
                    {
                        // ========== EJECUCIÓN DEL PIPELINE COMPLETO ==========
                        // Esto ejecuta las 4 etapas secuenciales:
                        // 1. Validación → 2. Transformación → 3. Enriquecimiento → 4. Deduplicación
                        await _pipelineETL.EjecutarETLAsync();
                        
                        // ========== AUDITORÍA DE PROCESO EXITOSO ==========
                        _logger.LogInformation("Proceso manual ETL {ProcesoId} completado exitosamente", procesoId);
                    }
                    catch (Exception ex)
                    {
                        // ========== AUDITORÍA DE PROCESO FALLIDO ==========
                        _logger.LogError(ex, "Error en proceso manual ETL {ProcesoId}", procesoId);
                    }
                });

                // ========== RESPUESTA INMEDIATA DE CONFIRMACIÓN ==========
                var response = new
                {
                    mensaje = "Proceso de ingestión iniciado correctamente",
                    procesoId = procesoId,                    // ID único para tracking
                    timestamp = DateTime.Now,                // Timestamp de inicio
                    tipo = "manual",                         // Distinguir de automático
                    parametros = request                     // Parámetros de configuración
                };

                // ========== AUDITORÍA DE INICIO DE PROCESO MANUAL ==========
                _logger.LogInformation("Proceso ETL manual iniciado con ID: {ProcesoId}", procesoId);
                
                // ========== RESPUESTA HTTP 200 OK CON CONFIRMACIÓN ==========
                return Ok(response); // Confirmación inmediata de inicio
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING PARA PROCESO CRÍTICO ==========
                _logger.LogError(ex, "Error al iniciar proceso manual de ingestión");
                return StatusCode(500, new 
                { 
                    mensaje = "Error al iniciar el proceso de ingestión",
                    error = ex.Message  // Información para debugging
                });
            }
        }

        // ========== ENDPOINTS FUTUROS PARA GESTIÓN AVANZADA DEL ETL ==========
        // TODO: Implementar endpoints adicionales para funcionalidad empresarial:
        
        /// <summary>
        /// Cancela proceso ETL en ejecución (si está en curso)
        /// POST /api/ingestion/cancel
        /// </summary>
        // [HttpPost("cancel")]
        // public async Task<ActionResult> CancelIngestion()
        // {
        //     // Cancela proceso ETL en curso de forma segura
        //     // Útil para interrumpir procesos problemáticos
        // }

        /// <summary>
        /// Obtiene historial completo de ejecuciones ETL
        /// GET /api/ingestion/history?page=1&pageSize=20
        /// </summary>
        // [HttpGet("history")]
        // public async Task<ActionResult<PagedResult<IngestionLogDto>>> GetIngestionHistory(
        //     [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        // {
        //     // Historial paginado de todas las ejecuciones
        //     // Para análisis de performance y troubleshooting
        // }

        /// <summary>
        /// Obtiene métricas detalladas de performance del ETL
        /// GET /api/ingestion/metrics
        /// </summary>
        // [HttpGet("metrics")]
        // public async Task<ActionResult<ETLMetricsDto>> GetETLMetrics()
        // {
        //     // Métricas de performance: duración promedio, throughput, errores
        //     // Para optimización y monitoreo operacional
        // }

        /// <summary>
        /// Ejecuta solo una etapa específica del pipeline ETL
        /// POST /api/ingestion/execute-stage
        /// </summary>
        // [HttpPost("execute-stage")]
        // public async Task<ActionResult> ExecuteETLStage([FromBody] ETLStageRequest request)
        // {
        //     // Ejecuta solo validación, transformación, enriquecimiento o deduplicación
        //     // Útil para testing y troubleshooting granular
        // }

        /// <summary>
        /// Ejecuta ETL para fuente específica únicamente
        /// POST /api/ingestion/source/{sourceId}
        /// </summary>
        // [HttpPost("source/{sourceId}")]
        // public async Task<ActionResult> ProcessSpecificSource(int sourceId)
        // {
        //     // Procesa solo una fuente de datos específica
        //     // Útil para testing y procesamiento selectivo
        // }

        /// <summary>
        /// Valida configuración antes de ejecutar ETL
        /// POST /api/ingestion/validate-config
        /// </summary>
        // [HttpPost("validate-config")]
        // public async Task<ActionResult<ValidationResultDto>> ValidateETLConfiguration()
        // {
        //     // Valida que todas las fuentes estén configuradas correctamente
        //     // Previene errores en ejecución del pipeline
        // }

        /// <summary>
        /// Programa ejecución ETL para horario específico
        /// POST /api/ingestion/schedule
        /// </summary>
        // [HttpPost("schedule")]
        // public async Task<ActionResult> ScheduleETLExecution([FromBody] ScheduleETLRequest request)
        // {
        //     // Programa ejecución para momento específico
        //     // Complementa el schedule automático diario
        // }

        // ========== CONSIDERACIONES PARA ETL EMPRESARIAL AVANZADO ==========
        // FUTURO: Para sistemas ETL de nivel empresarial considerar:
        // - Procesamiento paralelo de múltiples fuentes
        // - Checkpoints y recuperación automática de errores
        // - Streaming ETL para datos en tiempo real
        // - Delta processing (solo cambios desde última ejecución)
        // - Data lineage tracking para compliance
        // - Performance profiling con métricas detalladas
        // - Circuit breaker para fuentes problemáticas
        // - Rollback automático en caso de errores críticos
        // - Integration con data quality frameworks
        // - Alertas inteligentes basadas en patrones históricos
        // - Auto-scaling basado en volumen de datos
        // - Integración con herramientas de monitoring (Grafana, DataDog)
    }

    /// <summary>
    /// Modelo para solicitud de inicio de ingesta ETL con parámetros de configuración
    /// PROPÓSITO: Encapsular parámetros opcionales para ejecución ETL personalizada
    /// FLEXIBILIDAD: Permite configurar aspectos específicos del procesamiento
    /// CASOS DE USO: Testing, procesamiento selectivo, debugging, optimización
    /// </summary>
    public class StartIngestionRequest
    {
        /// <summary>
        /// ID de fuente específica a procesar (opcional)
        /// CASO DE USO: Procesar solo una fuente para testing o troubleshooting
        /// EJEMPLO: "1" para procesar solo la primera fuente configurada
        /// </summary>
        public string? SourceId { get; set; }
        
        /// <summary>
        /// Fecha desde la cual procesar datos (opcional)
        /// CASO DE USO: Procesamiento incremental o recuperación de datos específicos
        /// EJEMPLO: "2024-01-01" para procesar solo datos desde esa fecha
        /// </summary>
        public DateTime? FechaDesde { get; set; }
        
        /// <summary>
        /// Tipo específico de procesamiento a ejecutar (opcional)
        /// CASOS DE USO: Ejecutar solo ciertas etapas del pipeline
        /// EJEMPLOS: "validation", "transformation", "enrichment", "deduplication", "full"
        /// </summary>
        public string? Tipo { get; set; }
    }
} 