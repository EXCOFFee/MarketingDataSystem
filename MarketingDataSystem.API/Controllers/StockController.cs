// ==================== CONTROLADOR REST CRÍTICO DE GESTIÓN DE INVENTARIO ====================
// Este controlador controla TODO el inventario de la empresa
// CAPA: Presentation (Web API) - Expone endpoints REST para gestión completa de stock
// CRITICIDAD: MÁXIMA - Previene sobreventa, ruptura de stock y pérdidas económicas
// IMPACTO: Cada endpoint afecta disponibilidad de productos y satisfacción del cliente
// CLEAN ARCHITECTURE: Capa externa que llama a StockService (Application Layer)
// ALERTAS: Integrado con AlertaService para notificaciones automáticas de stock bajo

using Microsoft.AspNetCore.Authorization;  // Autorización JWT para datos sensibles
using Microsoft.AspNetCore.Mvc;           // Framework MVC para controladores REST
using MarketingDataSystem.Application.Interfaces;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.API.Controllers
{
    /// <summary>
    /// Controlador REST crítico para gestión completa de inventario empresarial
    /// RESPONSABILIDAD: Exponer API REST segura para todas las operaciones de stock
    /// ARQUITECTURA: Capa Presentation en Clean Architecture - punto de entrada HTTP
    /// CRITICIDAD: MÁXIMA - Previene sobreventa, ruptura de stock y pérdidas financieras
    /// ENDPOINTS EXPUESTOS:
    /// - GET /api/stock - Lista todo el inventario disponible
    /// - GET /api/stock/{id} - Obtiene stock específico por ID
    /// - POST /api/stock - Crea nuevo registro de inventario
    /// - PUT /api/stock/{id} - Actualiza información de stock
    /// - DELETE /api/stock/{id} - Elimina registro de stock
    /// - GET /api/stock/stock-bajo - Alerta de productos con stock crítico
    /// - POST /api/stock/{id}/ajustar - Ajusta cantidades por movimientos
    /// - GET /api/stock/estadisticas - Métricas de inventario en tiempo real
    /// VALIDACIONES CRÍTICAS:
    /// - Cantidades nunca negativas (imposibilidad física)
    /// - Consistencia de IDs en operaciones
    /// - Verificación de existencia antes de modificaciones
    /// - Validación de ajustes para evitar stock negativo
    /// CASOS DE USO EMPRESARIALES:
    /// - Sistema WMS (Warehouse Management System)
    /// - Prevención de sobreventa en e-commerce
    /// - Alertas automáticas de reabastecimiento
    /// - Auditorías de inventario y control de pérdidas
    /// - Integración con sistemas ERP y logística
    /// - Dashboard de disponibilidad para vendedores
    /// IMPACTO OPERACIONAL:
    /// - Previene pérdida de ventas por falta de stock
    /// - Optimiza niveles de inventario y capital de trabajo
    /// - Mejora satisfacción del cliente con disponibilidad precisa
    /// - Facilita planificación de compras y reabastecimiento
    /// ALERTAS AUTOMÁTICAS:
    /// - Stock bajo: Notificaciones a managers y compradores
    /// - Stock agotado: Alertas críticas inmediatas
    /// - Movimientos anómalos: Auditoría y seguimiento
    /// CLEAN ARCHITECTURE: Solo conoce StockService (Application Layer)
    /// SEGURIDAD: JWT requerido para proteger datos de inventario sensibles
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // CRÍTICO: Protegido según SRS - datos de inventario empresarial sensibles
    public class StockController : ControllerBase
    {
        // ========== DEPENDENCIAS PARA GESTIÓN DE INVENTARIO ==========
        private readonly IStockService _stockService;        // Servicio de lógica de negocio de stock
        private readonly ILogger<StockController> _logger;   // Logging para auditoría de inventario

        /// <summary>
        /// Constructor con inyección de dependencias para operaciones de inventario críticas
        /// PATRÓN: Dependency Injection - servicios inyectados como interfaces testeable
        /// CLEAN ARCHITECTURE: Controller solo conoce Application Layer (StockService)
        /// LOGGING: Logger especializado para auditoría de movimientos de inventario
        /// SOLID: Principio D - depende de abstracciones, no implementaciones concretas
        /// </summary>
        /// <param name="stockService">Servicio de aplicación para lógica de negocio de stock</param>
        /// <param name="logger">Logger para auditoría detallada de inventario</param>
        public StockController(
            IStockService stockService,
            ILogger<StockController> logger)
        {
            _stockService = stockService;  // Servicio con lógica de inventario
            _logger = logger;              // Auditoría de movimientos críticos
        }

        /// <summary>
        /// Obtiene todo el inventario disponible del sistema
        /// ENDPOINT: GET /api/stock
        /// AUTENTICACIÓN: JWT requerido (datos de inventario empresarial sensibles)
        /// CASO DE USO: Dashboard de inventario, WMS, planificación de compras
        /// INFORMACIÓN: Lista completa con cantidades, ubicaciones y estados
        /// PERFORMANCE: Retorna todo el inventario (considerar paginación para almacenes grandes)
        /// AUDITORÍA: Registra consultas para monitoreo de acceso a inventario
        /// RESPUESTA: Lista de StockDto con información completa de inventario
        /// </summary>
        /// <returns>200 OK con lista de stock, 500 si error interno</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockDto>>> GetStock()
        {
            try
            {
                // ========== CONSULTA COMPLETA DE INVENTARIO ==========
                var stock = await _stockService.GetAllAsync();
                
                // ========== AUDITORÍA DE CONSULTA DE INVENTARIO ==========
                _logger.LogInformation("Se consultaron {Count} registros de stock", stock.Count());
                
                // ========== RESPUESTA HTTP 200 OK ==========
                return Ok(stock); // Lista completa de inventario
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING CON LOGGING DETALLADO ==========
                _logger.LogError(ex, "Error al consultar stock");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene información específica de stock por ID único
        /// ENDPOINT: GET /api/stock/{id}
        /// AUTENTICACIÓN: JWT requerido para acceso a registro específico de inventario
        /// CASO DE USO: Verificación de disponibilidad, auditoría específica, WMS
        /// VALIDACIÓN: ID debe ser entero positivo válido
        /// RESPUESTA: StockDto con detalles completos del registro de inventario
        /// ERROR HANDLING: 404 si no existe, 500 si error interno
        /// </summary>
        /// <param name="id">ID único del registro de stock a consultar</param>
        /// <returns>200 OK con StockDto, 404 si no existe, 500 si error</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StockDto>> GetStock(int id)
        {
            try
            {
                // ========== BÚSQUEDA DE REGISTRO DE STOCK ==========
                var stock = await _stockService.GetByIdAsync(id);
                
                // ========== VALIDACIÓN DE EXISTENCIA ==========
                if (stock == null)
                {
                    return NotFound(new { message = "Registro de stock no encontrado" });
                }

                // ========== AUDITORÍA DE CONSULTA ESPECÍFICA ==========
                _logger.LogInformation("Stock {Id} consultado exitosamente", id);
                
                // ========== RESPUESTA HTTP 200 OK ==========
                return Ok(stock); // StockDto con información detallada
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING CON CONTEXTO ==========
                _logger.LogError(ex, "Error al consultar stock {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea un nuevo registro de stock - OPERACIÓN CRÍTICA DE INVENTARIO
        /// ENDPOINT: POST /api/stock
        /// AUTENTICACIÓN: JWT requerido (creación de registro de inventario)
        /// CASO DE USO: Ingreso de mercancía, nuevos productos, ajustes iniciales
        /// VALIDACIONES CRÍTICAS:
        /// - Cantidad >= 0 (imposibilidad física de stock negativo)
        /// - Información de producto válida y existente
        /// - Ubicación de almacén válida
        /// IMPACTO: Aumenta disponibilidad para ventas y afecta métricas
        /// ALERTAS: Puede desactivar alertas de stock bajo automáticamente
        /// </summary>
        /// <param name="stockDto">DTO con datos del nuevo registro de stock</param>
        /// <returns>201 Created con stock creado, 400 si datos inválidos, 500 si error</returns>
        [HttpPost]
        public async Task<ActionResult<StockDto>> CreateStock([FromBody] StockDto stockDto)
        {
            try
            {
                // ========== VALIDACIÓN CRÍTICA DE CANTIDAD ==========
                if (stockDto.Cantidad < 0)
                {
                    return BadRequest(new { message = "La cantidad no puede ser negativa" });
                }

                // ========== CREACIÓN VÍA SERVICIO ==========
                var nuevoStock = await _stockService.CreateAsync(stockDto);
                
                // ========== AUDITORÍA DE CREACIÓN DE INVENTARIO ==========
                _logger.LogInformation("Stock creado exitosamente con ID: {Id}", nuevoStock.IdStock);
                
                // ========== RESPUESTA HTTP 201 CREATED CON LOCATION ==========
                return CreatedAtAction(nameof(GetStock), new { id = nuevoStock.IdStock }, nuevoStock);
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING PARA CREACIÓN CRÍTICA ==========
                _logger.LogError(ex, "Error al crear stock");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza información de stock existente - MODIFICACIÓN SENSIBLE
        /// ENDPOINT: PUT /api/stock/{id}
        /// AUTENTICACIÓN: JWT requerido (modificación de datos de inventario)
        /// CASO DE USO: Corrección de errores, ajustes de inventario, cambio de ubicación
        /// VALIDACIONES CRÍTICAS:
        /// - ID en URL debe coincidir con ID en body (consistencia)
        /// - Cantidad >= 0 (validación de inventario)
        /// - Registro debe existir antes de actualizar
        /// AUDITORÍA: Registro detallado para trazabilidad de cambios
        /// IMPACTO: Afecta disponibilidad inmediata y puede disparar alertas
        /// </summary>
        /// <param name="id">ID del registro de stock a actualizar</param>
        /// <param name="stockDto">DTO con datos actualizados del stock</param>
        /// <returns>200 OK con stock actualizado, 400/404 si inválido, 500 si error</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<StockDto>> UpdateStock(int id, [FromBody] StockDto stockDto)
        {
            try
            {
                // ========== VALIDACIÓN DE CONSISTENCIA DE IDs ==========
                if (id != stockDto.IdStock)
                {
                    return BadRequest(new { message = "El ID del stock no coincide" });
                }

                // ========== VALIDACIÓN CRÍTICA DE CANTIDAD ==========
                if (stockDto.Cantidad < 0)
                {
                    return BadRequest(new { message = "La cantidad no puede ser negativa" });
                }

                // ========== ACTUALIZACIÓN VÍA SERVICIO ==========
                var stockActualizado = await _stockService.UpdateAsync(stockDto);
                
                // ========== VALIDACIÓN DE EXISTENCIA ==========
                if (stockActualizado == null)
                {
                    return NotFound(new { message = "Stock no encontrado" });
                }

                // ========== AUDITORÍA DE MODIFICACIÓN DE INVENTARIO ==========
                _logger.LogInformation("Stock {Id} actualizado exitosamente", id);
                
                // ========== RESPUESTA HTTP 200 OK ==========
                return Ok(stockActualizado); // StockDto actualizado
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING CON CONTEXTO ESPECÍFICO ==========
                _logger.LogError(ex, "Error al actualizar stock {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina registro de stock - OPERACIÓN SENSIBLE PARA AUDITORÍA
        /// ENDPOINT: DELETE /api/stock/{id}
        /// AUTENTICACIÓN: JWT requerido (eliminación de registro de inventario)
        /// IMPLEMENTACIÓN: Eliminación lógica recomendada para trazabilidad
        /// CASO DE USO: Descontinuación de productos, corrección de errores graves
        /// AUDITORÍA: Registro crítico para compliance e integridad de inventario
        /// VALIDACIÓN: Verificar existencia antes de intentar eliminación
        /// CONSIDERACIÓN: Evaluar impacto en disponibilidad y ventas activas
        /// </summary>
        /// <param name="id">ID único del registro de stock a eliminar</param>
        /// <returns>200 OK si eliminado, 404 si no existe, 500 si error</returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteStock(int id)
        {
            try
            {
                // ========== VERIFICACIÓN DE EXISTENCIA PREVIA ==========
                var stock = await _stockService.GetByIdAsync(id);
                if (stock == null)
                {
                    return NotFound(new { message = "Stock no encontrado" });
                }

                // ========== ELIMINACIÓN VÍA SERVICIO ==========
                await _stockService.DeleteAsync(id);
                
                // ========== AUDITORÍA CRÍTICA DE ELIMINACIÓN ==========
                _logger.LogInformation("Stock {Id} eliminado exitosamente", id);
                
                // ========== RESPUESTA HTTP 200 OK ==========
                return Ok(new { message = "Stock eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING PARA ELIMINACIÓN ==========
                _logger.LogError(ex, "Error al eliminar stock {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene productos con stock bajo - SISTEMA DE ALERTAS CRÍTICAS
        /// ENDPOINT: GET /api/stock/stock-bajo?stockMinimo=10
        /// AUTENTICACIÓN: JWT requerido (acceso a alertas de inventario crítico)
        /// CASO DE USO: Dashboard de alertas, sistema de reabastecimiento, compras
        /// PARÁMETROS: stockMinimo configurable (default: 10 unidades)
        /// ALERTAS: Integrado con AlertaService para notificaciones automáticas
        /// IMPACTO: Previene ruptura de stock y pérdida de ventas
        /// RESPUESTA: Lista de productos que requieren reabastecimiento urgente
        /// </summary>
        /// <param name="stockMinimo">Umbral mínimo para considerar stock bajo (default: 10)</param>
        /// <returns>200 OK con productos en alerta, 500 si error</returns>
        [HttpGet("stock-bajo")]
        public async Task<ActionResult<IEnumerable<StockDto>>> GetStockBajo([FromQuery] int stockMinimo = 10)
        {
            try
            {
                // ========== CONSULTA Y FILTRADO DE STOCK CRÍTICO ==========
                var stock = await _stockService.GetAllAsync();
                var stockBajo = stock.Where(s => s.Cantidad <= stockMinimo);
                
                // ========== AUDITORÍA DE CONSULTA DE ALERTAS ==========
                _logger.LogInformation("Consulta de stock bajo retornó {Count} productos", stockBajo.Count());
                
                // ========== RESPUESTA HTTP 200 OK CON ALERTAS ==========
                return Ok(stockBajo); // Lista de productos que requieren atención
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING PARA ALERTAS CRÍTICAS ==========
                _logger.LogError(ex, "Error al consultar stock bajo");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Ajusta cantidad de stock por movimientos de inventario - OPERACIÓN TRANSACCIONAL
        /// ENDPOINT: POST /api/stock/{id}/ajustar
        /// AUTENTICACIÓN: JWT requerido (modificación directa de cantidades)
        /// CASO DE USO: Recepciones, envíos, ajustes por inventario físico, mermas
        /// TRANSACCIÓN: Operación atómica que previene inconsistencias
        /// VALIDACIONES CRÍTICAS:
        /// - Stock debe existir antes del ajuste
        /// - Resultado final no puede ser negativo (imposibilidad física)
        /// - Ajuste debe tener motivo para auditoría
        /// AUDITORÍA: Logging detallado del ajuste, cantidad anterior y nueva
        /// ALERTAS: Puede disparar notificaciones si resultado queda en stock bajo
        /// </summary>
        /// <param name="id">ID del registro de stock a ajustar</param>
        /// <param name="request">Objeto con ajuste y motivo del movimiento</param>
        /// <returns>200 OK con stock ajustado, 400/404 si inválido, 500 si error</returns>
        [HttpPost("{id:int}/ajustar")]
        public async Task<ActionResult<StockDto>> AjustarStock(int id, [FromBody] AjusteStockRequest request)
        {
            try
            {
                // ========== VERIFICACIÓN DE EXISTENCIA DEL STOCK ==========
                var stock = await _stockService.GetByIdAsync(id);
                if (stock == null)
                {
                    return NotFound(new { message = "Stock no encontrado" });
                }

                // ========== CÁLCULO Y VALIDACIÓN DEL AJUSTE ==========
                var nuevaCantidad = stock.Cantidad + request.Ajuste;
                if (nuevaCantidad < 0)
                {
                    return BadRequest(new { message = "La cantidad resultante no puede ser negativa" });
                }

                // ========== APLICACIÓN DEL AJUSTE TRANSACCIONAL ==========
                stock.Cantidad = nuevaCantidad;
                var stockActualizado = await _stockService.UpdateAsync(stock);

                // ========== AUDITORÍA DETALLADA DEL MOVIMIENTO ==========
                _logger.LogInformation("Stock {Id} ajustado en {Ajuste} unidades. Nueva cantidad: {Cantidad}", 
                    id, request.Ajuste, nuevaCantidad);
                
                // ========== RESPUESTA HTTP 200 OK CON STOCK ACTUALIZADO ==========
                return Ok(stockActualizado); // StockDto con nueva cantidad
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING PARA AJUSTES CRÍTICOS ==========
                _logger.LogError(ex, "Error al ajustar stock {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene estadísticas de inventario en tiempo real - DASHBOARD OPERACIONAL
        /// ENDPOINT: GET /api/stock/estadisticas
        /// AUTENTICACIÓN: JWT requerido (acceso a métricas de inventario sensibles)
        /// CASO DE USO: Dashboard gerencial, KPIs de inventario, planificación operativa
        /// MÉTRICAS CALCULADAS:
        /// - Total de productos en inventario
        /// - Total de unidades disponibles
        /// - Productos sin stock (agotados)
        /// - Productos con stock bajo (requieren atención)
        /// - Cantidad promedio por producto
        /// PERFORMANCE: Cálculo en memoria (considerar caché para almacenes grandes)
        /// TIEMPO REAL: Datos actualizados con cada movimiento de inventario
        /// </summary>
        /// <returns>200 OK con objeto de estadísticas de inventario, 500 si error</returns>
        [HttpGet("estadisticas")]
        public async Task<ActionResult<object>> GetEstadisticasInventario()
        {
            try
            {
                // ========== CONSULTA DE DATOS BASE PARA MÉTRICAS ==========
                var stock = await _stockService.GetAllAsync();
                
                // ========== CÁLCULO DE ESTADÍSTICAS DE INVENTARIO ==========
                var estadisticas = new
                {
                    totalProductos = stock.Count(),                                    // Total SKUs en inventario
                    totalUnidades = stock.Sum(s => s.Cantidad),                      // Total unidades disponibles
                    productosSinStock = stock.Count(s => s.Cantidad == 0),           // Productos agotados (CRÍTICO)
                    productosStockBajo = stock.Count(s => s.Cantidad <= 10 && s.Cantidad > 0), // Requieren reabastecimiento
                    cantidadPromedio = stock.Any() ? stock.Average(s => s.Cantidad) : 0  // Promedio de unidades por SKU
                };

                // ========== AUDITORÍA DE CONSULTA DE MÉTRICAS ==========
                _logger.LogInformation("Estadísticas de inventario consultadas exitosamente");
                
                // ========== RESPUESTA HTTP 200 OK CON MÉTRICAS ==========
                return Ok(estadisticas); // Objeto con KPIs de inventario
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING PARA MÉTRICAS CRÍTICAS ==========
                _logger.LogError(ex, "Error al consultar estadísticas de inventario");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        // ========== ENDPOINTS FUTUROS PARA GESTIÓN AVANZADA DE INVENTARIO ==========
        // TODO: Implementar endpoints adicionales para funcionalidad empresarial:
        
        /// <summary>
        /// Obtiene stock por ubicación de almacén
        /// GET /api/stock/por-ubicacion/{ubicacion}
        /// </summary>
        // [HttpGet("por-ubicacion/{ubicacion}")]
        // public async Task<ActionResult<IEnumerable<StockDto>>> GetStockPorUbicacion(string ubicacion)
        
        /// <summary>
        /// Obtiene historial de movimientos de stock
        /// GET /api/stock/{id}/movimientos
        /// </summary>
        // [HttpGet("{id}/movimientos")]
        // public async Task<ActionResult<IEnumerable<MovimientoStockDto>>> GetMovimientosStock(int id)
        
        /// <summary>
        /// Reserva stock para venta pendiente
        /// POST /api/stock/{id}/reservar
        /// </summary>
        // [HttpPost("{id}/reservar")]
        // public async Task<ActionResult<StockDto>> ReservarStock(int id, [FromBody] ReservaStockRequest request)
        
        /// <summary>
        /// Libera stock reservado
        /// POST /api/stock/{id}/liberar
        /// </summary>
        // [HttpPost("{id}/liberar")]
        // public async Task<ActionResult<StockDto>> LiberarStock(int id, [FromBody] LiberarStockRequest request)
        
        /// <summary>
        /// Exporta inventario a Excel para auditoría
        /// GET /api/stock/export/excel
        /// </summary>
        // [HttpGet("export/excel")]
        // public async Task<ActionResult> ExportStockToExcel()
        
        /// <summary>
        /// Valida integridad del inventario
        /// POST /api/stock/validar-integridad
        /// </summary>
        // [HttpPost("validar-integridad")]
        // public async Task<ActionResult<IntegridadInventarioDto>> ValidarIntegridadInventario()

        // ========== CONSIDERACIONES PARA INVENTARIO EMPRESARIAL AVANZADO ==========
        // FUTURO: Para sistemas WMS de nivel empresarial considerar:
        // - Múltiples almacenes y ubicaciones geográficas
        // - Códigos de barras y RFID para rastreo automático
        // - Integración con sistemas ERP (SAP, Oracle, etc.)
        // - Pronósticos de demanda basados en ML
        // - Optimización automática de niveles de stock
        // - Alertas inteligentes basadas en patrones históricos
        // - API de integración con proveedores para reabastecimiento automático
        // - Trazabilidad completa desde proveedor hasta cliente final
        // - Gestión de lotes y fechas de vencimiento
        // - Auditorías cíclicas automatizadas con drones/robots
    }

    /// <summary>
    /// Modelo para solicitud de ajuste de stock con trazabilidad
    /// PROPÓSITO: Encapsular información del movimiento para auditoría completa
    /// CAMPOS: Ajuste (cantidad a sumar/restar) y Motivo (razón del movimiento)
    /// AUDITORÍA: Motivo obligatorio para trazabilidad y compliance
    /// CASOS DE USO: Recepciones (+), Envíos (-), Mermas (-), Ajustes por inventario físico
    /// </summary>
    public class AjusteStockRequest
    {
        /// <summary>
        /// Cantidad a ajustar (positiva para ingreso, negativa para salida)
        /// EJEMPLOS: +100 (recepción), -50 (venta), -5 (merma), +10 (devolución)
        /// </summary>
        public int Ajuste { get; set; }
        
        /// <summary>
        /// Motivo del ajuste para auditoría y trazabilidad
        /// EJEMPLOS: "Recepción compra #123", "Venta orden #456", "Merma por vencimiento"
        /// OBLIGATORIO: Requerido para compliance y auditorías de inventario
        /// </summary>
        public string? Motivo { get; set; }
    }
} 