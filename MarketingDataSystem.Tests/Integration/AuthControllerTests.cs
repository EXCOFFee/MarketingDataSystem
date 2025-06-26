using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MarketingDataSystem.Core.DTOs;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MarketingDataSystem.Tests.Integration
{
    public class AuthControllerTests : BaseApiTest
    {
        public AuthControllerTests(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Register_WithValidData_ShouldReturnCreated()
        {
            // Arrange
            await CleanupDatabase();
            var registerDto = new RegisterDto
            {
                Nombre = "Test User",
                Email = "test@example.com",
                Password = "Password123!",
                Rol = "Vendedor"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.NotNull(result);
            Assert.Equal(registerDto.Username, result.Username);
            Assert.Equal(registerDto.Email, result.Email);
        }

        [Fact]
        public async Task Register_WithInvalidData_ShouldReturnBadRequest()
        {
            // Arrange
            await CleanupDatabase();
            var registerDto = new CreateUserDto
            {
                Username = "",
                Email = "invalid-email",
                Password = "123",
                Role = "InvalidRole"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnOk()
        {
            // Arrange
            await CleanupDatabase();
            var registerDto = new CreateUserDto
            {
                Username = "Test User",
                Email = "test@example.com",
                Password = "Password123!",
                Role = "Vendedor"
            };

            await _client.PostAsJsonAsync("/api/auth/register", registerDto);

            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "Password123!"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            Assert.NotNull(result);
            Assert.NotNull(result.Token);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
        {
            // Arrange
            await CleanupDatabase();
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "WrongPassword123!"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetCurrentUser_WithValidToken_ShouldReturnOk()
        {
            // Arrange
            await CleanupDatabase();
            var registerDto = new CreateUserDto
            {
                Username = "Test User",
                Email = "test@example.com",
                Password = "Password123!",
                Role = "Vendedor"
            };

            await _client.PostAsJsonAsync("/api/auth/register", registerDto);

            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "Password123!"
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            var loginResult = await loginResponse.Content.ReadFromJsonAsync<AuthResponseDto>();

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult.Token);

            // Act
            var response = await _client.GetAsync("/api/auth/me");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.NotNull(result);
            Assert.Equal(registerDto.Email, result.Email);
        }

        [Fact]
        public async Task GetCurrentUser_WithoutToken_ShouldReturnUnauthorized()
        {
            // Arrange
            await CleanupDatabase();
            _client.DefaultRequestHeaders.Authorization = null;

            // Act
            var response = await _client.GetAsync("/api/auth/me");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
} 