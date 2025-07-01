// ==================== CONTROLADOR REST CRÍTICO DE TRANSACCIONES FINANCIERAS ====================
// Este controlador maneja TODO el dinero real del negocio
// CAPA: Presentation (Web API) - Expone endpoints REST para transacciones de ventas
// CRITICIDAD: MÁXIMA - Cada endpoint maneja dinero real, facturación y revenue
// SEGURIDAD: JWT Authentication + Authorization obligatoria para todos los endpoints
// CLEAN ARCHITECTURE: Capa externa que llama a VentaService (Application Layer)
// COMPLIANCE: Preparado para auditorías financieras y trazabilidad completa

using Microsoft.AspNetCore.Authorization;  // Autorización JWT obligatoria
using Microsoft.AspNetCore.Mvc;           // Framework MVC para controladores REST
using MarketingDataSystem.Application.Interfaces;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.API.Controllers
{
    /// <summary>
    /// Controlador REST crítico para gestión de transacciones de ventas financieras
    /// RESPONSABILIDAD: Exponer API REST segura para todas las operaciones de ventas
    /// ARQUITECTURA: Capa Presentation en Clean Architecture - punto de entrada HTTP
    /// CRITICIDAD: MÁXIMA - Maneja dinero real, facturación y revenue empresarial
    /// ENDPOINTS EXPUESTOS:
    /// - GET /api/venta - Lista todas las ventas (con paginación implícita)
    /// - GET /api/venta/{id} - Obtiene venta específica por ID
    /// - POST /api/venta - Crea nueva venta (transacción financiera)
    /// - PUT /api/venta/{id} - Actualiza venta existente (con validaciones)
    /// - DELETE /api/venta/{id} - Elimina venta (soft delete para auditoría)
    /// - GET /api/venta/por-fecha - Filtra ventas por rango de fechas
    /// - GET /api/venta/estadisticas - Métricas financieras en tiempo real
    /// SEGURIDAD IMPLEMENTADA:
    /// - [Authorize] obligatorio en todos los endpoints (JWT required)
    /// - Validación de datos financieros (precios, cantidades)
    /// - Logging detallado para auditoría financiera
    /// - Manejo de errores sin revelar información sensible
    /// VALIDACIONES CRÍTICAS:
    /// - Precios siempre > 0 (no ventas gratis accidentales)
    /// - IDs válidos y consistentes
    /// - Rangos de fechas lógicos
    /// - Integridad referencial con productos y clientes
    /// CASOS DE USO EMPRESARIALES:
    /// - Sistema POS para registrar ventas en tiempo real
    /// - Dashboard ejecutivo con métricas de revenue
    /// - Integración con sistemas de facturación
    /// - Reportes financieros y análisis de tendencias
    /// - Auditorías y compliance financiero
    /// CLEAN ARCHITECTURE: Llama únicamente a VentaService (Application Layer)
    /// HTTP STATUS CODES: Implementa códigos estándar REST (200, 201, 400, 404, 500)
    /// LOGGING: Structured logging para monitoreo y debugging empresarial
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // CRÍTICO: Protegido según SRS - datos financieros requieren autenticación
    public class VentaController : ControllerBase
    {
        // ========== DEPENDENCIAS PARA OPERACIONES FINANCIERAS ==========
        private readonly IVentaService _ventaService;        // Servicio de lógica de negocio
        private readonly ILogger<VentaController> _logger;   // Logging para auditoría financiera

        /// <summary>
        /// Constructor con inyección de dependencias para operaciones financieras críticas
        /// PATRÓN: Dependency Injection - servicios inyectados como interfaces testeable
        /// CLEAN ARCHITECTURE: Controller solo conoce Application Layer (VentaService)
        /// LOGGING: Logger especializado para auditoría de transacciones financieras
        /// SOLID: Principio D - depende de abstracciones, no implementaciones concretas
        /// </summary>
        /// <param name="ventaService">Servicio de aplicación para lógica de negocio de ventas</param>
        /// <param name="logger">Logger para auditoría detallada de transacciones financieras</param>
        public VentaController(
            IVentaService ventaService,
            ILogger<VentaController> logger)
        {
            _ventaService = ventaService;  // Servicio con lógica de negocio
            _logger = logger;              // Auditoría de transacciones críticas
        }

        /// <summary>
        /// Obtiene todas las ventas del sistema con auditoría completa
        /// ENDPOINT: GET /api/venta
        /// AUTENTICACIÓN: JWT requerido (datos financieros sensibles)
        /// CASO DE USO: Dashboard ejecutivo, reportes financieros, análisis de tendencias
        /// PERFORMANCE: Retorna todas las ventas (considerar paginación para volúmenes altos)
        /// AUDITORÍA: Registra cada consulta para compliance y monitoreo
        /// RESPUESTA: Lista completa de VentaDto con información financiera
        /// ERROR HANDLING: 500 con mensaje genérico (no revelar información interna)
        /// </summary>
        /// <returns>200 OK con lista de ventas, 500 si error interno</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaDto>>> GetVentas()
        {
            try
            {
                // ========== CONSULTA DE VENTAS VÍA SERVICIO ==========
                var ventas = await _ventaService.GetAllAsync();
                
                // ========== AUDITORÍA DE CONSULTA FINANCIERA ==========
                _logger.LogInformation("Se consultaron {Count} ventas", ventas.Count());
                
                // ========== RESPUESTA HTTP 200 OK ==========
                return Ok(ventas); // Lista de VentaDto con datos financieros
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING CON LOGGING DETALLADO ==========
                _logger.LogError(ex, "Error al consultar ventas");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene una venta específica por su ID único
        /// ENDPOINT: GET /api/venta/{id}
        /// AUTENTICACIÓN: JWT requerido para acceso a transacción específica
        /// CASO DE USO: Detalle de venta, edición, facturación, auditoría específica
        /// VALIDACIÓN: ID debe ser entero positivo válido
        /// RESPUESTA: VentaDto con detalles completos de la transacción
        /// ERROR HANDLING: 404 si no existe, 500 si error interno
        /// </summary>
        /// <param name="id">ID único de la venta a consultar</param>
        /// <returns>200 OK con VentaDto, 404 si no existe, 500 si error</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VentaDto>> GetVenta(int id)
        {
            try
            {
                // ========== BÚSQUEDA DE VENTA POR ID ==========
                var venta = await _ventaService.GetByIdAsync(id);
                
                // ========== VALIDACIÓN DE EXISTENCIA ==========
                if (venta == null)
                {
                    return NotFound(new { message = "Venta no encontrada" });
                }

                // ========== AUDITORÍA DE CONSULTA ESPECÍFICA ==========
                _logger.LogInformation("Venta {Id} consultada exitosamente", id);
                
                // ========== RESPUESTA HTTP 200 OK ==========
                return Ok(venta); // VentaDto con detalles completos
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING CON CONTEXTO ==========
                _logger.LogError(ex, "Error al consultar venta {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea una nueva venta - OPERACIÓN FINANCIERA CRÍTICA
        /// ENDPOINT: POST /api/venta
        /// AUTENTICACIÓN: JWT requerido (creación de transacción financiera)
        /// CASO DE USO: POS, e-commerce, venta telefónica, integración con ERP
        /// VALIDACIONES CRÍTICAS:
        /// - Precio unitario > 0 (prevenir ventas gratis accidentales)
        /// - Cantidad válida y consistente
        /// - Referencias a cliente y producto válidas
        /// TRANSACCIÓN: Operación ACID en VentaService con rollback automático
        /// IMPACTO: Afecta revenue, stock, métricas, reportes financieros
        /// RESPUESTA: 201 Created con Location header y VentaDto creado
        /// </summary>
        /// <param name="ventaDto">DTO con datos de la nueva venta a crear</param>
        /// <returns>201 Created con venta creada, 400 si datos inválidos, 500 si error</returns>
        [HttpPost]
        public async Task<ActionResult<VentaDto>> CreateVenta([FromBody] VentaDto ventaDto)
        {
            try
            {
                // ========== VALIDACIÓN CRÍTICA DE PRECIO ==========
                if (ventaDto.PrecioUnitario <= 0)
                {
                    return BadRequest(new { message = "El precio unitario de la venta debe ser mayor a 0" });
                }

                // ========== CREACIÓN VÍA SERVICIO (TRANSACCIÓN ACID) ==========
                var nuevaVenta = await _ventaService.CreateAsync(ventaDto);
                
                // ========== AUDITORÍA DE TRANSACCIÓN FINANCIERA CRÍTICA ==========
                _logger.LogInformation("Venta creada exitosamente con ID: {Id}, PrecioUnitario: {PrecioUnitario}", 
                    nuevaVenta.IdVenta, nuevaVenta.PrecioUnitario);
                
                // ========== RESPUESTA HTTP 201 CREATED CON LOCATION ==========
                return CreatedAtAction(nameof(GetVenta), new { id = nuevaVenta.IdVenta }, nuevaVenta);
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING PARA TRANSACCIONES CRÍTICAS ==========
                _logger.LogError(ex, "Error al crear venta");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza una venta existente - MODIFICACIÓN FINANCIERA SENSIBLE
        /// ENDPOINT: PUT /api/venta/{id}
        /// AUTENTICACIÓN: JWT requerido (modificación de datos financieros)
        /// CASO DE USO: Corrección de errores, ajustes de precio, modificación de cantidad
        /// VALIDACIONES CRÍTICAS:
        /// - ID en URL debe coincidir con ID en body (consistencia)
        /// - Precio unitario > 0 (validación financiera)
        /// - Venta debe existir antes de actualizar
        /// AUDITORÍA: Registro detallado para compliance y trazabilidad
        /// IMPACTO: Afecta métricas financieras, reportes, análisis histórico
        /// </summary>
        /// <param name="id">ID de la venta a actualizar</param>
        /// <param name="ventaDto">DTO con datos actualizados de la venta</param>
        /// <returns>200 OK con venta actualizada, 400/404 si inválido, 500 si error</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<VentaDto>> UpdateVenta(int id, [FromBody] VentaDto ventaDto)
        {
            try
            {
                // ========== VALIDACIÓN DE CONSISTENCIA DE IDs ==========
                if (id != ventaDto.IdVenta)
                {
                    return BadRequest(new { message = "El ID de la venta no coincide" });
                }

                // ========== VALIDACIÓN FINANCIERA CRÍTICA ==========
                if (ventaDto.PrecioUnitario <= 0)
                {
                    return BadRequest(new { message = "El precio unitario de la venta debe ser mayor a 0" });
                }

                // ========== ACTUALIZACIÓN VÍA SERVICIO ==========
                var ventaActualizada = await _ventaService.UpdateAsync(ventaDto);
                
                // ========== VALIDACIÓN DE EXISTENCIA ==========
                if (ventaActualizada == null)
                {
                    return NotFound(new { message = "Venta no encontrada" });
                }

                // ========== AUDITORÍA DE MODIFICACIÓN FINANCIERA ==========
                _logger.LogInformation("Venta {Id} actualizada exitosamente", id);
                
                // ========== RESPUESTA HTTP 200 OK ==========
                return Ok(ventaActualizada); // VentaDto actualizado
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING CON CONTEXTO ESPECÍFICO ==========
                _logger.LogError(ex, "Error al actualizar venta {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina una venta - OPERACIÓN SENSIBLE PARA AUDITORÍA
        /// ENDPOINT: DELETE /api/venta/{id}
        /// AUTENTICACIÓN: JWT requerido (eliminación de registro financiero)
        /// IMPLEMENTACIÓN: Soft delete para mantener trazabilidad (no eliminación física)
        /// CASO DE USO: Corrección de errores, cancelaciones, ajustes contables
        /// AUDITORÍA: Registro crítico para compliance y rastreabilidad
        /// VALIDACIÓN: Verificar existencia antes de intentar eliminación
        /// CONSIDERACIÓN: Evaluar impacto en reportes e integridad referencial
        /// </summary>
        /// <param name="id">ID único de la venta a eliminar</param>
        /// <returns>200 OK si eliminado, 404 si no existe, 500 si error</returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteVenta(int id)
        {
            try
            {
                // ========== VERIFICACIÓN DE EXISTENCIA PREVIA ==========
                var venta = await _ventaService.GetByIdAsync(id);
                if (venta == null)
                {
                    return NotFound(new { message = "Venta no encontrada" });
                }

                // ========== ELIMINACIÓN SOFT DELETE VÍA SERVICIO ==========
                await _ventaService.DeleteAsync(id);
                
                // ========== AUDITORÍA CRÍTICA DE ELIMINACIÓN ==========
                _logger.LogInformation("Venta {Id} eliminada exitosamente", id);
                
                // ========== RESPUESTA HTTP 200 OK ==========
                return Ok(new { message = "Venta eliminada exitosamente" });
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING PARA ELIMINACIÓN ==========
                _logger.LogError(ex, "Error al eliminar venta {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene ventas filtradas por rango de fechas - ANÁLISIS TEMPORAL
        /// ENDPOINT: GET /api/venta/por-fecha?fechaInicio=2024-01-01&fechaFin=2024-12-31
        /// AUTENTICACIÓN: JWT requerido (acceso a datos financieros históricos)
        /// CASO DE USO: Reportes por período, análisis de tendencias, cierre mensual/anual
        /// VALIDACIONES:
        /// - fechaInicio <= fechaFin (lógica temporal)
        /// - Fechas en formato válido
        /// OPTIMIZACIÓN: Filtrado en memoria (considerar filtrado en BD para volúmenes altos)
        /// RESPUESTA: Lista filtrada de ventas en el rango especificado
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio del rango (inclusive)</param>
        /// <param name="fechaFin">Fecha de fin del rango (inclusive)</param>
        /// <returns>200 OK con ventas filtradas, 400 si fechas inválidas, 500 si error</returns>
        [HttpGet("por-fecha")]
        public async Task<ActionResult<IEnumerable<VentaDto>>> GetVentasPorFecha(
            [FromQuery] DateTime fechaInicio, 
            [FromQuery] DateTime fechaFin)
        {
            try
            {
                // ========== VALIDACIÓN DE LÓGICA TEMPORAL ==========
                if (fechaInicio > fechaFin)
                {
                    return BadRequest(new { message = "La fecha de inicio no puede ser mayor a la fecha de fin" });
                }

                // ========== CONSULTA Y FILTRADO ==========
                var ventas = await _ventaService.GetAllAsync();
                var ventasFiltradas = ventas.Where(v => v.Fecha >= fechaInicio && v.Fecha <= fechaFin);
                
                // ========== AUDITORÍA DE CONSULTA TEMPORAL ==========
                _logger.LogInformation("Consulta de ventas entre {FechaInicio} y {FechaFin} retornó {Count} resultados", 
                    fechaInicio, fechaFin, ventasFiltradas.Count());
                
                // ========== RESPUESTA HTTP 200 OK ==========
                return Ok(ventasFiltradas); // Lista filtrada por fechas
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING PARA CONSULTAS TEMPORALES ==========
                _logger.LogError(ex, "Error al consultar ventas por fecha");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene estadísticas financieras en tiempo real - DASHBOARD EJECUTIVO
        /// ENDPOINT: GET /api/venta/estadisticas
        /// AUTENTICACIÓN: JWT requerido (acceso a métricas financieras sensibles)
        /// CASO DE USO: Dashboard CEO, métricas KPI, monitoreo de performance diario
        /// MÉTRICAS CALCULADAS:
        /// - Ventas del día actual (conteo y valor)
        /// - Ventas del mes actual (conteo y valor)
        /// - Promedio de venta (precio promedio)
        /// - Total histórico de ventas
        /// PERFORMANCE: Cálculo en memoria (considerar caché para alta concurrencia)
        /// TIEMPO REAL: Datos actualizados con cada transacción
        /// </summary>
        /// <returns>200 OK con objeto de estadísticas financieras, 500 si error</returns>
        [HttpGet("estadisticas")]
        public async Task<ActionResult<object>> GetEstadisticasVentas()
        {
            try
            {
                // ========== CONSULTA DE DATOS BASE ==========
                var ventas = await _ventaService.GetAllAsync();
                var hoy = DateTime.Today;
                var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
                
                // ========== CÁLCULO DE MÉTRICAS FINANCIERAS ==========
                var estadisticas = new
                {
                    ventasHoy = ventas.Count(v => v.Fecha.Date == hoy),
                    ventasMes = ventas.Count(v => v.Fecha >= inicioMes),
                    totalHoy = ventas.Where(v => v.Fecha.Date == hoy).Sum(v => v.PrecioUnitario * v.Cantidad),
                    totalMes = ventas.Where(v => v.Fecha >= inicioMes).Sum(v => v.PrecioUnitario * v.Cantidad),
                    promedioVenta = ventas.Any() ? ventas.Average(v => v.PrecioUnitario) : 0,
                    totalVentas = ventas.Count()
                };

                // ========== AUDITORÍA DE CONSULTA DE MÉTRICAS ==========
                _logger.LogInformation("Estadísticas de ventas consultadas exitosamente");
                
                // ========== RESPUESTA HTTP 200 OK CON MÉTRICAS ==========
                return Ok(estadisticas); // Objeto con KPIs financieros
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING PARA MÉTRICAS CRÍTICAS ==========
                _logger.LogError(ex, "Error al consultar estadísticas de ventas");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        // ========== ENDPOINTS FUTUROS PARA API EMPRESARIAL AVANZADA ==========
        // TODO: Implementar endpoints adicionales para funcionalidad empresarial:
        
        /// <summary>
        /// Obtiene ventas paginadas para optimizar performance
        /// GET /api/venta/paginadas?page=1&pageSize=20
        /// </summary>
        // [HttpGet("paginadas")]
        // public async Task<ActionResult<PagedResult<VentaDto>>> GetVentasPaginadas(
        //     [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        
        /// <summary>
        /// Obtiene ventas por cliente específico
        /// GET /api/venta/por-cliente/{clienteId}
        /// </summary>
        // [HttpGet("por-cliente/{clienteId:int}")]
        // public async Task<ActionResult<IEnumerable<VentaDto>>> GetVentasPorCliente(int clienteId)
        
        /// <summary>
        /// Obtiene ventas por producto específico
        /// GET /api/venta/por-producto/{productoId}
        /// </summary>
        // [HttpGet("por-producto/{productoId}")]
        // public async Task<ActionResult<IEnumerable<VentaDto>>> GetVentasPorProducto(string productoId)
        
        /// <summary>
        /// Bulk creation para múltiples ventas simultáneas
        /// POST /api/venta/bulk
        /// </summary>
        // [HttpPost("bulk")]
        // public async Task<ActionResult<BulkResult<VentaDto>>> CreateVentasBulk([FromBody] IEnumerable<VentaDto> ventas)
        
        /// <summary>
        /// Exporta ventas a Excel para análisis offline
        /// GET /api/venta/export/excel
        /// </summary>
        // [HttpGet("export/excel")]
        // public async Task<ActionResult> ExportVentasToExcel([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)

        // ========== CONSIDERACIONES PARA ESCALABILIDAD EMPRESARIAL ==========
        // FUTURO: Para APIs de nivel empresarial considerar:
        // - Paginación obligatoria para conjuntos grandes de datos
        // - Filtros avanzados (cliente, producto, rango de montos)
        // - Rate limiting para prevenir abuso de API
        // - Versionado de API (/api/v1/venta)
        // - Cache de estadísticas para mejorar performance
        // - Webhooks para notificaciones de ventas en tiempo real
        // - GraphQL para consultas flexibles
        // - Compresión GZIP para respuestas grandes
        // - Documentación OpenAPI/Swagger automática
        // - Monitoreo de performance con métricas detalladas
    }
} 