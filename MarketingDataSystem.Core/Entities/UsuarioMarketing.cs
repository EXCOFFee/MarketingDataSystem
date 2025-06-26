// Entidad que representa a un usuario del sistema de marketing
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MarketingDataSystem.Core.Entities
{
    public class UsuarioMarketing : BaseEntity
    {
        [Key] // Clave primaria
        public new int Id { get; set; }
        [Required] // Campo obligatorio
        public string Username { get; set; } = string.Empty; // Nombre de usuario para login
        [Required]
        public string PasswordHash { get; set; } = string.Empty; // Contraseña hasheada
        [Required]
        public string Nombre { get; set; } = string.Empty; // Nombre completo
        [Required]
        public string Email { get; set; } = string.Empty; // Correo electrónico
        [Required]
        public string Role { get; set; } = string.Empty; // Rol del usuario (Admin, User, etc)
        // Relación con DescargaReporte (reportes descargados por el usuario)
        public ICollection<DescargaReporte> Descargas { get; set; } = new List<DescargaReporte>();
    }
} 