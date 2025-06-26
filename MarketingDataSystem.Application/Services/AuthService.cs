using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MarketingDataSystem.Core.DTOs;
using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using BC = BCrypt.Net.BCrypt;
using System;
using System.Linq;
using System.Threading.Tasks;
using MarketingDataSystem.Application.Interfaces;

namespace MarketingDataSystem.Application.Services
{
    // Servicio de autenticación y gestión de usuarios
    public class AuthService : IAuthService
    {
        private readonly IRepository<UsuarioMarketing> _userRepository; // Repositorio de usuarios
        private readonly IConfiguration _configuration; // Configuración de la app (para JWT)
        private readonly IEventBus _eventBus; // Bus de eventos (para notificaciones)

        // Inyección de dependencias
        public AuthService(IRepository<UsuarioMarketing> userRepository, IConfiguration configuration, IEventBus eventBus)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _eventBus = eventBus;
        }

        // Autentica un usuario y devuelve un token JWT
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var users = await _userRepository.FindAsync(u => u.Email == loginDto.Email); // Busca usuario por email
            var user = users.FirstOrDefault();

            if (user == null || !BC.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }

            var token = GenerateJwtToken(user); // Genera el token JWT

            return new AuthResponseDto
            {
                Token = token,
                User = MapToDto(user)
            };
        }

        // Registra un nuevo usuario
        public async Task<UserDto> RegisterAsync(CreateUserDto createUserDto)
        {
            var existingUsers = await _userRepository.FindAsync(u => u.Email == createUserDto.Email);
            if (existingUsers.Any())
            {
                throw new InvalidOperationException("El email ya está registrado");
            }

            var newUser = new UsuarioMarketing
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                PasswordHash = BC.HashPassword(createUserDto.Password), // Hashea la contraseña
                Role = createUserDto.Role,
            };

            await _userRepository.AddAsync(newUser); // Agrega el usuario
            await _userRepository.SaveChangesAsync(); // Guarda cambios

            return MapToDto(newUser);
        }

        // Cambia la contraseña de un usuario
        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado");
            }

            if (!BC.Verify(currentPassword, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Contraseña actual incorrecta");
            }

            user.PasswordHash = BC.HashPassword(newPassword); // Actualiza la contraseña
            _userRepository.Update(user);

            return true;
        }

        // Restablece la contraseña de un usuario (envía email, simulado)
        public async Task<bool> ResetPasswordAsync(string email)
        {
            var users = await _userRepository.FindAsync(u => u.Email == email);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado");
            }

            // Aquí se implementaría la lógica para enviar un email con el enlace de restablecimiento
            // Por ahora solo retornamos true
            return true;
        }

        // Genera un token JWT para el usuario autenticado
        private string GenerateJwtToken(UsuarioMarketing user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Mapea la entidad UsuarioMarketing a UserDto
        private UserDto MapToDto(UsuarioMarketing user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
} 