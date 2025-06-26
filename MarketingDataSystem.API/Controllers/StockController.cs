using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketingDataSystem.Application.Interfaces;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protegido según SRS - datos de inventario sensibles
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly ILogger<StockController> _logger;

        public StockController(
            IStockService stockService,
            ILogger<StockController> logger)
        {
            _stockService = stockService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todo el stock disponible
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockDto>>> GetStock()
        {
            try
            {
                var stock = await _stockService.GetAllAsync();
                _logger.LogInformation("Se consultaron {Count} registros de stock", stock.Count());
                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar stock");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene stock por ID
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StockDto>> GetStock(int id)
        {
            try
            {
                var stock = await _stockService.GetByIdAsync(id);
                if (stock == null)
                {
                    return NotFound(new { message = "Registro de stock no encontrado" });
                }

                _logger.LogInformation("Stock {Id} consultado exitosamente", id);
                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar stock {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea un nuevo registro de stock
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<StockDto>> CreateStock([FromBody] StockDto stockDto)
        {
            try
            {
                if (stockDto.Cantidad < 0)
                {
                    return BadRequest(new { message = "La cantidad no puede ser negativa" });
                }

                var nuevoStock = await _stockService.CreateAsync(stockDto);
                _logger.LogInformation("Stock creado exitosamente con ID: {Id}", nuevoStock.IdStock);
                
                return CreatedAtAction(nameof(GetStock), new { id = nuevoStock.IdStock }, nuevoStock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear stock");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza stock existente
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<StockDto>> UpdateStock(int id, [FromBody] StockDto stockDto)
        {
            try
            {
                if (id != stockDto.IdStock)
                {
                    return BadRequest(new { message = "El ID del stock no coincide" });
                }

                if (stockDto.Cantidad < 0)
                {
                    return BadRequest(new { message = "La cantidad no puede ser negativa" });
                }

                var stockActualizado = await _stockService.UpdateAsync(stockDto);
                if (stockActualizado == null)
                {
                    return NotFound(new { message = "Stock no encontrado" });
                }

                _logger.LogInformation("Stock {Id} actualizado exitosamente", id);
                return Ok(stockActualizado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar stock {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina registro de stock
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteStock(int id)
        {
            try
            {
                var stock = await _stockService.GetByIdAsync(id);
                if (stock == null)
                {
                    return NotFound(new { message = "Stock no encontrado" });
                }

                await _stockService.DeleteAsync(id);
                _logger.LogInformation("Stock {Id} eliminado exitosamente", id);
                
                return Ok(new { message = "Stock eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar stock {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene productos con stock bajo (menos de stock mínimo)
        /// </summary>
        [HttpGet("stock-bajo")]
        public async Task<ActionResult<IEnumerable<StockDto>>> GetStockBajo([FromQuery] int stockMinimo = 10)
        {
            try
            {
                var stock = await _stockService.GetAllAsync();
                var stockBajo = stock.Where(s => s.Cantidad <= stockMinimo);
                
                _logger.LogInformation("Consulta de stock bajo retornó {Count} productos", stockBajo.Count());
                return Ok(stockBajo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar stock bajo");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Ajusta la cantidad de stock (para movimientos de inventario)
        /// </summary>
        [HttpPost("{id:int}/ajustar")]
        public async Task<ActionResult<StockDto>> AjustarStock(int id, [FromBody] AjusteStockRequest request)
        {
            try
            {
                var stock = await _stockService.GetByIdAsync(id);
                if (stock == null)
                {
                    return NotFound(new { message = "Stock no encontrado" });
                }

                // Aplicar ajuste
                var nuevaCantidad = stock.Cantidad + request.Ajuste;
                if (nuevaCantidad < 0)
                {
                    return BadRequest(new { message = "La cantidad resultante no puede ser negativa" });
                }

                stock.Cantidad = nuevaCantidad;
                var stockActualizado = await _stockService.UpdateAsync(stock);

                _logger.LogInformation("Stock {Id} ajustado en {Ajuste} unidades. Nueva cantidad: {Cantidad}", 
                    id, request.Ajuste, nuevaCantidad);
                
                return Ok(stockActualizado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ajustar stock {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene estadísticas de inventario
        /// </summary>
        [HttpGet("estadisticas")]
        public async Task<ActionResult<object>> GetEstadisticasInventario()
        {
            try
            {
                var stock = await _stockService.GetAllAsync();
                
                var estadisticas = new
                {
                    totalProductos = stock.Count(),
                    totalUnidades = stock.Sum(s => s.Cantidad),
                    productosSinStock = stock.Count(s => s.Cantidad == 0),
                    productosStockBajo = stock.Count(s => s.Cantidad <= 10 && s.Cantidad > 0),
                    cantidadPromedio = stock.Any() ? stock.Average(s => s.Cantidad) : 0
                };

                _logger.LogInformation("Estadísticas de inventario consultadas exitosamente");
                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar estadísticas de inventario");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }

    /// <summary>
    /// Modelo para solicitud de ajuste de stock
    /// </summary>
    public class AjusteStockRequest
    {
        public int Ajuste { get; set; }
        public string? Motivo { get; set; }
    }
} 