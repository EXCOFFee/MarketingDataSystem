using System;

// DTO para el login de usuario en el sistema
namespace MarketingDataSystem.Core.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
} 