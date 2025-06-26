using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketingDataSystem.Application.Services;
using MarketingDataSystem.Core.Interfaces;

namespace MarketingDataSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protegido según SRS
    public class IngestionController : ControllerBase
    {
        private readonly PipelineETLService _pipelineETL;
        private readonly IIngestionLogRepository _ingestionLogRepository;
        private readonly ILogger<IngestionController> _logger;

        public IngestionController(
            PipelineETLService pipelineETL,
            IIngestionLogRepository ingestionLogRepository,
            ILogger<IngestionController> logger)
        {
            _pipelineETL = pipelineETL;
            _ingestionLogRepository = ingestionLogRepository;
            _logger = logger;
        }

        /// <summary>
        /// Devuelve el estado actual de la última ejecución del proceso
        /// </summary>
        [HttpGet("status")]
        public async Task<ActionResult<object>> GetIngestionStatus()
        {
            try
            {
                // Obtener el último log de ingestión
                var logs = await _ingestionLogRepository.GetAllAsync();
                var ultimoLog = logs.OrderByDescending(l => l.FechaInicio).FirstOrDefault();

                if (ultimoLog == null)
                {
                    return Ok(new
                    {
                        estado = "sin_ejecuciones",
                        mensaje = "No se han encontrado ejecuciones previas del proceso ETL"
                    });
                }

                var response = new
                {
                    estado = ultimoLog.Estado,
                    fechaInicio = ultimoLog.FechaInicio,
                    fechaFin = ultimoLog.FechaFin,
                    registrosProcesados = ultimoLog.RegistrosProcesados,
                    mensajeError = ultimoLog.MensajeError,
                    duracion = ultimoLog.FechaFin.HasValue 
                        ? (TimeSpan?)(ultimoLog.FechaFin.Value - ultimoLog.FechaInicio) 
                        : (TimeSpan?)null,
                    enEjecucion = ultimoLog.FechaFin == null && ultimoLog.Estado == "EN_PROCESO"
                };

                _logger.LogInformation("Consultado estado de ingestión: {Estado}", ultimoLog.Estado);
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar estado de ingestión");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Dispara manualmente el proceso completo de ingestión (testing/manual)
        /// </summary>
        [HttpPost("start")]
        public async Task<ActionResult<object>> StartIngestion([FromBody] StartIngestionRequest? request = null)
        {
            try
            {
                _logger.LogInformation("Iniciando proceso manual de ingestión ETL");

                // Verificar si ya hay un proceso en ejecución
                var logs = await _ingestionLogRepository.GetAllAsync();
                var procesoEnCurso = logs.Where(l => l.Estado == "EN_PROCESO" && l.FechaFin == null)
                                       .OrderByDescending(l => l.FechaInicio)
                                       .FirstOrDefault();

                if (procesoEnCurso != null)
                {
                    return Conflict(new 
                    { 
                        mensaje = "Ya hay un proceso de ingestión en ejecución",
                        fechaInicio = procesoEnCurso.FechaInicio
                    });
                }

                // Generar ID único para este proceso
                var procesoId = $"manual-{DateTime.Now:yyyyMMdd-HHmmss}";

                // Iniciar proceso ETL de forma asíncrona
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _pipelineETL.EjecutarETLAsync();
                        _logger.LogInformation("Proceso manual ETL {ProcesoId} completado exitosamente", procesoId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error en proceso manual ETL {ProcesoId}", procesoId);
                    }
                });

                var response = new
                {
                    mensaje = "Proceso de ingestión iniciado correctamente",
                    procesoId = procesoId,
                    timestamp = DateTime.Now,
                    tipo = "manual",
                    parametros = request
                };

                _logger.LogInformation("Proceso ETL manual iniciado con ID: {ProcesoId}", procesoId);
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar proceso manual de ingestión");
                return StatusCode(500, new 
                { 
                    mensaje = "Error al iniciar el proceso de ingestión",
                    error = ex.Message 
                });
            }
        }
    }

    /// <summary>
    /// Modelo para la solicitud de inicio de ingestión
    /// </summary>
    public class StartIngestionRequest
    {
        public string? SourceId { get; set; }
        public DateTime? FechaDesde { get; set; }
        public string? Tipo { get; set; }
    }
} 