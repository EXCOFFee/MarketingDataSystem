// ==================== CONTROLADOR DE GESTIÓN DE CLIENTES ====================
// Este controlador maneja toda la gestión de clientes del sistema de marketing
// Incluye: CRUD completo (Create, Read, Update, Delete) con validaciones robustas
// PROTEGIDO: Requiere autenticación JWT debido a datos PII (Información Personal Identificable)

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketingDataSystem.Application.Interfaces;
using MarketingDataSystem.Core.DTOs;
using FluentValidation;

namespace MarketingDataSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]   // Ruta base: api/cliente
    [Authorize] // CRÍTICO: Protege datos PII según SRS - solo usuarios autenticados
    public class ClienteController : ControllerBase
    {
        // Dependencias inyectadas para separación de responsabilidades
        private readonly IClienteService _clienteService;           // Lógica de negocio de clientes
        private readonly IValidator<ClienteDto> _validator;         // Validaciones automáticas con FluentValidation
        private readonly ILogger<ClienteController> _logger;        // Logging estructurado para auditoria

        /// <summary>
        /// Constructor con inyección de dependencias múltiples
        /// Sigue principio de Inversión de Dependencias (SOLID)
        /// </summary>
        /// <param name="clienteService">Servicio con lógica de negocio de clientes</param>
        /// <param name="validator">Validador automático de ClienteDto</param>
        /// <param name="logger">Logger para auditoria y debugging</param>
        public ClienteController(
            IClienteService clienteService,
            IValidator<ClienteDto> validator,
            ILogger<ClienteController> logger)
        {
            _clienteService = clienteService;
            _validator = validator;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los clientes activos del sistema
        /// GET api/cliente
        /// REQUIERE: Token JWT válido (datos PII protegidos)
        /// DEVUELVE: Lista completa de clientes con sus datos personales
        /// </summary>
        /// <returns>Array de ClienteDto con todos los clientes activos</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetClientes()
        {
            try
            {
                // Obtener todos los clientes activos desde la capa de servicio
                var clientes = await _clienteService.GetAllAsync();
                
                // Log para auditoria - importante para datos sensibles
                _logger.LogInformation("Se consultaron {Count} clientes", clientes.Count());
                
                return Ok(clientes); // HTTP 200 con lista de clientes
            }
            catch (Exception ex)
            {
                // Log del error para debugging y monitoreo
                _logger.LogError(ex, "Error al consultar clientes");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene un cliente específico por su ID único
        /// GET api/cliente/{id}
        /// REQUIERE: Token JWT válido
        /// EJEMPLO: GET api/cliente/123
        /// </summary>
        /// <param name="id">ID único del cliente a consultar</param>
        /// <returns>ClienteDto con todos los datos del cliente o HTTP 404 si no existe</returns>
        [HttpGet("{id:int}")]  // {id:int} garantiza que solo acepta números enteros
        public async Task<ActionResult<ClienteDto>> GetCliente(int id)
        {
            try
            {
                // Buscar cliente por ID en la base de datos
                var cliente = await _clienteService.GetByIdAsync(id);
                if (cliente == null)
                {
                    // HTTP 404 si el cliente no existe
                    return NotFound(new { message = "Cliente no encontrado" });
                }

                // Log de auditoria para accesos a datos específicos
                _logger.LogInformation("Cliente {Id} consultado exitosamente", id);
                return Ok(cliente); // HTTP 200 con datos del cliente
            }
            catch (Exception ex)
            {
                // Log del error con ID específico para facilitar debugging
                _logger.LogError(ex, "Error al consultar cliente {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea un nuevo cliente en el sistema con validaciones estrictas
        /// POST api/cliente
        /// REQUIERE: Token JWT válido + datos válidos según FluentValidation
        /// RECIBE: { "nombre": "Juan Pérez", "email": "juan@email.com", "telefono": "+1234567890", ... }
        /// DEVUELVE: Cliente creado con ID asignado + Location header
        /// </summary>
        /// <param name="clienteDto">Datos del nuevo cliente a crear</param>
        /// <returns>HTTP 201 con cliente creado o HTTP 400 con errores de validación</returns>
        [HttpPost]
        public async Task<ActionResult<ClienteDto>> CreateCliente([FromBody] ClienteDto clienteDto)
        {
            try
            {
                // ========== VALIDACIÓN AUTOMÁTICA CON FLUENTVALIDATION ==========
                // Ejecutar todas las reglas de validación definidas en ClienteDtoValidator
                var validationResult = await _validator.ValidateAsync(clienteDto);
                if (!validationResult.IsValid)
                {
                    // Formatear errores para respuesta estructurada
                    var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
                    return BadRequest(new { message = "Datos inválidos", errors }); // HTTP 400
                }

                // ========== CREACIÓN DEL CLIENTE ==========
                // Delegar la lógica de negocio al servicio
                var nuevoCliente = await _clienteService.CreateAsync(clienteDto);
                
                // Log para auditoria de creación (importante para cumplimiento)
                _logger.LogInformation("Cliente creado exitosamente con ID: {Id}", nuevoCliente.IdCliente);
                
                // HTTP 201 Created con Location header apuntando al nuevo recurso
                return CreatedAtAction(nameof(GetCliente), new { id = nuevoCliente.IdCliente }, nuevoCliente);
            }
            catch (Exception ex)
            {
                // Log detallado del error para debugging
                _logger.LogError(ex, "Error al crear cliente");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza completamente un cliente existente (reemplazo total)
        /// PUT api/cliente/{id}
        /// REQUIERE: Token JWT válido + datos válidos + ID coincidente
        /// EJEMPLO: PUT api/cliente/123 con body completo del cliente
        /// NOTA: PUT reemplaza completamente el recurso (vs PATCH que es parcial)
        /// </summary>
        /// <param name="id">ID del cliente a actualizar</param>
        /// <param name="clienteDto">Datos completos actualizados del cliente</param>
        /// <returns>HTTP 200 con cliente actualizado o errores correspondientes</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ClienteDto>> UpdateCliente(int id, [FromBody] ClienteDto clienteDto)
        {
            try
            {
                // ========== VALIDACIÓN DE CONSISTENCIA ID ==========
                // Verificar que el ID de la URL coincida con el ID del DTO
                if (id != clienteDto.IdCliente)
                {
                    return BadRequest(new { message = "El ID del cliente no coincide" }); // HTTP 400
                }

                // ========== VALIDACIÓN AUTOMÁTICA CON FLUENTVALIDATION ==========
                // Aplicar todas las reglas de validación para actualización
                var validationResult = await _validator.ValidateAsync(clienteDto);
                if (!validationResult.IsValid)
                {
                    // Formatear errores de validación para respuesta clara
                    var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
                    return BadRequest(new { message = "Datos inválidos", errors }); // HTTP 400
                }

                // ========== ACTUALIZACIÓN DEL CLIENTE ==========
                // Delegar la lógica de actualización al servicio
                var clienteActualizado = await _clienteService.UpdateAsync(clienteDto);
                if (clienteActualizado == null)
                {
                    // HTTP 404 si el cliente no existe en la base de datos
                    return NotFound(new { message = "Cliente no encontrado" });
                }

                // Log para auditoria de modificaciones (crítico para compliance)
                _logger.LogInformation("Cliente {Id} actualizado exitosamente", id);
                return Ok(clienteActualizado); // HTTP 200 con cliente actualizado
            }
            catch (Exception ex)
            {
                // Log detallado con ID para facilitar debugging
                _logger.LogError(ex, "Error al actualizar cliente {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina un cliente del sistema (soft delete por seguridad)
        /// DELETE api/cliente/{id}
        /// REQUIERE: Token JWT válido
        /// EJEMPLO: DELETE api/cliente/123
        /// NOTA: Implementa "soft delete" - marca como inactivo pero preserva datos para auditoria
        /// IMPORTANTE: Por regulaciones de privacidad, los datos no se eliminan físicamente
        /// </summary>
        /// <param name="id">ID único del cliente a eliminar</param>
        /// <returns>HTTP 200 con confirmación o HTTP 404 si no existe</returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCliente(int id)
        {
            try
            {
                // ========== VERIFICACIÓN DE EXISTENCIA ==========
                // Verificar que el cliente existe antes de intentar eliminarlo
                var cliente = await _clienteService.GetByIdAsync(id);
                if (cliente == null)
                {
                    // HTTP 404 si el cliente no existe
                    return NotFound(new { message = "Cliente no encontrado" });
                }

                // ========== ELIMINACIÓN LÓGICA (SOFT DELETE) ==========
                // No eliminar físicamente por razones de auditoria y compliance
                await _clienteService.DeleteAsync(id);
                
                // Log CRÍTICO para auditoria de eliminaciones (requerimiento legal)
                _logger.LogInformation("Cliente {Id} eliminado exitosamente", id);
                
                return Ok(new { message = "Cliente eliminado exitosamente" }); // HTTP 200
            }
            catch (Exception ex)
            {
                // Log del error con contexto específico
                _logger.LogError(ex, "Error al eliminar cliente {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }
} 