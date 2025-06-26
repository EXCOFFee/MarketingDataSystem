using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(CreateUserDto createUserDto);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> ResetPasswordAsync(string email);
    }
} 