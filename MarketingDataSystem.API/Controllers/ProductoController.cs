// ==================== CONTROLADOR DE GESTIÓN DE PRODUCTOS ====================
// Este controlador maneja el catálogo completo de productos del sistema de marketing
// Incluye: CRUD completo + búsqueda por nombre para funcionalidad e-commerce
// PROTEGIDO: Requiere autenticación JWT - datos comerciales sensibles (precios, costos, proveedores)
// PATRÓN: Sigue misma arquitectura que ClienteController para consistencia

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketingDataSystem.Application.Interfaces;
using MarketingDataSystem.Core.DTOs;
using FluentValidation;

namespace MarketingDataSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]   // Ruta base: api/producto
    [Authorize] // CRÍTICO: Protege información comercial sensible (precios, costos, proveedores)
    public class ProductoController : ControllerBase
    {
        // ========== DEPENDENCIAS INYECTADAS ==========
        private readonly IProductoService _productoService;        // Lógica de negocio de productos
        private readonly IValidator<ProductoDto> _validator;       // Validaciones automáticas con FluentValidation
        private readonly ILogger<ProductoController> _logger;      // Logging para auditoria comercial

        /// <summary>
        /// Constructor con inyección de dependencias para gestión de productos
        /// PATRÓN: Dependency Injection siguiendo principios SOLID
        /// CONSISTENCIA: Mismo patrón que otros controladores del sistema
        /// </summary>
        /// <param name="productoService">Servicio con lógica de negocio de productos</param>
        /// <param name="validator">Validador automático para ProductoDto</param>
        /// <param name="logger">Logger para auditoria de operaciones comerciales</param>
        public ProductoController(
            IProductoService productoService,
            IValidator<ProductoDto> validator,
            ILogger<ProductoController> logger)
        {
            _productoService = productoService;
            _validator = validator;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene el catálogo completo de productos activos del sistema
        /// GET api/producto
        /// REQUIERE: Token JWT válido (información comercial protegida)
        /// USO: Catálogo para ventas, inventario, reportes comerciales
        /// DEVUELVE: Lista completa con precios, costos, proveedores, stock
        /// </summary>
        /// <returns>Array de ProductoDto con información comercial completa</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos()
        {
            try
            {
                // Obtener catálogo completo desde la capa de servicio
                var productos = await _productoService.GetAllAsync();
                
                // Log para auditoria comercial - importante para control de acceso a precios
                _logger.LogInformation("Se consultaron {Count} productos", productos.Count());
                
                return Ok(productos); // HTTP 200 con catálogo completo
            }
            catch (Exception ex)
            {
                // Log del error para monitoreo del catálogo
                _logger.LogError(ex, "Error al consultar productos");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductoDto>> GetProducto(int id)
        {
            try
            {
                var producto = await _productoService.GetByIdAsync(id);
                if (producto == null)
                {
                    return NotFound(new { message = "Producto no encontrado" });
                }

                _logger.LogInformation("Producto {Id} consultado exitosamente", id);
                return Ok(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar producto {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProductoDto>> CreateProducto([FromBody] ProductoDto productoDto)
        {
            try
            {
                // Validación con FluentValidation
                var validationResult = await _validator.ValidateAsync(productoDto);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
                    return BadRequest(new { message = "Datos inválidos", errors });
                }

                var nuevoProducto = await _productoService.CreateAsync(productoDto);
                _logger.LogInformation("Producto creado exitosamente con ID: {Id}", nuevoProducto.IdProducto);
                
                return CreatedAtAction(nameof(GetProducto), new { id = nuevoProducto.IdProducto }, nuevoProducto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductoDto>> UpdateProducto(int id, [FromBody] ProductoDto productoDto)
        {
            try
            {
                if (id != productoDto.IdProducto)
                {
                    return BadRequest(new { message = "El ID del producto no coincide" });
                }

                // Validación con FluentValidation
                var validationResult = await _validator.ValidateAsync(productoDto);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
                    return BadRequest(new { message = "Datos inválidos", errors });
                }

                var productoActualizado = await _productoService.UpdateAsync(productoDto);
                if (productoActualizado == null)
                {
                    return NotFound(new { message = "Producto no encontrado" });
                }

                _logger.LogInformation("Producto {Id} actualizado exitosamente", id);
                return Ok(productoActualizado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar producto {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina un producto (soft delete)
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProducto(int id)
        {
            try
            {
                var producto = await _productoService.GetByIdAsync(id);
                if (producto == null)
                {
                    return NotFound(new { message = "Producto no encontrado" });
                }

                await _productoService.DeleteAsync(id);
                _logger.LogInformation("Producto {Id} eliminado exitosamente", id);
                
                return Ok(new { message = "Producto eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar producto {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Busca productos por nombre - funcionalidad adicional para e-commerce
        /// GET api/producto/search?nombre=laptop
        /// REQUIERE: Token JWT válido + parámetro de búsqueda
        /// FUNCIONALIDAD: Búsqueda case-insensitive por nombre de producto
        /// USO: Catálogos web, apps móviles, sistemas de ventas con búsqueda
        /// OPTIMIZACIÓN: En producción se debería implementar con índices de texto completo
        /// </summary>
        /// <param name="nombre">Término de búsqueda para filtrar productos por nombre</param>
        /// <returns>Array de ProductoDto que contienen el término buscado</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> SearchProductos([FromQuery] string nombre)
        {
            try
            {
                // ========== VALIDACIÓN DE PARÁMETROS ==========
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    return BadRequest(new { message = "El nombre de búsqueda es requerido" }); // HTTP 400
                }

                // ========== BÚSQUEDA EN CATÁLOGO ==========
                // NOTA: En producción esto debería usar índices de base de datos para performance
                var productos = await _productoService.GetAllAsync();
                var filtrados = productos.Where(p => p.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase));
                
                // Log de búsquedas para analytics y optimización de catálogo
                _logger.LogInformation("Búsqueda de productos por nombre '{Nombre}' retornó {Count} resultados", nombre, filtrados.Count());
                
                return Ok(filtrados); // HTTP 200 con productos encontrados
            }
            catch (Exception ex)
            {
                // Log de errores en búsqueda para debugging
                _logger.LogError(ex, "Error al buscar productos por nombre");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }
} 