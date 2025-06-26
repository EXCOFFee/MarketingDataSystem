using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MarketingDataSystem.Core.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MarketingDataSystem.Tests.Integration
{
    public class UsuariosControllerTests : BaseApiTest
    {
        public UsuariosControllerTests(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetAllUsuarios_ShouldReturnOk()
        {
            // Arrange
            await CleanupDatabase();

            // Act
            var response = await _client.GetAsync("/api/usuarios");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetUsuarioById_WithValidId_ShouldReturnOk()
        {
            // Arrange
            await CleanupDatabase();
            var usuario = new UsuarioDto
            {
                Nombre = "Test User",
                Email = "test@example.com",
                Rol = "Vendedor"
            };

            var createResponse = await _client.PostAsJsonAsync("/api/usuarios", usuario);
            var createdUsuario = await createResponse.Content.ReadFromJsonAsync<UsuarioDto>();

            // Act
            var response = await _client.GetAsync($"/api/usuarios/{createdUsuario.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<UsuarioDto>();
            Assert.NotNull(result);
            Assert.Equal(createdUsuario.Id, result.Id);
        }

        [Fact]
        public async Task CreateUsuario_WithValidData_ShouldReturnCreated()
        {
            // Arrange
            await CleanupDatabase();
            var usuario = new UsuarioDto
            {
                Nombre = "New User",
                Email = "new@example.com",
                Rol = "Marketing"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/usuarios", usuario);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<UsuarioDto>();
            Assert.NotNull(result);
            Assert.Equal(usuario.Nombre, result.Nombre);
            Assert.Equal(usuario.Email, result.Email);
            Assert.Equal(usuario.Rol, result.Rol);
        }

        [Fact]
        public async Task UpdateUsuario_WithValidData_ShouldReturnOk()
        {
            // Arrange
            await CleanupDatabase();
            var usuario = new UsuarioDto
            {
                Nombre = "Original User",
                Email = "original@example.com",
                Rol = "Vendedor"
            };

            var createResponse = await _client.PostAsJsonAsync("/api/usuarios", usuario);
            var createdUsuario = await createResponse.Content.ReadFromJsonAsync<UsuarioDto>();

            createdUsuario.Nombre = "Updated User";
            createdUsuario.Rol = "Marketing";

            // Act
            var response = await _client.PutAsJsonAsync($"/api/usuarios/{createdUsuario.Id}", createdUsuario);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<UsuarioDto>();
            Assert.NotNull(result);
            Assert.Equal("Updated User", result.Nombre);
            Assert.Equal("Marketing", result.Rol);
        }

        [Fact]
        public async Task DeleteUsuario_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            await CleanupDatabase();
            var usuario = new UsuarioDto
            {
                Nombre = "Test User",
                Email = "test@example.com",
                Rol = "Vendedor"
            };

            var createResponse = await _client.PostAsJsonAsync("/api/usuarios", usuario);
            var createdUsuario = await createResponse.Content.ReadFromJsonAsync<UsuarioDto>();

            // Act
            var response = await _client.DeleteAsync($"/api/usuarios/{createdUsuario.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task GetUsuariosByRol_WithValidRol_ShouldReturnOk()
        {
            // Arrange
            await CleanupDatabase();
            var usuario = new UsuarioDto
            {
                Nombre = "Test User",
                Email = "test@example.com",
                Rol = "Vendedor"
            };

            await _client.PostAsJsonAsync("/api/usuarios", usuario);

            // Act
            var response = await _client.GetAsync("/api/usuarios/rol/Vendedor");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<UsuarioDto>>();
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetUsuarioByEmail_WithValidEmail_ShouldReturnOk()
        {
            // Arrange
            await CleanupDatabase();
            var usuario = new UsuarioDto
            {
                Nombre = "Test User",
                Email = "test@example.com",
                Rol = "Vendedor"
            };

            await _client.PostAsJsonAsync("/api/usuarios", usuario);

            // Act
            var response = await _client.GetAsync("/api/usuarios/email/test@example.com");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<UsuarioDto>();
            Assert.NotNull(result);
            Assert.Equal("test@example.com", result.Email);
        }
    }
} 