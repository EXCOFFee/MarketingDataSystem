using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketingDataSystem.Application.Interfaces;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protegido según SRS - datos financieros críticos
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaService;
        private readonly ILogger<VentaController> _logger;

        public VentaController(
            IVentaService ventaService,
            ILogger<VentaController> logger)
        {
            _ventaService = ventaService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las ventas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaDto>>> GetVentas()
        {
            try
            {
                var ventas = await _ventaService.GetAllAsync();
                _logger.LogInformation("Se consultaron {Count} ventas", ventas.Count());
                return Ok(ventas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar ventas");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene una venta por su ID
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VentaDto>> GetVenta(int id)
        {
            try
            {
                var venta = await _ventaService.GetByIdAsync(id);
                if (venta == null)
                {
                    return NotFound(new { message = "Venta no encontrada" });
                }

                _logger.LogInformation("Venta {Id} consultada exitosamente", id);
                return Ok(venta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar venta {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea una nueva venta
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<VentaDto>> CreateVenta([FromBody] VentaDto ventaDto)
        {
            try
            {
                if (ventaDto.PrecioUnitario <= 0)
                {
                    return BadRequest(new { message = "El precio unitario de la venta debe ser mayor a 0" });
                }

                var nuevaVenta = await _ventaService.CreateAsync(ventaDto);
                _logger.LogInformation("Venta creada exitosamente con ID: {Id}, PrecioUnitario: {PrecioUnitario}", nuevaVenta.IdVenta, nuevaVenta.PrecioUnitario);
                
                return CreatedAtAction(nameof(GetVenta), new { id = nuevaVenta.IdVenta }, nuevaVenta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear venta");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza una venta existente
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<VentaDto>> UpdateVenta(int id, [FromBody] VentaDto ventaDto)
        {
            try
            {
                if (id != ventaDto.IdVenta)
                {
                    return BadRequest(new { message = "El ID de la venta no coincide" });
                }

                if (ventaDto.PrecioUnitario <= 0)
                {
                    return BadRequest(new { message = "El precio unitario de la venta debe ser mayor a 0" });
                }

                var ventaActualizada = await _ventaService.UpdateAsync(ventaDto);
                if (ventaActualizada == null)
                {
                    return NotFound(new { message = "Venta no encontrada" });
                }

                _logger.LogInformation("Venta {Id} actualizada exitosamente", id);
                return Ok(ventaActualizada);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar venta {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina una venta (soft delete)
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteVenta(int id)
        {
            try
            {
                var venta = await _ventaService.GetByIdAsync(id);
                if (venta == null)
                {
                    return NotFound(new { message = "Venta no encontrada" });
                }

                await _ventaService.DeleteAsync(id);
                _logger.LogInformation("Venta {Id} eliminada exitosamente", id);
                
                return Ok(new { message = "Venta eliminada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar venta {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene ventas por rango de fechas
        /// </summary>
        [HttpGet("por-fecha")]
        public async Task<ActionResult<IEnumerable<VentaDto>>> GetVentasPorFecha(
            [FromQuery] DateTime fechaInicio, 
            [FromQuery] DateTime fechaFin)
        {
            try
            {
                if (fechaInicio > fechaFin)
                {
                    return BadRequest(new { message = "La fecha de inicio no puede ser mayor a la fecha de fin" });
                }

                var ventas = await _ventaService.GetAllAsync();
                var ventasFiltradas = ventas.Where(v => v.Fecha >= fechaInicio && v.Fecha <= fechaFin);
                
                _logger.LogInformation("Consulta de ventas entre {FechaInicio} y {FechaFin} retornó {Count} resultados", 
                    fechaInicio, fechaFin, ventasFiltradas.Count());
                
                return Ok(ventasFiltradas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar ventas por fecha");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene estadísticas de ventas
        /// </summary>
        [HttpGet("estadisticas")]
        public async Task<ActionResult<object>> GetEstadisticasVentas()
        {
            try
            {
                var ventas = await _ventaService.GetAllAsync();
                var hoy = DateTime.Today;
                var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
                
                var estadisticas = new
                {
                    ventasHoy = ventas.Count(v => v.Fecha.Date == hoy),
                    ventasMes = ventas.Count(v => v.Fecha >= inicioMes),
                    totalHoy = ventas.Where(v => v.Fecha.Date == hoy).Sum(v => v.PrecioUnitario * v.Cantidad),
                    totalMes = ventas.Where(v => v.Fecha >= inicioMes).Sum(v => v.PrecioUnitario * v.Cantidad),
                    promedioVenta = ventas.Any() ? ventas.Average(v => v.PrecioUnitario) : 0,
                    totalVentas = ventas.Count()
                };

                _logger.LogInformation("Estadísticas de ventas consultadas exitosamente");
                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar estadísticas de ventas");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }
} 