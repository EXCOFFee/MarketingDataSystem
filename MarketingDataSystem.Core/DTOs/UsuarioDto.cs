using System;

// DTO que representa un usuario del sistema principal
namespace MarketingDataSystem.Core.DTOs
{
    /// <summary>
    /// DTO para usuarios del sistema principal - COMPATIBILIDAD CON TESTS
    /// PROPÓSITO: Transferencia de datos de usuarios entre capas
    /// CASOS DE USO: Tests unitarios, APIs REST, servicios de usuario
    /// </summary>
    public class UsuarioDto
    {
        /// <summary>
        /// Identificador único del usuario
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Nombre completo del usuario
        /// </summary>
        public string Nombre { get; set; } = string.Empty;
        
        /// <summary>
        /// Correo electrónico del usuario
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Rol del usuario (Admin, User, Manager, etc.)
        /// </summary>
        public string Role { get; set; } = string.Empty;
        
        /// <summary>
        /// Nombre de usuario para login
        /// </summary>
        public string Username { get; set; } = string.Empty;
        
        /// <summary>
        /// Hash de la contraseña (solo para casos específicos)
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;
        
        /// <summary>
        /// Fecha de creación del usuario
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Fecha de última modificación
        /// </summary>
        public DateTime? FechaModificacion { get; set; }
        
        /// <summary>
        /// Estado activo/inactivo del usuario
        /// </summary>
        public bool Activo { get; set; } = true;

        // ========== PROPIEDADES PARA COMPATIBILIDAD CON TESTS ==========
        /// <summary>
        /// Alias para Role - Compatibilidad con tests existentes
        /// </summary>
        public string Rol 
        { 
            get => Role; 
            set => Role = value; 
        }
    }
}
