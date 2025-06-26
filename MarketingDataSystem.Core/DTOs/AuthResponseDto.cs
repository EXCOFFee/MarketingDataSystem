using System;

// DTO que representa la respuesta de autenticación (login)
namespace MarketingDataSystem.Core.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = new();
    }
} 