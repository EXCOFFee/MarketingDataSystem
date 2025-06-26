using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketingDataSystem.Application.Interfaces;
using MarketingDataSystem.Core.Interfaces;

namespace MarketingDataSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protegido según SRS
    public class ReportsController : ControllerBase
    {
        private readonly IReporteRepository _reporteRepository;
        private readonly IGeneradorReporteService _generadorReporte;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(
            IReporteRepository reporteRepository,
            IGeneradorReporteService generadorReporte,
            ILogger<ReportsController> logger)
        {
            _reporteRepository = reporteRepository;
            _generadorReporte = generadorReporte;
            _logger = logger;
        }

        /// <summary>
        /// Lista los reportes generados (nombre, fecha, ruta)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetReports()
        {
            try
            {
                var reportes = await _reporteRepository.GetAllAsync();
                
                var reportesDto = reportes.Select(r => new
                {
                    id = r.IdReporte,
                    nombre = r.Nombre,
                    fecha = r.FechaGeneracion,
                    ruta = r.RutaArchivo,
                    descripcion = r.Descripcion
                }).ToList();

                _logger.LogInformation("Se consultaron {Count} reportes disponibles", reportesDto.Count);
                
                return Ok(reportesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar reportes disponibles");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Fuerza la generación manual de un reporte Excel (opcional, testing)
        /// </summary>
        [HttpPost("generate")]
        public ActionResult<object> GenerateReport()
        {
            try
            {
                _logger.LogInformation("Iniciando generación manual de reporte Excel");
                
                // Generar reporte manualmente
                _generadorReporte.GenerarReporte();
                
                var response = new
                {
                    mensaje = "Reporte Excel generado correctamente",
                    timestamp = DateTime.Now,
                    tipo = "manual"
                };

                _logger.LogInformation("Reporte generado manualmente con éxito");
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la generación manual del reporte");
                return StatusCode(500, new 
                { 
                    mensaje = "Error al generar el reporte", 
                    error = ex.Message 
                });
            }
        }
    }
} 