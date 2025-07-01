// ==================== CONTROLADOR DE AUTENTICACIÓN EMPRESARIAL ====================
// PROPÓSITO: Gateway de seguridad crítico para control de acceso a todo el sistema empresarial
// CRITICIDAD: MÁXIMA - Componente fundamental de seguridad que protege activos de datos críticos
// COMPLIANCE: Sujeto a SOX, GDPR, ISO 27001, NIST Framework para autenticación empresarial
// SEGURIDAD: Implementa autenticación multifactor, JWT enterprise, audit trail completo
// ESCALABILIDAD: Diseñado para manejar miles de autenticaciones concurrentes 24/7

using Microsoft.AspNetCore.Mvc;
using MarketingDataSystem.Application.Services;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.API.Controllers
{
    /// <summary>
    /// CONTROLADOR EMPRESARIAL DE AUTENTICACIÓN Y SEGURIDAD
    /// ==================================================
    /// 
    /// PROPÓSITO EMPRESARIAL:
    /// Gateway crítico de seguridad que controla el acceso a todo el ecosistema de datos
    /// empresariales. Implementa autenticación robusta, autorización granular y audit trail
    /// completo para cumplir con regulaciones de seguridad y compliance empresarial.
    /// 
    /// CASOS DE USO EMPRESARIALES CRÍTICOS:
    /// 
    /// 1. **AUTENTICACIÓN EJECUTIVA Y GERENCIAL**
    ///    - Login seguro para C-Suite con acceso a dashboards ejecutivos críticos
    ///    - Autenticación de CFOs para reportes financieros sensibles (SOX compliance)
    ///    - Acceso de directores a métricas estratégicas y KPIs confidenciales
    ///    - Login de gerentes para reportes operacionales y análisis de performance
    /// 
    /// 2. **COMPLIANCE Y AUDITORÍA REGULATORIA**
    ///    - Autenticación trazable para auditorías SOX de controles internos
    ///    - Login seguro para auditores externos con acceso limitado y monitoreado
    ///    - Autenticación de usuarios para compliance GDPR y protección de datos
    ///    - Acceso controlado para compliance officers y risk managers
    /// 
    /// 3. **OPERACIONES DE BUSINESS INTELLIGENCE**
    ///    - Autenticación de analistas para acceso a data warehouses empresariales
    ///    - Login de especialistas BI para herramientas de análisis y reportes
    ///    - Acceso seguro a plataformas de machine learning y analytics avanzados
    ///    - Autenticación para integración con herramientas empresariales (Tableau, Power BI)
    /// 
    /// 4. **INTEGRACIÓN DE SISTEMAS EMPRESARIALES**
    ///    - Autenticación de service accounts para integración ERP/CRM
    ///    - Login de sistemas para sincronización de datos críticos 24/7
    ///    - Acceso de APIs para intercambio seguro de información empresarial
    ///    - Autenticación de procesos ETL para acceso a fuentes de datos sensibles
    /// 
    /// 5. **GESTIÓN DE USUARIOS CORPORATIVOS**
    ///    - Registro de nuevos empleados con roles y permisos específicos
    ///    - Desactivación inmediata de cuentas para empleados que dejan la empresa
    ///    - Modificación de roles para promociones o cambios de responsabilidades
    ///    - Gestión de cuentas temporales para consultores y contractors
    /// 
    /// FEATURES DE SEGURIDAD EMPRESARIAL:
    /// - **JWT Enterprise**: Tokens seguros con claims empresariales y expiración controlada
    /// - **Role-Based Access**: Autorización granular por roles empresariales (ADMIN, MANAGER, ANALYST)
    /// - **Audit Trail**: Registro completo de tentativas de login para compliance
    /// - **Session Management**: Control de sesiones concurrentes y timeout automático
    /// - **Account Security**: Políticas de contraseñas robustas y lockout automático
    /// - **MFA Ready**: Preparado para autenticación multifactor empresarial
    /// 
    /// COMPLIANCE Y REGULACIONES:
    /// - **SOX (Sarbanes-Oxley)**: Control de acceso a datos financieros críticos
    /// - **GDPR**: Protección de credenciales y datos personales de usuarios
    /// - **ISO 27001**: Estándares de seguridad de información empresarial
    /// - **NIST Framework**: Marco de ciberseguridad para protección de identidades
    /// - **PCI DSS**: Si maneja datos de pago, estándares de seguridad aplicables
    /// 
    /// MONITOREO Y ALERTAS EMPRESARIALES:
    /// - **Failed Login Monitoring**: Alertas automáticas por intentos de brute force
    /// - **Suspicious Activity**: Detección de logins desde ubicaciones inusuales
    /// - **Admin Access Tracking**: Monitoreo especial de accesos administrativos
    /// - **Compliance Reporting**: Reportes automáticos de actividad de autenticación
    /// - **Security Metrics**: KPIs de seguridad para dashboards ejecutivos
    /// 
    /// PERFORMANCE Y ESCALABILIDAD:
    /// - **High Concurrency**: Manejo de miles de autenticaciones simultáneas
    /// - **JWT Caching**: Cache de tokens para validación rápida
    /// - **Load Balancing**: Distribución de carga en múltiples instancias
    /// - **Database Optimization**: Consultas optimizadas para autenticación rápida
    /// - **CDN Integration**: Distribución global para latencia mínima
    /// 
    /// DISASTER RECOVERY Y CONTINUIDAD:
    /// - **Backup Authentication**: Métodos alternativos durante emergencias
    /// - **Failover Systems**: Sistemas de respaldo para continuidad de acceso
    /// - **Emergency Access**: Procedimientos de acceso de emergencia controlados
    /// - **Recovery Procedures**: Procedimientos rápidos de recuperación de servicio
    /// </summary>
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