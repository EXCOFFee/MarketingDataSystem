using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using MarketingDataSystem.API;
using MarketingDataSystem.Core.DTOs;
using Newtonsoft.Json;
using System.Text;

namespace MarketingDataSystem.Tests.Integration
{
    public class ReportesControllerIntegrationTests : BaseApiTest
    {
        public ReportesControllerIntegrationTests(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task GenerarReporteVentas_ConTokenValido_DebeRetornarArchivoExcel()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new
            {
                FechaInicio = DateTime.Now.AddDays(-30),
                FechaFin = DateTime.Now,
                TipoReporte = "Ventas"
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/reports/generar", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                         response.Content.Headers.ContentType?.MediaType);
            
            var contentLength = response.Content.Headers.ContentLength;
            Assert.True(contentLength > 0, "El archivo Excel debe tener contenido");
        }

        [Fact]
        public async Task GenerarReporteClientes_ConTokenValido_DebeRetornarArchivoExcel()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new
            {
                FechaInicio = DateTime.Now.AddDays(-30),
                FechaFin = DateTime.Now,
                TipoReporte = "Clientes"
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/reports/generar", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                         response.Content.Headers.ContentType?.MediaType);
        }

        [Fact]
        public async Task GenerarReporteProductos_ConTokenValido_DebeRetornarArchivoExcel()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new
            {
                FechaInicio = DateTime.Now.AddDays(-30),
                FechaFin = DateTime.Now,
                TipoReporte = "Productos"
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/reports/generar", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                         response.Content.Headers.ContentType?.MediaType);
        }

        [Fact]
        public async Task GenerarReporte_SinToken_DebeRetornarUnauthorized()
        {
            // Arrange
            var request = new
            {
                FechaInicio = DateTime.Now.AddDays(-30),
                FechaFin = DateTime.Now,
                TipoReporte = "Ventas"
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/reports/generar", content);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GenerarReporte_ConFechasInvalidas_DebeRetornarBadRequest()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new
            {
                FechaInicio = DateTime.Now.AddDays(1), // Fecha futura
                FechaFin = DateTime.Now.AddDays(-30),  // Fecha anterior a inicio
                TipoReporte = "Ventas"
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/reports/generar", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GenerarReporte_ConTipoReporteInvalido_DebeRetornarBadRequest()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new
            {
                FechaInicio = DateTime.Now.AddDays(-30),
                FechaFin = DateTime.Now,
                TipoReporte = "TipoInexistente"
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/reports/generar", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ListarReportes_ConTokenValido_DebeRetornarListaReportes()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/api/reports");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var reportes = JsonConvert.DeserializeObject<List<ReporteDto>>(responseContent);
            
            Assert.NotNull(reportes);
            Assert.IsType<List<ReporteDto>>(reportes);
        }

        [Fact]
        public async Task ObtenerReporte_ConIdValido_DebeRetornarReporte()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Primero generar un reporte
            var requestGenerar = new
            {
                FechaInicio = DateTime.Now.AddDays(-7),
                FechaFin = DateTime.Now,
                TipoReporte = "Ventas"
            };

            var jsonGenerar = JsonConvert.SerializeObject(requestGenerar);
            var contentGenerar = new StringContent(jsonGenerar, Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/reports/generar", contentGenerar);

            // Obtener lista de reportes
            var responseListar = await _client.GetAsync("/api/reports");
            var responseContent = await responseListar.Content.ReadAsStringAsync();
            var reportes = JsonConvert.DeserializeObject<List<ReporteDto>>(responseContent);
            
            if (reportes?.Any() == true)
            {
                var reporteId = reportes.First().Id;

                // Act
                var response = await _client.GetAsync($"/api/reports/{reporteId}");

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                
                var reporteContent = await response.Content.ReadAsStringAsync();
                var reporte = JsonConvert.DeserializeObject<ReporteDto>(reporteContent);
                
                Assert.NotNull(reporte);
                Assert.Equal(reporteId, reporte.Id);
            }
        }

        [Fact]
        public async Task DescargarReporte_ConIdValido_DebeRetornarArchivoExcel()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Primero generar un reporte
            var requestGenerar = new
            {
                FechaInicio = DateTime.Now.AddDays(-7),
                FechaFin = DateTime.Now,
                TipoReporte = "Ventas"
            };

            var jsonGenerar = JsonConvert.SerializeObject(requestGenerar);
            var contentGenerar = new StringContent(jsonGenerar, Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/reports/generar", contentGenerar);

            // Obtener lista de reportes
            var responseListar = await _client.GetAsync("/api/reports");
            var responseContent = await responseListar.Content.ReadAsStringAsync();
            var reportes = JsonConvert.DeserializeObject<List<ReporteDto>>(responseContent);
            
            if (reportes?.Any() == true)
            {
                var reporteId = reportes.First().Id;

                // Act
                var response = await _client.GetAsync($"/api/reports/{reporteId}/descargar");

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                             response.Content.Headers.ContentType?.MediaType);
                
                var contentLength = response.Content.Headers.ContentLength;
                Assert.True(contentLength > 0, "El archivo descargado debe tener contenido");
            }
        }

        [Fact]
        public async Task GenerarReportePersonalizado_ConParametrosComplejos_DebeRetornarExcel()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new
            {
                FechaInicio = DateTime.Now.AddDays(-30),
                FechaFin = DateTime.Now,
                TipoReporte = "Personalizado",
                Filtros = new
                {
                    ClienteId = 1,
                    ProductoCategoria = "Electrónicos",
                    RangoPrecios = new { Min = 100, Max = 1000 }
                },
                Columnas = new[] { "Fecha", "Cliente", "Producto", "Cantidad", "Total" }
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/reports/generar-personalizado", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                         response.Content.Headers.ContentType?.MediaType);
        }

        [Fact]
        public async Task ValidarPipelineCompleto_GenerarReporte_DebeCompletarFlujoETL()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Simular activación del proceso ETL
            var responseETL = await _client.PostAsync("/api/ingestion/ejecutar-etl", null);
            Assert.Equal(HttpStatusCode.OK, responseETL.StatusCode);

            // Esperar un momento para que el proceso termine
            await Task.Delay(2000);

            // Generar reporte después del ETL
            var request = new
            {
                FechaInicio = DateTime.Now.AddDays(-1),
                FechaFin = DateTime.Now,
                TipoReporte = "Ventas"
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/reports/generar", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                         response.Content.Headers.ContentType?.MediaType);
        }
    }
} 