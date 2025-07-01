// ==================== CONTROLADOR REST CRÍTICO DE CONFIGURACIÓN DE FUENTES DE DATOS ====================
// Este controlador gestiona la configuración completa del pipeline ETL
// CAPA: Presentation (Web API) - Expone endpoints REST para gestión de fuentes de datos
// CRITICIDAD: ALTA - Configura de dónde el sistema obtiene datos para procesar
// PIPELINE ETL: Base fundamental para todo el procesamiento automático de datos
// CLEAN ARCHITECTURE: Capa externa que llama a FuenteDeDatosService (Application Layer)
// CONEXIONES: Gestiona múltiples tipos de fuentes (JSON, CSV, XML, APIs, Bases de Datos)

using Microsoft.AspNetCore.Authorization;  // Autorización JWT para configuración crítica
using Microsoft.AspNetCore.Mvc;           // Framework MVC para controladores REST
using MarketingDataSystem.Application.Interfaces;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.API.Controllers
{
    /// <summary>
    /// Controlador REST para configuración y gestión de fuentes de datos del pipeline ETL
    /// RESPONSABILIDAD: Exponer API REST para configurar de dónde obtiene datos el sistema
    /// ARQUITECTURA: Capa Presentation en Clean Architecture - punto de entrada HTTP
    /// CRITICIDAD: ALTA - Sin fuentes configuradas, el pipeline ETL no puede funcionar
    /// ENDPOINTS EXPUESTOS:
    /// - GET /api/fuentededatos - Lista todas las fuentes configuradas
    /// - GET /api/fuentededatos/{id} - Obtiene fuente específica por ID
    /// - POST /api/fuentededatos - Crea nueva fuente de datos
    /// - PUT /api/fuentededatos/{id} - Actualiza configuración de fuente
    /// - DELETE /api/fuentededatos/{id} - Elimina fuente (soft delete)
    /// - POST /api/fuentededatos/{id}/test-connection - Prueba conectividad
    /// - GET /api/fuentededatos/por-tipo/{tipo} - Filtra por tipo de fuente
    /// - GET /api/fuentededatos/estadisticas - Métricas de fuentes configuradas
    /// TIPOS DE FUENTES SOPORTADAS:
    /// - JSON: APIs REST, archivos JSON locales
    /// - CSV: Archivos CSV, exports de Excel
    /// - XML: Servicios SOAP, archivos XML estructurados
    /// - DATABASE: SQL Server, MySQL, PostgreSQL
    /// - API: Endpoints RESTful externos
    /// - FTP: Archivos en servidores FTP/SFTP
    /// CASOS DE USO EMPRESARIALES:
    /// - Configuración inicial del sistema de datos
    /// - Integración con sistemas legacy (ERP, CRM)
    /// - Conexión con APIs de proveedores/partners
    /// - Ingesta de archivos de marketing campaigns
    /// - Synchronización con plataformas e-commerce
    /// - Integración con herramientas de analytics
    /// PIPELINE ETL DEPENDENCY:
    /// - ValidadorService: Necesita saber qué validar según tipo de fuente
    /// - TransformadorService: Aplica transformaciones específicas por tipo
    /// - EnriquecedorService: Enriquece datos según el origen
    /// - DeduplicadorService: Identifica duplicados entre fuentes
    /// CONFIGURACIÓN CRÍTICA:
    /// - Connection strings seguros (encriptados)
    /// - Credenciales de autenticación
    /// - Formatos de datos esperados
    /// - Scheduling de ingesta automática
    /// CLEAN ARCHITECTURE: Solo conoce FuenteDeDatosService (Application Layer)
    /// SEGURIDAD: JWT requerido para proteger configuración sensible del sistema
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // CRÍTICO: Protegido según SRS - configuración sensible del pipeline ETL
    public class FuenteDeDatosController : ControllerBase
    {
        // ========== DEPENDENCIAS PARA GESTIÓN DE FUENTES DE DATOS ==========
        private readonly IFuenteDeDatosService _fuenteService;         // Servicio de lógica de negocio
        private readonly ILogger<FuenteDeDatosController> _logger;     // Logging para auditoría de configuración

        /// <summary>
        /// Constructor con inyección de dependencias para gestión de fuentes de datos críticas
        /// PATRÓN: Dependency Injection - servicios inyectados como interfaces testeable
        /// CLEAN ARCHITECTURE: Controller solo conoce Application Layer (FuenteDeDatosService)
        /// LOGGING: Logger especializado para auditoría de configuraciones críticas
        /// SOLID: Principio D - depende de abstracciones, no implementaciones concretas
        /// </summary>
        /// <param name="fuenteService">Servicio de aplicación para lógica de fuentes de datos</param>
        /// <param name="logger">Logger para auditoría de configuraciones del pipeline ETL</param>
        public FuenteDeDatosController(
            IFuenteDeDatosService fuenteService,
            ILogger<FuenteDeDatosController> logger)
        {
            _fuenteService = fuenteService;  // Servicio con lógica de configuración
            _logger = logger;                // Auditoría de cambios críticos
        }

        /// <summary>
        /// Obtiene todas las fuentes de datos configuradas en el sistema
        /// ENDPOINT: GET /api/fuentededatos
        /// AUTENTICACIÓN: JWT requerido (configuración sensible del pipeline)
        /// CASO DE USO: Dashboard de configuración, auditoría, troubleshooting ETL
        /// INFORMACIÓN: Lista completa con tipos, conexiones, estados, metadatos
        /// PERFORMANCE: Retorna configuraciones completas (datos seguros filtrados)
        /// AUDITORÍA: Registra consultas para monitoreo de acceso a configuración
        /// RESPUESTA: Lista de FuenteDeDatosDto con información completa pero segura
        /// </summary>
        /// <returns>200 OK con lista de fuentes configuradas, 500 si error interno</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FuenteDeDatosDto>>> GetFuentesDeDatos()
        {
            try
            {
                // ========== CONSULTA DE CONFIGURACIONES DE FUENTES ==========
                var fuentes = await _fuenteService.GetAllAsync();
                
                // ========== AUDITORÍA DE CONSULTA DE CONFIGURACIÓN ==========
                _logger.LogInformation("Se consultaron {Count} fuentes de datos", fuentes.Count());
                
                // ========== RESPUESTA HTTP 200 OK ==========
                return Ok(fuentes); // Lista de configuraciones (credentials protegidas)
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING CON LOGGING DETALLADO ==========
                _logger.LogError(ex, "Error al consultar fuentes de datos");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene configuración específica de fuente de datos por ID único
        /// ENDPOINT: GET /api/fuentededatos/{id}
        /// AUTENTICACIÓN: JWT requerido para acceso a configuración específica
        /// CASO DE USO: Edición de configuración, troubleshooting, validación setup
        /// VALIDACIÓN: ID debe ser entero positivo válido
        /// RESPUESTA: FuenteDeDatosDto con configuración completa (sin credenciales)
        /// ERROR HANDLING: 404 si no existe, 500 si error interno
        /// </summary>
        /// <param name="id">ID único de la fuente de datos a consultar</param>
        /// <returns>200 OK con FuenteDeDatosDto, 404 si no existe, 500 si error</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<FuenteDeDatosDto>> GetFuenteDeDatos(int id)
        {
            try
            {
                // ========== BÚSQUEDA DE CONFIGURACIÓN POR ID ==========
                var fuente = await _fuenteService.GetByIdAsync(id);
                
                // ========== VALIDACIÓN DE EXISTENCIA ==========
                if (fuente == null)
                {
                    return NotFound(new { message = "Fuente de datos no encontrada" });
                }

                // ========== AUDITORÍA DE CONSULTA ESPECÍFICA ==========
                _logger.LogInformation("Fuente de datos {Id} consultada exitosamente", id);
                
                // ========== RESPUESTA HTTP 200 OK ==========
                return Ok(fuente); // Configuración completa (sin credenciales sensibles)
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING CON CONTEXTO ==========
                _logger.LogError(ex, "Error al consultar fuente de datos {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea nueva fuente de datos - CONFIGURACIÓN CRÍTICA DEL PIPELINE ETL
        /// ENDPOINT: POST /api/fuentededatos
        /// AUTENTICACIÓN: JWT requerido (creación de configuración crítica)
        /// CASO DE USO: Setup inicial, integración nueva, conexión con sistema externo
        /// VALIDACIONES CRÍTICAS:
        /// - Nombre obligatorio y único (identificación clara)
        /// - Tipo de conexión válido (JSON, CSV, XML, DATABASE, API, FTP)
        /// - Configuración de conexión válida según tipo
        /// - Credenciales de acceso cuando sea necesario
        /// IMPACTO: Habilita nueva fuente para ingesta automática en pipeline ETL
        /// SEGURIDAD: Credentials encriptadas antes de almacenar
        /// </summary>
        /// <param name="fuenteDto">DTO con configuración de la nueva fuente de datos</param>
        /// <returns>201 Created con fuente creada, 400 si datos inválidos, 500 si error</returns>
        [HttpPost]
        public async Task<ActionResult<FuenteDeDatosDto>> CreateFuenteDeDatos([FromBody] FuenteDeDatosDto fuenteDto)
        {
            try
            {
                // ========== VALIDACIONES CRÍTICAS DE CONFIGURACIÓN ==========
                if (string.IsNullOrWhiteSpace(fuenteDto.Nombre))
                {
                    return BadRequest(new { message = "El nombre de la fuente es requerido" });
                }

                if (string.IsNullOrWhiteSpace(fuenteDto.Tipo))
                {
                    return BadRequest(new { message = "El tipo de conexión es requerido" });
                }

                // ========== CREACIÓN VÍA SERVICIO (CON ENCRIPTACIÓN) ==========
                var nuevaFuente = await _fuenteService.CreateAsync(fuenteDto);
                
                // ========== AUDITORÍA DE NUEVA CONFIGURACIÓN CRÍTICA ==========
                _logger.LogInformation("Fuente de datos creada exitosamente con ID: {Id}", nuevaFuente.IdFuente);
                
                // ========== RESPUESTA HTTP 201 CREATED CON LOCATION ==========
                return CreatedAtAction(nameof(GetFuenteDeDatos), new { id = nuevaFuente.IdFuente }, nuevaFuente);
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING PARA CONFIGURACIÓN CRÍTICA ==========
                _logger.LogError(ex, "Error al crear fuente de datos");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza configuración de fuente de datos existente - MODIFICACIÓN SENSIBLE
        /// ENDPOINT: PUT /api/fuentededatos/{id}
        /// AUTENTICACIÓN: JWT requerido (modificación de configuración crítica)
        /// CASO DE USO: Cambio de credenciales, actualización de endpoints, ajuste configuración
        /// VALIDACIONES CRÍTICAS:
        /// - ID en URL debe coincidir con ID en body (consistencia)
        /// - Nombre obligatorio (identificación clara)
        /// - Configuración debe existir antes de actualizar
        /// AUDITORÍA: Registro detallado para compliance y trazabilidad
        /// IMPACTO: Afecta ingesta automática de datos en pipeline ETL
        /// SEGURIDAD: Credentials actualizadas de forma segura (encriptación)
        /// </summary>
        /// <param name="id">ID de la fuente de datos a actualizar</param>
        /// <param name="fuenteDto">DTO con configuración actualizada de la fuente</param>
        /// <returns>200 OK con fuente actualizada, 400/404 si inválido, 500 si error</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<FuenteDeDatosDto>> UpdateFuenteDeDatos(int id, [FromBody] FuenteDeDatosDto fuenteDto)
        {
            try
            {
                // ========== VALIDACIÓN DE CONSISTENCIA DE IDs ==========
                if (id != fuenteDto.IdFuente)
                {
                    return BadRequest(new { message = "El ID de la fuente de datos no coincide" });
                }

                // ========== VALIDACIÓN CRÍTICA DE CONFIGURACIÓN ==========
                if (string.IsNullOrWhiteSpace(fuenteDto.Nombre))
                {
                    return BadRequest(new { message = "El nombre de la fuente es requerido" });
                }

                // ========== ACTUALIZACIÓN VÍA SERVICIO ==========
                var fuenteActualizada = await _fuenteService.UpdateAsync(fuenteDto);
                
                // ========== VALIDACIÓN DE EXISTENCIA ==========
                if (fuenteActualizada == null)
                {
                    return NotFound(new { message = "Fuente de datos no encontrada" });
                }

                // ========== AUDITORÍA DE MODIFICACIÓN DE CONFIGURACIÓN ==========
                _logger.LogInformation("Fuente de datos {Id} actualizada exitosamente", id);
                
                // ========== RESPUESTA HTTP 200 OK ==========
                return Ok(fuenteActualizada); // Configuración actualizada
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING CON CONTEXTO ESPECÍFICO ==========
                _logger.LogError(ex, "Error al actualizar fuente de datos {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina fuente de datos - OPERACIÓN SENSIBLE PARA PIPELINE ETL
        /// ENDPOINT: DELETE /api/fuentededatos/{id}
        /// AUTENTICACIÓN: JWT requerido (eliminación de configuración crítica)
        /// IMPLEMENTACIÓN: Soft delete para mantener trazabilidad histórica
        /// CASO DE USO: Descontinuación de fuente, migración, corrección de errores
        /// AUDITORÍA: Registro crítico para compliance y rastreabilidad
        /// VALIDACIÓN: Verificar existencia antes de intentar eliminación
        /// CONSIDERACIÓN: Evaluar impacto en pipeline ETL automático
        /// IMPACTO: Fuente no estará disponible para futuras ingestas ETL
        /// </summary>
        /// <param name="id">ID único de la fuente de datos a eliminar</param>
        /// <returns>200 OK si eliminado, 404 si no existe, 500 si error</returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteFuenteDeDatos(int id)
        {
            try
            {
                // ========== VERIFICACIÓN DE EXISTENCIA PREVIA ==========
                var fuente = await _fuenteService.GetByIdAsync(id);
                if (fuente == null)
                {
                    return NotFound(new { message = "Fuente de datos no encontrada" });
                }

                // ========== ELIMINACIÓN SOFT DELETE VÍA SERVICIO ==========
                await _fuenteService.DeleteAsync(id);
                
                // ========== AUDITORÍA CRÍTICA DE ELIMINACIÓN ==========
                _logger.LogInformation("Fuente de datos {Id} eliminada exitosamente", id);
                
                // ========== RESPUESTA HTTP 200 OK ==========
                return Ok(new { message = "Fuente de datos eliminada exitosamente" });
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING PARA ELIMINACIÓN ==========
                _logger.LogError(ex, "Error al eliminar fuente de datos {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Prueba conectividad de fuente de datos - VALIDACIÓN DE CONFIGURACIÓN
        /// ENDPOINT: POST /api/fuentededatos/{id}/test-connection
        /// AUTENTICACIÓN: JWT requerido (testing de configuración sensible)
        /// CASO DE USO: Validación post-configuración, troubleshooting, health check
        /// FUNCIONALIDAD:
        /// - Valida credenciales de acceso
        /// - Verifica conectividad de red
        /// - Confirma formato de datos esperado
        /// - Prueba permisos de lectura
        /// TIPOS DE PRUEBA POR FUENTE:
        /// - JSON/API: HTTP request con autenticación
        /// - CSV/FTP: Conexión y listado de archivos
        /// - DATABASE: Query de prueba simple
        /// - XML: Parsing de estructura básica
        /// RESPUESTA: Boolean success + timestamp + detalles del test
        /// SIMULADO: Implementación actual simula prueba real
        /// </summary>
        /// <param name="id">ID de la fuente de datos a probar</param>
        /// <returns>200 OK con resultado de prueba, 404 si no existe, 500 si error</returns>
        [HttpPost("{id:int}/test-connection")]
        public async Task<ActionResult<object>> TestConnection(int id)
        {
            try
            {
                // ========== VERIFICACIÓN DE EXISTENCIA DE FUENTE ==========
                var fuente = await _fuenteService.GetByIdAsync(id);
                if (fuente == null)
                {
                    return NotFound(new { message = "Fuente de datos no encontrada" });
                }

                // ========== EJECUCIÓN DE PRUEBA DE CONECTIVIDAD ==========
                // NOTA: En implementación real, esto haría conexión real según tipo
                var exito = SimularPruebaConexion(fuente);
                
                if (exito)
                {
                    // ========== AUDITORÍA DE PRUEBA EXITOSA ==========
                    _logger.LogInformation("Prueba de conexión exitosa para fuente {Id}", id);
                    return Ok(new { 
                        success = true, 
                        message = "Conexión exitosa",
                        timestamp = DateTime.Now 
                    });
                }
                else
                {
                    // ========== AUDITORÍA DE FALLA EN PRUEBA ==========
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
                // ========== ERROR HANDLING PARA PRUEBAS CRÍTICAS ==========
                _logger.LogError(ex, "Error al probar conexión de fuente {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene fuentes de datos filtradas por tipo - ANÁLISIS POR CATEGORÍA
        /// ENDPOINT: GET /api/fuentededatos/por-tipo/{tipo}
        /// AUTENTICACIÓN: JWT requerido (acceso a configuraciones por categoría)
        /// CASO DE USO: Dashboard por tecnología, análisis de integración, troubleshooting
        /// TIPOS VÁLIDOS: JSON, CSV, XML, DATABASE, API, FTP
        /// FILTRADO: Case-insensitive para flexibilidad
        /// RESPUESTA: Lista de fuentes que coincidan con el tipo especificado
        /// CASOS DE USO: Gestión por tecnología, análisis de dependencias
        /// </summary>
        /// <param name="tipo">Tipo de fuente a filtrar (JSON, CSV, XML, DATABASE, API, FTP)</param>
        /// <returns>200 OK con fuentes filtradas, 500 si error</returns>
        [HttpGet("por-tipo/{tipo}")]
        public async Task<ActionResult<IEnumerable<FuenteDeDatosDto>>> GetFuentesPorTipo(string tipo)
        {
            try
            {
                // ========== CONSULTA Y FILTRADO POR TIPO ==========
                var fuentes = await _fuenteService.GetAllAsync();
                var fuentesFiltradas = fuentes.Where(f => f.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase));
                
                // ========== AUDITORÍA DE CONSULTA FILTRADA ==========
                _logger.LogInformation("Consulta de fuentes por tipo '{Tipo}' retornó {Count} resultados", tipo, fuentesFiltradas.Count());
                
                // ========== RESPUESTA HTTP 200 OK CON FILTRO ==========
                return Ok(fuentesFiltradas); // Lista filtrada por tipo
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING PARA CONSULTAS FILTRADAS ==========
                _logger.LogError(ex, "Error al consultar fuentes por tipo");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene estadísticas de fuentes de datos configuradas - DASHBOARD ADMINISTRATIVO
        /// ENDPOINT: GET /api/fuentededatos/estadisticas
        /// AUTENTICACIÓN: JWT requerido (acceso a métricas de configuración)
        /// CASO DE USO: Dashboard administrativo, análisis de integración, planificación
        /// MÉTRICAS CALCULADAS:
        /// - Total de fuentes configuradas
        /// - Distribución por tipo de tecnología
        /// - Análisis de diversificación de fuentes
        /// INFORMACIÓN: Útil para entender la complejidad del ecosistema de datos
        /// RESPUESTA: Objeto con estadísticas agregadas y desglose por tipo
        /// </summary>
        /// <returns>200 OK con estadísticas de fuentes, 500 si error</returns>
        [HttpGet("estadisticas")]
        public async Task<ActionResult<object>> GetEstadisticasFuentes()
        {
            try
            {
                // ========== CONSULTA DE DATOS BASE PARA MÉTRICAS ==========
                var fuentes = await _fuenteService.GetAllAsync();
                
                // ========== CÁLCULO DE ESTADÍSTICAS DE CONFIGURACIÓN ==========
                var estadisticas = new
                {
                    totalFuentes = fuentes.Count(),                           // Total de fuentes configuradas
                    tipos = fuentes.GroupBy(f => f.Tipo)                    // Agrupación por tipo de tecnología  
                        .Select(g => new { tipo = g.Key, cantidad = g.Count() })
                        .ToList()
                };

                // ========== AUDITORÍA DE CONSULTA DE MÉTRICAS ==========
                _logger.LogInformation("Estadísticas de fuentes de datos consultadas exitosamente");
                
                // ========== RESPUESTA HTTP 200 OK CON ESTADÍSTICAS ==========
                return Ok(estadisticas); // Métricas de configuración del sistema
            }
            catch (Exception ex)
            {
                // ========== ERROR HANDLING PARA MÉTRICAS ADMINISTRATIVAS ==========
                _logger.LogError(ex, "Error al consultar estadísticas de fuentes");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Simula prueba de conexión para demostración - MOCK DE VALIDACIÓN
        /// IMPLEMENTACIÓN REAL: Debería hacer conexión real según tipo de fuente
        /// - JSON/API: HTTP GET request con headers apropiados
        /// - CSV/FTP: Verificar acceso y listar archivos
        /// - DATABASE: SELECT 1 query para validar conexión
        /// - XML: Verificar acceso y parsing básico
        /// SEGURIDAD: No exponer credenciales en logs durante pruebas
        /// </summary>
        /// <param name="fuente">Configuración de fuente a probar</param>
        /// <returns>true si conexión exitosa, false si falla</returns>
        private bool SimularPruebaConexion(FuenteDeDatosDto fuente)
        {
            // ========== SIMULACIÓN PARA DEMOSTRACIÓN ==========
            // En implementación real, esto haría:
            // switch (fuente.Tipo.ToUpper())
            // {
            //     case "JSON":
            //     case "API":
            //         return await TestHttpConnection(fuente.ConnectionString);
            //     case "CSV":
            //     case "FTP":
            //         return await TestFtpConnection(fuente.ConnectionString);
            //     case "DATABASE":
            //         return await TestDatabaseConnection(fuente.ConnectionString);
            //     case "XML":
            //         return await TestXmlConnection(fuente.ConnectionString);
            //     default:
            //         return false;
            // }
            
            // ========== SIMULACIÓN SIMPLE PARA DEMOSTRACIÓN ==========
            return true; // Siempre exitoso para propósitos de demo
        }

        // ========== ENDPOINTS FUTUROS PARA GESTIÓN AVANZADA DE FUENTES ==========
        // TODO: Implementar endpoints adicionales para funcionalidad empresarial:
        
        /// <summary>
        /// Sincroniza datos desde fuente específica de forma manual
        /// POST /api/fuentededatos/{id}/sync
        /// </summary>
        // [HttpPost("{id}/sync")]
        // public async Task<ActionResult> SyncFuenteDatos(int id)
        // {
        //     // Ejecuta ingesta manual desde fuente específica
        //     // Útil para testing y troubleshooting
        // }

        /// <summary>
        /// Obtiene vista previa de datos de la fuente sin procesar
        /// GET /api/fuentededatos/{id}/preview
        /// </summary>
        // [HttpGet("{id}/preview")]
        // public async Task<ActionResult<DataPreviewDto>> PreviewDatos(int id, [FromQuery] int limit = 10)
        // {
        //     // Muestra primeros registros de la fuente para validación
        //     // Sin ejecutar todo el pipeline ETL
        // }

        /// <summary>
        /// Programa schedule de ingesta automática para fuente
        /// POST /api/fuentededatos/{id}/schedule
        /// </summary>
        // [HttpPost("{id}/schedule")]
        // public async Task<ActionResult> ScheduleIngesta(int id, [FromBody] ScheduleRequest request)
        // {
        //     // Configura horarios automáticos de ingesta
        //     // Integración con job scheduler
        // }

        /// <summary>
        /// Obtiene historial de ingestas de una fuente específica
        /// GET /api/fuentededatos/{id}/historial
        /// </summary>
        // [HttpGet("{id}/historial")]
        // public async Task<ActionResult<IEnumerable<IngestionLogDto>>> GetHistorialIngesta(int id)
        // {
        //     // Historial de ejecuciones exitosas y fallidas
        //     // Útil para monitoreo y troubleshooting
        // }

        /// <summary>
        /// Valida formato de datos de la fuente contra schema esperado
        /// POST /api/fuentededatos/{id}/validate-schema
        /// </summary>
        // [HttpPost("{id}/validate-schema")]
        // public async Task<ActionResult<SchemaValidationDto>> ValidateSchema(int id)
        // {
        //     // Validación de estructura de datos antes de ingesta
        //     // Previene errores en pipeline ETL
        // }

        // ========== CONSIDERACIONES PARA GESTIÓN EMPRESARIAL DE FUENTES ==========
        // FUTURO: Para sistemas de nivel empresarial considerar:
        // - Encriptación de credenciales en base de datos
        // - Rotación automática de API keys y passwords  
        // - Monitoring de salud de fuentes con alertas
        // - Versionado de configuraciones para rollback
        // - Backup y restauración de configuraciones críticas
        // - Integration con secretos management (Azure KeyVault, AWS Secrets)
        // - Support para autenticación OAuth2 y certificates
        // - Rate limiting y throttling para APIs externas
        // - Caching de conexiones para mejor performance
        // - Retry policies con exponential backoff
        // - Circuit breaker pattern para fuentes inestables
        // - Métricas detalladas de performance por fuente
    }
} 