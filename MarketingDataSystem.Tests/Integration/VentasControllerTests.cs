using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MarketingDataSystem.Core.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System;

namespace MarketingDataSystem.Tests.Integration
{
    public class VentasControllerTests : BaseApiTest
    {
        public VentasControllerTests(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetAllVentas_ShouldReturnOk()
        {
            // Arrange
            await CleanupDatabase();

            // Act
            var response = await _client.GetAsync("/api/ventas");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetVentaById_WithValidId_ShouldReturnOk()
        {
            // Arrange
            await CleanupDatabase();
            var venta = new VentaDto
            {
                Fecha = DateTime.Now,
                Monto = 1000,
                ProductoId = 1
            };

            var createResponse = await _client.PostAsJsonAsync("/api/ventas", venta);
            var createdVenta = await createResponse.Content.ReadFromJsonAsync<VentaDto>();

            // Act
            var response = await _client.GetAsync($"/api/ventas/{createdVenta.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<VentaDto>();
            Assert.NotNull(result);
            Assert.Equal(createdVenta.Id, result.Id);
        }

        [Fact]
        public async Task CreateVenta_WithValidData_ShouldReturnCreated()
        {
            // Arrange
            await CleanupDatabase();
            var venta = new VentaDto
            {
                Fecha = DateTime.Now,
                Monto = 1000,
                ProductoId = 1
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/ventas", venta);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<VentaDto>();
            Assert.NotNull(result);
            Assert.Equal(venta.Monto, result.Monto);
            Assert.Equal(venta.ProductoId, result.ProductoId);
        }

        [Fact]
        public async Task UpdateVenta_WithValidData_ShouldReturnOk()
        {
            // Arrange
            await CleanupDatabase();
            var venta = new VentaDto
            {
                Fecha = DateTime.Now,
                Monto = 1000,
                ProductoId = 1
            };

            var createResponse = await _client.PostAsJsonAsync("/api/ventas", venta);
            var createdVenta = await createResponse.Content.ReadFromJsonAsync<VentaDto>();

            createdVenta.Monto = 2000;

            // Act
            var response = await _client.PutAsJsonAsync($"/api/ventas/{createdVenta.Id}", createdVenta);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<VentaDto>();
            Assert.NotNull(result);
            Assert.Equal(2000, result.Monto);
        }

        [Fact]
        public async Task DeleteVenta_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            await CleanupDatabase();
            var venta = new VentaDto
            {
                Fecha = DateTime.Now,
                Monto = 1000,
                ProductoId = 1
            };

            var createResponse = await _client.PostAsJsonAsync("/api/ventas", venta);
            var createdVenta = await createResponse.Content.ReadFromJsonAsync<VentaDto>();

            // Act
            var response = await _client.DeleteAsync($"/api/ventas/{createdVenta.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task GetVentasByProducto_WithValidProductoId_ShouldReturnOk()
        {
            // Arrange
            await CleanupDatabase();
            var venta = new VentaDto
            {
                Fecha = DateTime.Now,
                Monto = 1000,
                ProductoId = 1
            };

            await _client.PostAsJsonAsync("/api/ventas", venta);

            // Act
            var response = await _client.GetAsync("/api/ventas/producto/1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<VentaDto>>();
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetVentasByDateRange_WithValidDates_ShouldReturnOk()
        {
            // Arrange
            await CleanupDatabase();
            var venta = new VentaDto
            {
                Fecha = DateTime.Now,
                Monto = 1000,
                ProductoId = 1
            };

            await _client.PostAsJsonAsync("/api/ventas", venta);

            var startDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var endDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            // Act
            var response = await _client.GetAsync($"/api/ventas/rango?startDate={startDate}&endDate={endDate}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<VentaDto>>();
            Assert.NotNull(result);
            Assert.Single(result);
        }
    }
} 