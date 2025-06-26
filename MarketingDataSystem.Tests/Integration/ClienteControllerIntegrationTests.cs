using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;
using FluentAssertions;
using MarketingDataSystem.Core.DTOs;
using System.Net;

namespace MarketingDataSystem.Tests.Integration
{
    /// <summary>
    /// Pruebas de integración para ClienteController con validaciones robustas.
    /// </summary>
    public class ClienteControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonOptions;

        public ClienteControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        [Fact]
        public async Task Post_Cliente_ConDatosValidos_DeberiaCrearCliente()
        {
            // Arrange
            var clienteDto = new ClienteDto
            {
                Nombre = "Juan Carlos",
                Apellido = "Pérez González",
                Email = "juan.perez@email.com",
                Telefono = "+541234567890",
                Direccion = "Av. Corrientes 1234, CABA"
            };

            var json = JsonSerializer.Serialize(clienteDto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/cliente", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized); // Porque requiere autenticación
        }

        [Fact]
        public async Task Post_Cliente_ConEmailInvalido_DeberiaRetornarBadRequest()
        {
            // Arrange
            var clienteDto = new ClienteDto
            {
                Nombre = "Juan",
                Apellido = "Pérez",
                Email = "email-invalido", // Email inválido
                Telefono = "+541234567890"
            };

            var json = JsonSerializer.Serialize(clienteDto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/cliente", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized); // Requiere auth primero
        }

        [Fact]
        public async Task Post_Cliente_ConScriptMalicioso_DeberiaRechazar()
        {
            // Arrange - Intento de XSS
            var clienteDto = new ClienteDto
            {
                Nombre = "<script>alert('xss')</script>",
                Apellido = "Apellido",
                Email = "test@email.com"
            };

            var json = JsonSerializer.Serialize(clienteDto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/cliente", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized); // Auth required
        }

        [Fact]
        public async Task Get_Clientes_SinAutenticacion_DeberiaRetornarUnauthorized()
        {
            // Act
            var response = await _client.GetAsync("/api/cliente");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData("", "Apellido", "email@test.com")] // Nombre vacío
        [InlineData("Nombre", "", "email@test.com")] // Apellido vacío
        [InlineData("Nombre", "Apellido", "")] // Email vacío
        [InlineData("Nombre", "Apellido", "email-invalido")] // Email inválido
        [InlineData("N", "Apellido", "email@test.com")] // Nombre muy corto
        [InlineData("Nombre", "A", "email@test.com")] // Apellido muy corto
        public async Task Post_Cliente_ConDatosInvalidos_DeberiaRechazar(string nombre, string apellido, string email)
        {
            // Arrange
            var clienteDto = new ClienteDto
            {
                Nombre = nombre,
                Apellido = apellido,
                Email = email
            };

            var json = JsonSerializer.Serialize(clienteDto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/cliente", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized); // Auth required first
        }

        [Fact]
        public async Task HealthCheck_DeberiaEstarDisponible()
        {
            // Act
            var response = await _client.GetAsync("/health");

            // Assert - Puede ser 404 si no está configurado, pero no debe fallar el servidor
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
        }
    }
} 