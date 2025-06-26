using System;

// DTO que representa un usuario de marketing en el sistema
namespace MarketingDataSystem.Core.DTOs
{
    public class UsuarioMarketingDto
    {
        public int Id { get; set; } // Identificador único del usuario
        public string Nombre { get; set; } = string.Empty; // Nombre completo del usuario
        public string Email { get; set; } = string.Empty; // Correo electrónico del usuario
        public string Role { get; set; } = string.Empty; // Rol del usuario (Admin, User, etc)
        // Puedes agregar más propiedades según el dominio
    }
} 