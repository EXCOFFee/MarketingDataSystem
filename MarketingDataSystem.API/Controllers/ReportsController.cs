// ==================== CONTROLADOR REST CRÍTICO DE REPORTES EJECUTIVOS ====================
// Este controlador expone reportes automáticos generados por el pipeline ETL
// CAPA: Presentation (Web API) - Punto de acceso REST para reportes empresariales
// CRITICIDAD: ALTA - Proporciona información ejecutiva para toma de decisiones
// INTEGRACIÓN: Conectado con GeneradorReporteService y pipeline ETL automático
// CLEAN ARCHITECTURE: Capa externa que expone reportes generados por Application Layer
// ARCHIVOS: Gestiona archivos Excel físicos en directorio /reportes

using Microsoft.AspNetCore.Authorization;  // Autorización JWT para reportes sensibles
using Microsoft.AspNetCore.Mvc;           // Framework MVC para controladores REST
using MarketingDataSystem.Application.Interfaces;
using MarketingDataSystem.Core.Interfaces;

namespace MarketingDataSystem.API.Controllers
{
    /// <summary>
    /// Controlador REST para gestión y acceso a reportes ejecutivos empresariales
    /// RESPONSABILIDAD: Exponer API REST para consulta y generación de reportes críticos
    /// ARQUITECTURA: Capa Presentation en Clean Architecture - punto de entrada HTTP
    /// CRITICIDAD: ALTA - Proporciona información ejecutiva para decisiones estratégicas
    /// ENDPOINTS EXPUESTOS:
    /// - GET /api/reports - Lista reportes disponibles con metadatos completos
    /// - POST /api/reports/generate - Genera reporte manual para testing/urgencias
    /// INTEGRACIÓN CON PIPELINE ETL:
    /// - Reportes automáticos: Generados cada día después del ETL (2:30 AM)
    /// - EventBus: Escucha evento 'CargaFinalizada' para generación automática
    /// - GeneradorReporteService: Crea archivos Excel multi-hoja con datos procesados
    /// TIPOS DE REPORTES GENERADOS:
    /// - Reporte de Ventas: Análisis completo de transacciones y revenue
    /// - Reporte de Stock: Estado de inventario y alertas de reabastecimiento
    /// - Reporte de Clientes: Segmentación y análisis de comportamiento
    /// - Reporte Ejecutivo: Dashboard consolidado con KPIs principales
    /// CASOS DE USO EMPRESARIALES:
    /// - Dashboard ejecutivo para CEO/CFO con métricas críticas
    /// - Análisis financiero mensual y trimestral
    /// - Reportes regulatorios y compliance para auditorías
    /// - Distribución automática por email a stakeholders
    /// - Análisis de tendencias y forecasting
    /// - Monitoreo de performance operacional
    /// ARCHIVOS FÍSICOS:
    /// - Ubicación: /reportes/ en servidor
    /// - Formato: Excel (.xlsx) con múltiples hojas
    /// - Nomenclatura: ReporteVentas_YYYY-MM-DD_HHMM.xlsx
    /// - Retención: Configurable (default: 90 días)
    /// SEGURIDAD:
    /// - JWT requerido para acceso a reportes ejecutivos
    /// - Información financiera y operacional sensible
    /// - Auditoría de descargas para compliance
    /// CLEAN ARCHITECTURE: Solo conoce Application Layer (servicios de reportes)
    /// PERFORMANCE: Archivos pre-generados para respuesta rápida
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // CRÍTICO: Protegido según SRS - reportes ejecutivos contienen información sensible
    public class ReportsController : ControllerBase
    {
        // ========== DEPENDENCIAS PARA GESTIÓN DE REPORTES ==========
        private readonly IReporteRepository _reporteRepository;     // Repositorio para metadatos de reportes
        private readonly IGeneradorReporteService _generadorReporte; // Servicio para generación de Excel
        private readonly ILogger<ReportsController> _logger;        // Logging para auditoría de acceso

        /// <summary>
        /// Constructor con inyección de dependencias para gestión de reportes ejecutivos
        /// PATRÓN: Dependency Injection - servicios inyectados como interfaces testeable
        /// CLEAN ARCHITECTURE: Controller conoce Repository (directo) y Service (Application)
        /// LOGGING: Logger especializado para auditoría de acceso a reportes
        /// SOLID: Principio D - depende de abstracciones, no implementaciones concretas
        /// </summary>
        /// <param name="reporteRepository">Repositorio para metadatos de reportes generados</param>
        /// <param name="generadorReporte">Servicio para generación de archivos Excel</param>
        /// <param name="logger">Logger para auditoría de acceso a reportes ejecutivos</param>
        public ReportsController(
            IReporteRepository reporteRepository,
            IGeneradorReporteService generadorReporte,
            ILogger<ReportsController> logger)
        {
            _reporteRepository = reporteRepository;  // Acceso a metadatos de reportes
            _generadorReporte = generadorReporte;    // Generación de archivos Excel
            _logger = logger;                        // Auditoría de acceso ejecutivo
        }

        /// <summary>
        /// Lista todos los reportes generados disponibles con metadatos completos
        /// ENDPOINT: GET /api/reports
        /// AUTENTICACIÓN: JWT requerido (acceso a reportes ejecutivos sensibles)
        /// CASO DE USO: Dashboard de reportes, selección para descarga, auditoría
        /// INFORMACIÓN RETORNADA:
        /// - ID único del reporte
        /// - Nombre descriptivo del reporte
        /// - Fecha y hora de generación
        /// - Ruta del archivo Excel físico
        /// - Descripción del contenido
        /// ORDENAMIENTO: Por fecha de generación (más recientes primero)
        /// PERFORMANCE: Solo metadatos, no archivos completos
        /// AUDITORÍA: Registra consultas para monitoreo de acceso
        /// </summary>
        /// <returns>200 OK con lista de metadatos de reportes, 500 si error</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetReports()
        {
            try
            {
                // ========== CONSULTA DE METADATOS DE REPORTES ==========
                var reportes = await _reporteRepository.GetAllAsync();
                
                // ========== MAPEO A DTO CON INFORMACIÓN EJECUTIVA ==========
                var reportesDto = reportes.Select(r => new
                {
                    id = r.IdReporte,                    // ID único para referencias
                    nombre = r.Nombre,                   // Nombre descriptivo del reporte
                    fecha = r.FechaGeneracion,          // Timestamp de generación
                    ruta = r.RutaArchivo,               // Path del archivo Excel físico
                    descripcion = r.Descripcion         // Descripción del contenido
                }).ToList();

                // ========== AUDITORÍA DE CONSULTA DE REPORTES EJECUTIVOS ==========
                _logger.LogInformation("Se consultaron {Count} reportes disponibles", reportesDto.Count);
                
                // ========== RESPUESTA HTTP 200 OK CON METADATOS ==========
                return Ok(reportesDto); // Lista de reportes disponibles para descarga
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING CON LOGGING DETALLADO ==========
                _logger.LogError(ex, "Error al consultar reportes disponibles");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Fuerza generación manual de reporte Excel - OPERACIÓN DE EMERGENCIA
        /// ENDPOINT: POST /api/reports/generate
        /// AUTENTICACIÓN: JWT requerido (generación manual de reportes)
        /// CASO DE USO: Emergencias, testing, reportes ad-hoc fuera del schedule
        /// PROCESAMIENTO:
        /// - Ejecuta GeneradorReporteService inmediatamente
        /// - Procesa datos actuales en tiempo real
        /// - Genera archivo Excel multi-hoja
        /// - Almacena en directorio /reportes/
        /// - Registra metadatos en base de datos
        /// DIFERENCIA CON AUTOMÁTICO:
        /// - Automático: Triggered por EventBus después del ETL (2:30 AM)
        /// - Manual: Triggered por this endpoint en cualquier momento
        /// PERFORMANCE: Operación intensiva, puede tomar varios segundos
        /// AUDITORÍA: Registro crítico para tracking de generaciones manuales
        /// USO RECOMENDADO: Solo para emergencias o testing
        /// </summary>
        /// <returns>200 OK con confirmación de generación, 500 si error</returns>
        [HttpPost("generate")]
        public ActionResult<object> GenerateReport()
        {
            try
            {
                // ========== INICIO DE GENERACIÓN MANUAL ==========
                _logger.LogInformation("Iniciando generación manual de reporte Excel");
                
                // ========== EJECUCIÓN DEL GENERADOR DE REPORTES ==========
                // NOTA: Esta operación puede tomar varios segundos
                // - Consulta datos de ventas, stock, clientes
                // - Procesa métricas y KPIs
                // - Genera archivo Excel multi-hoja
                // - Almacena en directorio físico
                _generadorReporte.GenerarReporte();
                
                // ========== RESPUESTA DE CONFIRMACIÓN ==========
                var response = new
                {
                    mensaje = "Reporte Excel generado correctamente",
                    timestamp = DateTime.Now,            // Timestamp de generación
                    tipo = "manual"                      // Distinguir de automático
                };

                // ========== AUDITORÍA DE GENERACIÓN MANUAL EXITOSA ==========
                _logger.LogInformation("Reporte generado manualmente con éxito");
                
                // ========== RESPUESTA HTTP 200 OK CON CONFIRMACIÓN ==========
                return Ok(response); // Confirmación de generación exitosa
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING PARA GENERACIÓN CRÍTICA ==========
                _logger.LogError(ex, "Error durante la generación manual del reporte");
                return StatusCode(500, new 
                { 
                    mensaje = "Error al generar el reporte", 
                    error = ex.Message  // Información del error para debugging
                });
            }
        }

        // ========== ENDPOINTS FUTUROS PARA SISTEMA DE REPORTES EMPRESARIAL ==========
        // TODO: Implementar endpoints adicionales para funcionalidad empresarial:
        
        /// <summary>
        /// Descarga archivo Excel específico por ID
        /// GET /api/reports/{id}/download
        /// </summary>
        // [HttpGet("{id}/download")]
        // public async Task<ActionResult> DownloadReport(int id)
        // {
        //     // Descarga directa del archivo Excel con headers apropiados
        //     // Content-Type: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
        //     // Content-Disposition: attachment; filename="ReporteVentas_2024-01-15.xlsx"
        // }

        /// <summary>
        /// Programa generación automática de reporte personalizado
        /// POST /api/reports/schedule
        /// </summary>
        // [HttpPost("schedule")]
        // public async Task<ActionResult> ScheduleReport([FromBody] ScheduleReportRequest request)
        // {
        //     // Programar reportes personalizados con parámetros específicos
        //     // Frecuencia, filtros, destinatarios, formato
        // }

        /// <summary>
        /// Obtiene preview/resumen de reporte sin generar archivo completo
        /// GET /api/reports/preview
        /// </summary>
        // [HttpGet("preview")]
        // public async Task<ActionResult<ReportPreviewDto>> GetReportPreview()
        // {
        //     // Vista previa con métricas principales sin generar Excel completo
        //     // Útil para dashboards en tiempo real
        // }

        /// <summary>
        /// Envía reporte por email a lista de destinatarios
        /// POST /api/reports/{id}/email
        /// </summary>
        // [HttpPost("{id}/email")]
        // public async Task<ActionResult> EmailReport(int id, [FromBody] EmailReportRequest request)
        // {
        //     // Envío automático de reportes por email
        //     // Integración con servicio de email empresarial
        // }

        /// <summary>
        /// Obtiene estadísticas de uso de reportes
        /// GET /api/reports/analytics
        /// </summary>
        // [HttpGet("analytics")]
        // public async Task<ActionResult<ReportAnalyticsDto>> GetReportAnalytics()
        // {
        //     // Métricas de uso: reportes más descargados, frecuencia, usuarios
        //     // Útil para optimización y decisiones de producto
        // }

        /// <summary>
        /// Elimina reportes antiguos según política de retención
        /// DELETE /api/reports/cleanup
        /// </summary>
        // [HttpDelete("cleanup")]
        // public async Task<ActionResult<CleanupResultDto>> CleanupOldReports([FromQuery] int daysToKeep = 90)
        // {
        //     // Limpieza automática de archivos antiguos
        //     // Libera espacio en disco manteniendo compliance
        // }

        /// <summary>
        /// Genera reporte personalizado con filtros específicos
        /// POST /api/reports/custom
        /// </summary>
        // [HttpPost("custom")]
        // public async Task<ActionResult> GenerateCustomReport([FromBody] CustomReportRequest request)
        // {
        //     // Reportes ad-hoc con parámetros personalizados
        //     // Fechas, productos, clientes, métricas específicas
        // }

        // ========== CONSIDERACIONES PARA SISTEMA DE REPORTES EMPRESARIAL ==========
        // FUTURO: Para sistema de reportes de nivel empresarial considerar:
        // - Generación asíncrona para reportes grandes (background jobs)
        // - Cache inteligente para reportes frecuentes
        // - Múltiples formatos de salida (PDF, CSV, JSON, Power BI)
        // - Reportes interactivos con drill-down capabilities
        // - Integración con herramientas BI (Tableau, Power BI, Qlik)
        // - API de suscripción para reportes automáticos
        // - Versionado de reportes para tracking de cambios
        // - Compression automática para archivos grandes
        // - Integración con cloud storage (Azure Blob, AWS S3)
        // - Real-time dashboards con SignalR
        // - Machine Learning para insights automáticos
        // - Colaboración y comentarios en reportes
        // - Compliance y retención según regulaciones específicas
    }
} 