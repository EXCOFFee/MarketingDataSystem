// ==================== DATA TRANSFER OBJECT - REGISTRO DE USUARIOS ====================
// Este DTO define el contrato de comunicación para registro de nuevos usuarios
// PROPÓSITO: Entrada segura para creación de cuentas de usuario en el sistema
// SEGURIDAD: Incluye validaciones estrictas y roles predeterminados seguros
// ARQUITECTURA: Capa Core - DTOs compartidos entre todas las capas
// AUTENTICACIÓN: Integrado con JWT Bearer Token authentication
// AUTORIZACIÓN: Soporte para roles empresariales (Admin, User, Marketing)
// COMPLIANCE: Cumple con estándares de seguridad empresarial

using System.ComponentModel.DataAnnotations;

namespace MarketingDataSystem.Core.DTOs
{
    /// <summary>
    /// Data Transfer Object para registro de nuevos usuarios en el sistema
    /// RESPONSABILIDAD: Definir estructura de datos para creación segura de cuentas
    /// SEGURIDAD: Incluye validaciones robustas para email, password y roles
    /// AUTENTICACIÓN: Usado en endpoint POST /api/auth/register
    /// AUTORIZACIÓN: Establece roles predeterminados seguros (User por defecto)
    /// VALIDACIÓN: FluentValidation aplica reglas de negocio empresariales
    /// COMPATIBILIDAD: Propiedades duplicadas para compatibilidad con tests legacy
    /// CASOS DE USO:
    /// - Registro de nuevos usuarios desde interfaz web
    /// - Creación de cuentas por administradores
    /// - Importación masiva de usuarios empresariales
    /// - Integración con sistemas externos de HR/LDAP
    /// </summary>
    public class RegisterDto
    {
        // ========== INFORMACIÓN DE AUTENTICACIÓN CRÍTICA ==========
        /// <summary>
        /// Dirección de correo electrónico - Identificador único del usuario
        /// VALIDACIÓN: Formato RFC 5321 obligatorio, verificación de dominio
        /// ÚNICO: No puede haber dos usuarios con el mismo email
        /// COMUNICACIÓN: Usado para notificaciones, recuperación de password
        /// SEGURIDAD: Verificado contra listas de emails corporativos válidos
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña segura para acceso al sistema
        /// VALIDACIÓN: Mínimo 6 caracteres (configurable para mayor seguridad)
        /// SEGURIDAD: Hasheada con BCrypt antes de almacenamiento
        /// POLÍTICA: Debe incluir mayúsculas, minúsculas, números (futuro)
        /// EXPIRACIÓN: Configurable para cumplir políticas empresariales
        /// </summary>
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        // ========== INFORMACIÓN PERSONAL DEL USUARIO ==========
        /// <summary>
        /// Nombre completo del usuario
        /// VALIDACIÓN: Campo obligatorio, longitud mínima 2 caracteres
        /// DISPLAY: Mostrado en interfaz de usuario y reportes
        /// AUDITORÍA: Incluido en logs de acceso para trazabilidad
        /// </summary>
        [Required]
        public string Nombre { get; set; } = string.Empty;
        
        /// <summary>
        /// Nombre de usuario alternativo - Identificador legible para el usuario
        /// VALIDACIÓN: Campo obligatorio, único en el sistema
        /// LEGACY: Mantenido para compatibilidad con sistema anterior
        /// DISPLAY: Mostrado en interfaces y reportes administrativos
        /// </summary>
        [Required]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Apellido del usuario - Información complementaria
        /// OPCIONAL: Para permitir flexibilidad en naming conventions
        /// REPORTES: Usado en generación de reportes formales
        /// </summary>
        public string? Apellido { get; set; }
        
        /// <summary>
        /// Número telefónico de contacto del usuario
        /// OPCIONAL: Para notificaciones críticas y verificación 2FA
        /// FUTURO: Integración con SMS gateway para alertas
        /// </summary>
        public string? Telefono { get; set; }
        
        /// <summary>
        /// Dirección física del usuario
        /// OPCIONAL: Para reportes empresariales y compliance
        /// AUDITORÍA: Requerido para roles administrativos (futuro)
        /// </summary>
        public string? Direccion { get; set; }
        
        // ========== AUTORIZACIÓN Y ROLES EMPRESARIALES ==========
        /// <summary>
        /// Rol predeterminado del usuario en el sistema
        /// DEFAULT: "User" - Rol con menor privilegio por seguridad
        /// ROLES DISPONIBLES: User, Admin, Marketing, Supervisor
        /// ESCALACIÓN: Solo Admin puede asignar roles privilegiados
        /// AUDITORÍA: Cambios de rol registrados en audit log
        /// </summary>
        public string Role { get; set; } = "User";
        
        /// <summary>
        /// Alias en español para Role - Compatibilidad con tests legacy y UI localized
        /// PROPÓSITO: Evitar breaking changes en tests existentes
        /// SINCRONIZACIÓN: Mantenido en sync con propiedad Role
        /// LOCALIZACIÓN: Soporte para interfaces en español
        /// </summary>
        public string Rol { get; set; } = "User"; // Alias en español para compatibilidad

        // ========== NOTAS DE DISEÑO ARQUITECTURAL ==========
        // DELIBERADAMENTE OMITIDO: Información sensible (passwords hasheados, tokens)
        // RAZÓN: Los DTOs deben contener solo datos de entrada, no estado interno
        // SEGURIDAD: Password nunca almacenado en texto plano en DTO
        // EXTENSIBILIDAD: Preparado para campos adicionales (2FA, preferences)
        // FUTURO: Considerar deprecación de Username en favor de Email único
    }
} 