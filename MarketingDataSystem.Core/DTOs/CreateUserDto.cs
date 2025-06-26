using System;

// DTO para la creación de un nuevo usuario en el sistema
namespace MarketingDataSystem.Core.DTOs
{
    public class CreateUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
} 