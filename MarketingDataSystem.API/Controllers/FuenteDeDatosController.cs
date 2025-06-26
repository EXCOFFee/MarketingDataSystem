using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketingDataSystem.Application.Interfaces;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protegido según SRS - configuración crítica del sistema
    public class FuenteDeDatosController : ControllerBase
    {
        private readonly IFuenteDeDatosService _fuenteService;
        private readonly ILogger<FuenteDeDatosController> _logger;

        public FuenteDeDatosController(
            IFuenteDeDatosService fuenteService,
            ILogger<FuenteDeDatosController> logger)
        {
            _fuenteService = fuenteService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las fuentes de datos configuradas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FuenteDeDatosDto>>> GetFuentesDeDatos()
        {
            try
            {
                var fuentes = await _fuenteService.GetAllAsync();
                _logger.LogInformation("Se consultaron {Count} fuentes de datos", fuentes.Count());
                return Ok(fuentes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar fuentes de datos");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene una fuente de datos por su ID
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<FuenteDeDatosDto>> GetFuenteDeDatos(int id)
        {
            try
            {
                var fuente = await _fuenteService.GetByIdAsync(id);
                if (fuente == null)
                {
                    return NotFound(new { message = "Fuente de datos no encontrada" });
                }

                _logger.LogInformation("Fuente de datos {Id} consultada exitosamente", id);
                return Ok(fuente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar fuente de datos {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea una nueva fuente de datos
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<FuenteDeDatosDto>> CreateFuenteDeDatos([FromBody] FuenteDeDatosDto fuenteDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fuenteDto.Nombre))
                {
                    return BadRequest(new { message = "El nombre de la fuente es requerido" });
                }

                if (string.IsNullOrWhiteSpace(fuenteDto.Tipo))
                {
                    return BadRequest(new { message = "El tipo de conexión es requerido" });
                }

                var nuevaFuente = await _fuenteService.CreateAsync(fuenteDto);
                _logger.LogInformation("Fuente de datos creada exitosamente con ID: {Id}", nuevaFuente.IdFuente);
                
                return CreatedAtAction(nameof(GetFuenteDeDatos), new { id = nuevaFuente.IdFuente }, nuevaFuente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear fuente de datos");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza una fuente de datos existente
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<FuenteDeDatosDto>> UpdateFuenteDeDatos(int id, [FromBody] FuenteDeDatosDto fuenteDto)
        {
            try
            {
                if (id != fuenteDto.IdFuente)
                {
                    return BadRequest(new { message = "El ID de la fuente de datos no coincide" });
                }

                if (string.IsNullOrWhiteSpace(fuenteDto.Nombre))
                {
                    return BadRequest(new { message = "El nombre de la fuente es requerido" });
                }

                var fuenteActualizada = await _fuenteService.UpdateAsync(fuenteDto);
                if (fuenteActualizada == null)
                {
                    return NotFound(new { message = "Fuente de datos no encontrada" });
                }

                _logger.LogInformation("Fuente de datos {Id} actualizada exitosamente", id);
                return Ok(fuenteActualizada);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar fuente de datos {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina una fuente de datos (soft delete)
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteFuenteDeDatos(int id)
        {
            try
            {
                var fuente = await _fuenteService.GetByIdAsync(id);
                if (fuente == null)
                {
                    return NotFound(new { message = "Fuente de datos no encontrada" });
                }

                await _fuenteService.DeleteAsync(id);
                _logger.LogInformation("Fuente de datos {Id} eliminada exitosamente", id);
                
                return Ok(new { message = "Fuente de datos eliminada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar fuente de datos {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Prueba la conexión a una fuente de datos
        /// </summary>
        [HttpPost("{id:int}/test-connection")]
        public async Task<ActionResult<object>> TestConnection(int id)
        {
            try
            {
                var fuente = await _fuenteService.GetByIdAsync(id);
                if (fuente == null)
                {
                    return NotFound(new { message = "Fuente de datos no encontrada" });
                }

                // Simulación de prueba de conexión
                var exito = SimularPruebaConexion(fuente);
                
                if (exito)
                {
                    _logger.LogInformation("Prueba de conexión exitosa para fuente {Id}", id);
                    return Ok(new { 
                        success = true, 
                        message = "Conexión exitosa",
                        timestamp = DateTime.Now 
                    });
                }
                else
                {
                    _logger.LogWarning("Prueba de conexión falló para fuente {Id}", id);
                    return Ok(new { 
                        success = false, 
                        message = "Error de conexión",
                        timestamp = DateTime.Now 
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al probar conexión de fuente {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene fuentes de datos por tipo
        /// </summary>
        [HttpGet("por-tipo/{tipo}")]
        public async Task<ActionResult<IEnumerable<FuenteDeDatosDto>>> GetFuentesPorTipo(string tipo)
        {
            try
            {
                var fuentes = await _fuenteService.GetAllAsync();
                var fuentesFiltradas = fuentes.Where(f => f.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase));
                
                _logger.LogInformation("Consulta de fuentes por tipo '{Tipo}' retornó {Count} resultados", tipo, fuentesFiltradas.Count());
                return Ok(fuentesFiltradas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar fuentes por tipo");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene estadísticas de fuentes de datos
        /// </summary>
        [HttpGet("estadisticas")]
        public async Task<ActionResult<object>> GetEstadisticasFuentes()
        {
            try
            {
                var fuentes = await _fuenteService.GetAllAsync();
                
                var estadisticas = new
                {
                    totalFuentes = fuentes.Count(),
                    tipos = fuentes.GroupBy(f => f.Tipo)
                        .Select(g => new { tipo = g.Key, cantidad = g.Count() })
                        .ToList()
                };

                _logger.LogInformation("Estadísticas de fuentes de datos consultadas exitosamente");
                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar estadísticas de fuentes");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Simula una prueba de conexión (en un escenario real, esto haría una conexión real)
        /// </summary>
        private bool SimularPruebaConexion(FuenteDeDatosDto fuente)
        {
            // Simulación simple: siempre retorna true para demostración
            return true;
        }
    }
} 