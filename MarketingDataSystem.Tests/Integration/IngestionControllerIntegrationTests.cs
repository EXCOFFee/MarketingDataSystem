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
    public class IngestionControllerIntegrationTests : BaseApiTest
    {
        public IngestionControllerIntegrationTests(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task EjecutarETL_ConTokenValido_DebeCompletarProceso()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.PostAsync("/api/ingestion/ejecutar-etl", null);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.True((bool)resultado.success);
            Assert.Contains("ETL ejecutado exitosamente", (string)resultado.message);
        }

        [Fact]
        public async Task EjecutarETL_SinToken_DebeRetornarUnauthorized()
        {
            // Arrange - No se configura token

            // Act
            var response = await _client.PostAsync("/api/ingestion/ejecutar-etl", null);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task IngresarDatosCrudos_ConDatosValidos_DebeRetornarCreated()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var datosCrudos = new[]
            {
                new DatoCrudoDto
                {
                    Contenido = JsonConvert.SerializeObject(new
                    {
                        producto_id = "PROD001",
                        nombre = "Laptop HP Test",
                        precio = 1500.50,
                        categoria = "Electrónicos"
                    }),
                    Timestamp = DateTime.Now,
                    Origen = "API_TEST_INTEGRATION"
                },
                new DatoCrudoDto
                {
                    Contenido = JsonConvert.SerializeObject(new
                    {
                        cliente_id = "CLI001",
                        nombre = "Juan Pérez Test",
                        email = "juan.test@email.com"
                    }),
                    Timestamp = DateTime.Now,
                    Origen = "API_TEST_INTEGRATION"
                }
            };

            var json = JsonConvert.SerializeObject(datosCrudos);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/ingestion/datos-crudos", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.Equal(2, (int)resultado.datosIngresados);
        }

        [Fact]
        public async Task IngresarDatosCrudos_ConDatosInvalidos_DebeRetornarBadRequest()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var datosInvalidos = new[]
            {
                new DatoCrudoDto
                {
                    Contenido = "", // Contenido vacío - inválido
                    Timestamp = DateTime.Now.AddDays(1), // Fecha futura - inválida
                    Origen = "" // Origen vacío - inválido
                }
            };

            var json = JsonConvert.SerializeObject(datosInvalidos);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/ingestion/datos-crudos", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ConsultarEstadoETL_ConProcesoEjecutandose_DebeRetornarEstado()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Primero iniciamos un proceso ETL
            await _client.PostAsync("/api/ingestion/ejecutar-etl", null);
            
            // Act - Consultar estado inmediatamente
            var response = await _client.GetAsync("/api/ingestion/estado-etl");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var estado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(estado);
            Assert.NotNull((string)estado.estadoActual);
            Assert.NotNull((DateTime)estado.ultimaEjecucion);
        }

        [Fact]
        public async Task ProcesarArchivoCSV_ConArchivoValido_DebeIngresarDatos()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Crear contenido CSV válido
            var csvContent = "ID,Nombre,Precio,Categoria\n" +
                           "PROD002,Mouse Logitech,45.99,Accesorios\n" +
                           "PROD003,Teclado Corsair,89.50,Accesorios";

            var content = new MultipartFormDataContent();
            var fileContent = new StringContent(csvContent);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            content.Add(fileContent, "archivo", "productos_test.csv");

            // Act
            var response = await _client.PostAsync("/api/ingestion/procesar-csv", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.Equal(2, (int)resultado.registrosProcesados);
            Assert.True((bool)resultado.success);
        }

        [Fact]
        public async Task ProcesarArchivoCSV_ConArchivoInvalido_DebeRetornarError()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // CSV malformado
            var csvInvalido = "ID,Nombre,Precio\n" +
                            "PROD004,Mouse,"; // Falta precio

            var content = new MultipartFormDataContent();
            var fileContent = new StringContent(csvInvalido);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            content.Add(fileContent, "archivo", "productos_invalido.csv");

            // Act
            var response = await _client.PostAsync("/api/ingestion/procesar-csv", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ObtenerLogIngestion_ConFechas_DebeRetornarLogs()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var fechaInicio = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            var fechaFin = DateTime.Now.ToString("yyyy-MM-dd");

            // Act
            var response = await _client.GetAsync($"/api/ingestion/logs?fechaInicio={fechaInicio}&fechaFin={fechaFin}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var logs = JsonConvert.DeserializeObject<List<IngestionLogDto>>(responseContent);
            
            Assert.NotNull(logs);
            // Los logs pueden estar vacíos si no hay datos, pero la respuesta debe ser válida
        }

        [Fact]
        public async Task ValidarFormatosDatos_ConMultiplesFormatos_DebeValidarCorrectamente()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var formatosTest = new
            {
                datosJSON = JsonConvert.SerializeObject(new { id = 1, nombre = "Test JSON" }),
                datosXML = "<producto><id>1</id><nombre>Test XML</nombre></producto>",
                datosCSV = "ID,Nombre\n1,Test CSV"
            };

            var json = JsonConvert.SerializeObject(formatosTest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/ingestion/validar-formatos", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.True((bool)resultado.jsonValido);
            Assert.True((bool)resultado.xmlValido);
            Assert.True((bool)resultado.csvValido);
        }

        [Fact]
        public async Task ConfigurarOrigenDatos_ConNuevaFuente_DebeCrearConfiguracion()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var nuevaFuente = new FuenteDeDatosDto
            {
                Nombre = "API_SUCURSAL_TEST",
                Tipo = "API",
                Configuracion = JsonConvert.SerializeObject(new
                {
                    url = "https://api.sucursal-test.com",
                    token = "test-token",
                    frecuencia = "diaria"
                }),
                Activa = true
            };

            var json = JsonConvert.SerializeObject(nuevaFuente);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/ingestion/configurar-fuente", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<FuenteDeDatosDto>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.Equal("API_SUCURSAL_TEST", resultado.Nombre);
            Assert.True(resultado.Activa);
        }

        [Fact]
        public async Task ProcesarDatosCorruptos_ConManejodeErrores_DebeManejartGraciosamente()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var datosCorruptos = new[]
            {
                new DatoCrudoDto
                {
                    Contenido = "{ json malformado sin cerrar",
                    Timestamp = DateTime.Now,
                    Origen = "API_TEST_CORRUPTO"
                },
                new DatoCrudoDto
                {
                    Contenido = null, // Contenido nulo
                    Timestamp = DateTime.Now,
                    Origen = "API_TEST_CORRUPTO"
                }
            };

            var json = JsonConvert.SerializeObject(datosCorruptos);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/ingestion/datos-crudos", content);

            // Assert
            // Debe retornar OK pero con errores registrados
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.True((int)resultado.errores > 0);
            Assert.Contains("datos corruptos procesados", (string)resultado.message);
        }

        [Fact]
        public async Task MonitorearProgresoETL_ConProcesoLargo_DebeRetornarProgreso()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Iniciar proceso ETL
            var responseInicio = await _client.PostAsync("/api/ingestion/ejecutar-etl", null);
            Assert.Equal(HttpStatusCode.OK, responseInicio.StatusCode);

            // Act - Monitorear progreso
            var response = await _client.GetAsync("/api/ingestion/progreso-etl");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var progreso = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(progreso);
            Assert.NotNull((string)progreso.etapa);
            Assert.True((double)progreso.porcentaje >= 0);
            Assert.True((double)progreso.porcentaje <= 100);
        }

        [Fact]
        public async Task ReiniciarProcesosETL_ConProcesosDetenidos_DebeReiniciarCorrectamente()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.PostAsync("/api/ingestion/reiniciar-procesos", null);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.True((bool)resultado.success);
            Assert.Contains("Procesos ETL reiniciados", (string)resultado.message);
        }

        [Fact]
        public async Task ObtenerMetricasIngestion_ConDatosTiempoReal_DebeRetornarMetricas()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/api/ingestion/metricas");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var metricas = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(metricas);
            Assert.NotNull((int)metricas.registrosProcesadosHoy);
            Assert.NotNull((int)metricas.erroresHoy);
            Assert.NotNull((double)metricas.tasaExito);
            Assert.NotNull((TimeSpan)metricas.tiempoPromedioETL);
        }

        [Fact]
        public async Task TestIntegracionCompleta_ETLToReporte_DebeCompletarFlujoCompleto()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Paso 1: Ingresar datos crudos
            var datosCrudos = new[]
            {
                new DatoCrudoDto
                {
                    Contenido = JsonConvert.SerializeObject(new
                    {
                        venta_id = "V_INTEGRATION_001",
                        cliente_id = 100,
                        producto_id = 200,
                        cantidad = 2,
                        precio = 500.00,
                        fecha = DateTime.Now.AddHours(-1).ToString("yyyy-MM-ddTHH:mm:ss")
                    }),
                    Timestamp = DateTime.Now,
                    Origen = "API_INTEGRATION_TEST"
                }
            };

            var jsonDatos = JsonConvert.SerializeObject(datosCrudos);
            var contentDatos = new StringContent(jsonDatos, Encoding.UTF8, "application/json");

            var responseIngestion = await _client.PostAsync("/api/ingestion/datos-crudos", contentDatos);
            Assert.Equal(HttpStatusCode.Created, responseIngestion.StatusCode);

            // Paso 2: Ejecutar ETL
            var responseETL = await _client.PostAsync("/api/ingestion/ejecutar-etl", null);
            Assert.Equal(HttpStatusCode.OK, responseETL.StatusCode);

            // Esperar un momento para que complete el procesamiento
            await Task.Delay(3000);

            // Paso 3: Generar reporte con los datos procesados
            var requestReporte = new
            {
                FechaInicio = DateTime.Now.AddDays(-1),
                FechaFin = DateTime.Now.AddMinutes(10),
                TipoReporte = "Ventas"
            };

            var jsonReporte = JsonConvert.SerializeObject(requestReporte);
            var contentReporte = new StringContent(jsonReporte, Encoding.UTF8, "application/json");

            var responseReporte = await _client.PostAsync("/api/reports/generar", contentReporte);

            // Assert - Todo el flujo debe completarse exitosamente
            Assert.Equal(HttpStatusCode.OK, responseReporte.StatusCode);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                         responseReporte.Content.Headers.ContentType?.MediaType);
        }
    }
} 