using System;

// DTO que representa un usuario en el sistema (para autenticación y gestión)
namespace MarketingDataSystem.Core.DTOs
{
    public class UserDto
    {
        public int Id { get; set; } // Identificador único del usuario
        public string Username { get; set; } = string.Empty; // Nombre de usuario para login
        public string Email { get; set; } = string.Empty; // Correo electrónico del usuario
        public string Role { get; set; } = string.Empty; // Rol del usuario (Admin, User, etc)
    }
} 