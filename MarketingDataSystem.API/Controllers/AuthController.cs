// ==================== CONTROLADOR DE AUTENTICACIÓN ====================
// Este controlador maneja todos los aspectos de autenticación y autorización del sistema
// Incluye: login, registro, cambio de contraseña y recuperación de contraseña
// NOTA: Este controlador NO requiere autenticación ([Authorize]) porque es el punto de entrada

using Microsoft.AspNetCore.Mvc;
using MarketingDataSystem.Application.Services;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.API.Controllers
{
    [ApiController] // Habilita comportamientos automáticos de API (validación, serialización JSON)
    [Route("api/[controller]")] // Ruta base: api/auth - [controller] se reemplaza por "Auth"
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService; // Servicio que contiene toda la lógica de autenticación

        /// <summary>
        /// Constructor con inyección de dependencias
        /// El framework automáticamente inyecta la implementación de IAuthService
        /// </summary>
        /// <param name="authService">Servicio de autenticación registrado en Program.cs</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Endpoint para autenticar usuarios y generar token JWT
        /// POST api/auth/login
        /// Recibe: { "email": "usuario@email.com", "password": "contraseña" }
        /// Devuelve: { "token": "jwt_token", "user": { datos_usuario }, "expiresAt": "fecha" }
        /// </summary>
        /// <param name="loginDto">Credenciales del usuario (email y contraseña)</param>
        /// <returns>Token JWT y datos del usuario si las credenciales son válidas</returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                // Validar credenciales contra la base de datos y generar token JWT
                var response = await _authService.LoginAsync(loginDto);
                return Ok(response); // HTTP 200 con token y datos del usuario
            }
            catch (UnauthorizedAccessException)
            {
                // HTTP 401 si las credenciales son incorrectas
                return Unauthorized(new { message = "Credenciales inválidas" });
            }
        }

        /// <summary>
        /// Endpoint para registrar nuevos usuarios en el sistema
        /// POST api/auth/register
        /// Recibe: { "username": "usuario", "email": "email@domain.com", "password": "password", "nombre": "Nombre Completo" }
        /// Devuelve: datos del usuario creado (sin contraseña)
        /// </summary>
        /// <param name="createUserDto">Datos del nuevo usuario a crear</param>
        /// <returns>Datos del usuario creado sin información sensible</returns>
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                // Crear nuevo usuario en la base de datos con contraseña hasheada
                var user = await _authService.RegisterAsync(createUserDto);
                return Ok(user); // HTTP 200 con datos del usuario creado
            }
            catch (InvalidOperationException ex)
            {
                // HTTP 400 si hay errores de validación (email duplicado, etc.)
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Endpoint para cambiar la contraseña de un usuario autenticado
        /// POST api/auth/change-password
        /// REQUIERE: Token JWT válido en el header Authorization
        /// Recibe: { "currentPassword": "contraseña_actual", "newPassword": "nueva_contraseña" }
        /// </summary>
        /// <param name="changePasswordDto">Contraseña actual y nueva contraseña</param>
        /// <returns>Confirmación de cambio exitoso</returns>
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                // Extraer el ID del usuario del token JWT
                var userIdClaim = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    // El usuario debe estar autenticado para cambiar su contraseña
                    return Unauthorized(new { message = "Usuario no autenticado" });
                }

                var userId = int.Parse(userIdClaim);
                // Verificar contraseña actual y actualizar con la nueva
                var success = await _authService.ChangePasswordAsync(
                    userId,
                    changePasswordDto.CurrentPassword,
                    changePasswordDto.NewPassword
                );
                return Ok(new { message = "Contraseña actualizada exitosamente" });
            }
            catch (UnauthorizedAccessException)
            {
                // HTTP 401 si la contraseña actual es incorrecta
                return Unauthorized(new { message = "Contraseña actual incorrecta" });
            }
            catch (KeyNotFoundException)
            {
                // HTTP 404 si el usuario no existe (caso muy raro)
                return NotFound(new { message = "Usuario no encontrado" });
            }
        }

        /// <summary>
        /// Endpoint para solicitar restablecimiento de contraseña
        /// POST api/auth/reset-password
        /// Recibe: { "email": "usuario@email.com" }
        /// Simula envío de email con instrucciones (en producción enviaría email real)
        /// </summary>
        /// <param name="resetPasswordDto">Email del usuario que olvido su contraseña</param>
        /// <returns>Confirmación de envío de email</returns>
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                // En producción esto enviaría un email real con link de recuperación
                var success = await _authService.ResetPasswordAsync(resetPasswordDto.Email);
                return Ok(new { message = "Se ha enviado un correo con instrucciones para restablecer la contraseña" });
            }
            catch (KeyNotFoundException)
            {
                // HTTP 404 si el email no existe en el sistema
                return NotFound(new { message = "Usuario no encontrado" });
            }
        }
    }

    // ==================== DTOs ESPECÍFICOS DEL CONTROLADOR ====================
    // DTOs locales para endpoints específicos de autenticación

    /// <summary>
    /// DTO para cambio de contraseña de usuario autenticado
    /// Requiere contraseña actual para verificar identidad
    /// </summary>
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty; // Contraseña actual para validación
        public string NewPassword { get; set; } = string.Empty;     // Nueva contraseña deseada
    }

    /// <summary>
    /// DTO para solicitud de restablecimiento de contraseña
    /// Solo requiere email para enviar instrucciones de recuperación
    /// </summary>
    public class ResetPasswordDto
    {
        public string Email { get; set; } = string.Empty; // Email del usuario que olvido contraseña
    }
} 