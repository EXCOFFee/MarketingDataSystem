using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketingDataSystem.Core.Entities
{
    /// <summary>
    /// Entidad que representa un usuario del sistema (histórico o extendido)
    /// </summary>
    public class Usuario : BaseEntity
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public ICollection<DescargaReporte> Descargas { get; set; } = new List<DescargaReporte>();

        /// <summary>
        /// Nombre de usuario para el login
        /// </summary>
        public string? NombreUsuario { get; set; }

        /// <summary>
        /// Nombre completo del usuario
        /// </summary>
        public string? NombreCompleto { get; set; }

        /// <summary>
        /// Rol del usuario en el sistema
        /// </summary>
        public string? Rol { get; set; }

        /// <summary>
        /// Último acceso del usuario al sistema
        /// </summary>
        public DateTime? UltimoAcceso { get; set; }

        /// <summary>
        /// Colección de campañas creadas por el usuario (eliminado)
        /// </summary>
        // public virtual ICollection<Campaña> Campañas { get; set; }
    }
} 