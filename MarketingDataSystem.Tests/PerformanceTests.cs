using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Testing;
using MarketingDataSystem.API;
using MarketingDataSystem.Core.DTOs;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace MarketingDataSystem.Tests
{
    [Collection("Performance Tests")]
    public class PerformanceTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public PerformanceTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GenerarReporte_ConMilesDeDatos_DebeCompletarEnTiempoRazonable()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var requestReporte = new
            {
                FechaInicio = DateTime.Now.AddYears(-1), // Período amplio para generar más datos
                FechaFin = DateTime.Now,
                TipoReporte = "Ventas"
            };

            var json = JsonConvert.SerializeObject(requestReporte);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await _client.PostAsync("/api/reports/generar", content);

            // Assert
            stopwatch.Stop();
            
            Assert.True(response.IsSuccessStatusCode);
            Assert.True(stopwatch.ElapsedMilliseconds < 30000, $"Reporte tardó {stopwatch.ElapsedMilliseconds}ms - debe completarse en menos de 30 segundos");
            
            // Verificar que el archivo tiene contenido razonable
            var contentLength = response.Content.Headers.ContentLength;
            Assert.True(contentLength > 1024, "El reporte debe tener al menos 1KB de contenido");
        }

        [Fact]
        public async Task ProcesarETL_ConVolumenAlto_DebeMantenertRendimiento()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Generar 1000 registros de datos crudos
            var datosCrudos = GenerarDatosCrudosVolumen(1000);
            var json = JsonConvert.SerializeObject(datosCrudos);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var stopwatch = Stopwatch.StartNew();

            // Act
            var responseIngestion = await _client.PostAsync("/api/ingestion/datos-crudos", content);
            Assert.True(responseIngestion.IsSuccessStatusCode);

            var responseETL = await _client.PostAsync("/api/ingestion/ejecutar-etl", null);
            
            // Assert
            stopwatch.Stop();
            
            Assert.True(responseETL.IsSuccessStatusCode);
            Assert.True(stopwatch.ElapsedMilliseconds < 60000, $"ETL de 1000 registros tardó {stopwatch.ElapsedMilliseconds}ms - debe completarse en menos de 60 segundos");
            
            // Verificar throughput
            var registrosPorSegundo = 1000.0 / (stopwatch.ElapsedMilliseconds / 1000.0);
            Assert.True(registrosPorSegundo > 50, $"Throughput: {registrosPorSegundo:F2} registros/segundo - debe ser mayor a 50 reg/seg");
        }

        [Fact]
        public async Task ConcurrenciaUsuarios_ConMultiplesRequests_DebeMantenertEstabilidad()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            var numeroUsuariosConcurrentes = 20;
            var requestsPorUsuario = 10;

            var tasks = new List<Task<bool>>();

            // Act - Simular múltiples usuarios concurrentes
            for (int usuario = 0; usuario < numeroUsuariosConcurrentes; usuario++)
            {
                tasks.Add(SimularUsuarioConcurrente(token, requestsPorUsuario, usuario));
            }

            var stopwatch = Stopwatch.StartNew();
            var resultados = await Task.WhenAll(tasks);
            stopwatch.Stop();

            // Assert
            var solicitudesExitosas = resultados.Count(r => r);
            var totalSolicitudes = numeroUsuariosConcurrentes * requestsPorUsuario;
            var tasaExito = (double)solicitudesExitosas / totalSolicitudes * 100;

            Assert.True(tasaExito >= 95, $"Tasa de éxito: {tasaExito:F2}% - debe ser mayor al 95%");
            Assert.True(stopwatch.ElapsedMilliseconds < 120000, $"Tiempo total: {stopwatch.ElapsedMilliseconds}ms - debe completarse en menos de 2 minutos");
            
            var requestsPorSegundo = totalSolicitudes / (stopwatch.ElapsedMilliseconds / 1000.0);
            Assert.True(requestsPorSegundo > 5, $"Throughput: {requestsPorSegundo:F2} requests/segundo - debe ser mayor a 5 req/seg");
        }

        [Fact]
        public async Task ConsultasBaseDatos_ConVolumenAlto_DebeMantenertRendimiento()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var numeroConsultas = 100;
            var tiemposRespuesta = new List<long>();

            // Act - Realizar múltiples consultas
            for (int i = 0; i < numeroConsultas; i++)
            {
                var stopwatch = Stopwatch.StartNew();
                
                var response = await _client.GetAsync("/api/clientes");
                
                stopwatch.Stop();
                tiemposRespuesta.Add(stopwatch.ElapsedMilliseconds);
                
                Assert.True(response.IsSuccessStatusCode);
            }

            // Assert
            var tiempoPromedio = tiemposRespuesta.Average();
            var tiempoMaximo = tiemposRespuesta.Max();
            var tiempoP95 = tiemposRespuesta.OrderBy(t => t).Skip((int)(numeroConsultas * 0.95)).First();

            Assert.True(tiempoPromedio < 1000, $"Tiempo promedio: {tiempoPromedio:F2}ms - debe ser menor a 1 segundo");
            Assert.True(tiempoMaximo < 5000, $"Tiempo máximo: {tiempoMaximo}ms - debe ser menor a 5 segundos");
            Assert.True(tiempoP95 < 2000, $"Percentil 95: {tiempoP95}ms - debe ser menor a 2 segundos");
        }

        [Fact]
        public async Task MemoryUsage_ConOperacionesIntensivas_NoDebeExcederLimites()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var memoriaInicial = GC.GetTotalMemory(true);

            // Act - Realizar operaciones que consumen memoria
            var tasks = new List<Task>();
            
            for (int i = 0; i < 50; i++)
            {
                tasks.Add(RealizarOperacionIntensiva(token, i));
            }

            await Task.WhenAll(tasks);

            // Forzar garbage collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var memoriaFinal = GC.GetTotalMemory(true);

            // Assert
            var incrementoMemoria = memoriaFinal - memoriaInicial;
            var incrementoMB = incrementoMemoria / (1024.0 * 1024.0);

            Assert.True(incrementoMB < 500, $"Incremento de memoria: {incrementoMB:F2}MB - debe ser menor a 500MB");
        }

        [Fact]
        public async Task StressTest_ConCargaExtendida_DebeMantenertEstabilidad()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            var duracionTestMinutos = 2; // Test de 2 minutos
            var intervaloSegundos = 1;
            
            var finTest = DateTime.Now.AddMinutes(duracionTestMinutos);
            var errores = 0;
            var solicitudesTotal = 0;
            var tiemposRespuesta = new List<long>();

            // Act - Mantener carga constante
            while (DateTime.Now < finTest)
            {
                var stopwatch = Stopwatch.StartNew();
                
                try
                {
                    var response = await _client.GetAsync("/api/admin/estadisticas-sistema");
                    stopwatch.Stop();
                    
                    if (response.IsSuccessStatusCode)
                    {
                        tiemposRespuesta.Add(stopwatch.ElapsedMilliseconds);
                    }
                    else
                    {
                        errores++;
                    }
                }
                catch
                {
                    errores++;
                    stopwatch.Stop();
                }
                
                solicitudesTotal++;
                await Task.Delay(TimeSpan.FromSeconds(intervaloSegundos));
            }

            // Assert
            var tasaError = (double)errores / solicitudesTotal * 100;
            var tiempoPromedioRespuesta = tiemposRespuesta.Any() ? tiemposRespuesta.Average() : 0;

            Assert.True(tasaError < 5, $"Tasa de error: {tasaError:F2}% - debe ser menor al 5%");
            Assert.True(tiempoPromedioRespuesta < 2000, $"Tiempo promedio: {tiempoPromedioRespuesta:F2}ms - debe ser menor a 2 segundos");
            Assert.True(solicitudesTotal > duracionTestMinutos * 50, $"Solicitudes procesadas: {solicitudesTotal} - debe procesar al menos 50 por minuto");
        }

        [Fact]
        public async Task DatabaseConnection_ConPoolingConnections_DebeGestionarCorrectamente()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var numeroConexionesSimultaneas = 50;
            var tasks = new List<Task<bool>>();

            // Act - Simular múltiples conexiones simultáneas
            for (int i = 0; i < numeroConexionesSimultaneas; i++)
            {
                tasks.Add(TestearConexionBaseDatos(token, i));
            }

            var stopwatch = Stopwatch.StartNew();
            var resultados = await Task.WhenAll(tasks);
            stopwatch.Stop();

            // Assert
            var conexionesExitosas = resultados.Count(r => r);
            var tasaExitoConexiones = (double)conexionesExitosas / numeroConexionesSimultaneas * 100;

            Assert.True(tasaExitoConexiones >= 98, $"Tasa éxito conexiones: {tasaExitoConexiones:F2}% - debe ser mayor al 98%");
            Assert.True(stopwatch.ElapsedMilliseconds < 30000, $"Tiempo total conexiones: {stopwatch.ElapsedMilliseconds}ms - debe ser menor a 30 segundos");
        }

        [Fact]
        public async Task DataProcessing_ConTransformacionesComplejas_DebeOptimizarRendimiento()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Generar datos complejos para transformación
            var datosComplejos = GenerarDatosComplejosTransformacion(500);
            var json = JsonConvert.SerializeObject(datosComplejos);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await _client.PostAsync("/api/ingestion/datos-crudos", content);

            // Assert
            stopwatch.Stop();
            
            Assert.True(response.IsSuccessStatusCode);
            Assert.True(stopwatch.ElapsedMilliseconds < 45000, $"Transformación compleja tardó {stopwatch.ElapsedMilliseconds}ms - debe ser menor a 45 segundos");
            
            var throughputComplejo = 500.0 / (stopwatch.ElapsedMilliseconds / 1000.0);
            Assert.True(throughputComplejo > 20, $"Throughput transformación compleja: {throughputComplejo:F2} reg/seg - debe ser mayor a 20 reg/seg");
        }

        // Métodos auxiliares para los tests de rendimiento

        private async Task<string> ObtenerTokenAdminAsync()
        {
            var loginRequest = new
            {
                Username = "admin",
                Password = "Admin123!"
            };

            var json = JsonConvert.SerializeObject(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/auth/login", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

            return authResponse.token;
        }

        private async Task<bool> SimularUsuarioConcurrente(string token, int requestsPorUsuario, int usuarioId)
        {
            try
            {
                var clienteUsuario = _factory.CreateClient();
                clienteUsuario.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var solicitudesExitosas = 0;

                for (int i = 0; i < requestsPorUsuario; i++)
                {
                    var endpoint = ElegirEndpointAleatorio();
                    var response = await clienteUsuario.GetAsync(endpoint);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        solicitudesExitosas++;
                    }

                    // Pequeña pausa para simular comportamiento real
                    await Task.Delay(Random.Shared.Next(50, 200));
                }

                return solicitudesExitosas >= requestsPorUsuario * 0.9; // 90% de éxito mínimo
            }
            catch
            {
                return false;
            }
        }

        private async Task RealizarOperacionIntensiva(string token, int operacionId)
        {
            var cliente = _factory.CreateClient();
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Realizar operaciones que consumen memoria
            var tasks = new List<Task>();
            
            tasks.Add(cliente.GetAsync("/api/clientes"));
            tasks.Add(cliente.GetAsync("/api/productos"));
            tasks.Add(cliente.GetAsync("/api/ventas"));
            tasks.Add(cliente.GetAsync("/api/reports"));

            await Task.WhenAll(tasks);
        }

        private async Task<bool> TestearConexionBaseDatos(string token, int conexionId)
        {
            try
            {
                var cliente = _factory.CreateClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await cliente.GetAsync("/api/admin/estadisticas-sistema");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        private string ElegirEndpointAleatorio()
        {
            var endpoints = new[]
            {
                "/api/clientes",
                "/api/productos", 
                "/api/ventas",
                "/api/stock/consultar",
                "/api/reports",
                "/api/admin/estadisticas-sistema"
            };

            return endpoints[Random.Shared.Next(endpoints.Length)];
        }

        private List<DatoCrudoDto> GenerarDatosCrudosVolumen(int cantidad)
        {
            var datos = new List<DatoCrudoDto>();

            for (int i = 1; i <= cantidad; i++)
            {
                datos.Add(new DatoCrudoDto
                {
                    Contenido = JsonConvert.SerializeObject(new
                    {
                        id = $"PERF_TEST_{i:D6}",
                        tipo = i % 3 == 0 ? "Cliente" : i % 3 == 1 ? "Producto" : "Venta",
                        nombre = $"Elemento Performance Test {i}",
                        valor = Random.Shared.NextDouble() * 1000,
                        fecha = DateTime.Now.AddMinutes(-Random.Shared.Next(10080)), // Última semana
                        categoria = $"Categoria_{i % 10}",
                        atributos = GenerarAtributosAleatorios()
                    }),
                    Timestamp = DateTime.Now.AddSeconds(-Random.Shared.Next(3600)),
                    Origen = $"API_PERFORMANCE_TEST_{i % 5 + 1}"
                });
            }

            return datos;
        }

        private List<DatoCrudoDto> GenerarDatosComplejosTransformacion(int cantidad)
        {
            var datos = new List<DatoCrudoDto>();

            for (int i = 1; i <= cantidad; i++)
            {
                datos.Add(new DatoCrudoDto
                {
                    Contenido = JsonConvert.SerializeObject(new
                    {
                        id = $"COMPLEX_{i:D4}",
                        estructura_anidada = new
                        {
                            nivel1 = new
                            {
                                nivel2 = new
                                {
                                    datos = Enumerable.Range(1, 10).Select(x => new { id = x, valor = Random.Shared.NextDouble() * 100 }),
                                    metadatos = GenerarMetadatosComplejos()
                                }
                            }
                        },
                        arrays_grandes = Enumerable.Range(1, 50).Select(x => $"Item_{x}").ToArray(),
                        fechas_multiples = new
                        {
                            creacion = DateTime.Now.AddDays(-Random.Shared.Next(365)),
                            modificacion = DateTime.Now.AddHours(-Random.Shared.Next(24)),
                            expiracion = DateTime.Now.AddDays(Random.Shared.Next(30))
                        }
                    }),
                    Timestamp = DateTime.Now,
                    Origen = "API_COMPLEX_TRANSFORM"
                });
            }

            return datos;
        }

        private object GenerarAtributosAleatorios()
        {
            return new
            {
                color = new[] { "Rojo", "Azul", "Verde", "Amarillo" }[Random.Shared.Next(4)],
                tamaño = new[] { "Pequeño", "Mediano", "Grande" }[Random.Shared.Next(3)],
                activo = Random.Shared.Next(2) == 1,
                prioridad = Random.Shared.Next(1, 11),
                tags = Enumerable.Range(1, Random.Shared.Next(1, 6)).Select(x => $"tag_{x}").ToArray()
            };
        }

        private object GenerarMetadatosComplejos()
        {
            return new
            {
                configuracion = Enumerable.Range(1, 20).ToDictionary(x => $"config_{x}", x => Random.Shared.NextDouble()),
                relaciones = Enumerable.Range(1, 10).Select(x => new { tipo = $"relacion_{x}", peso = Random.Shared.NextDouble() }),
                validaciones = new
                {
                    reglas = Enumerable.Range(1, 15).Select(x => $"regla_{x}").ToArray(),
                    resultado = Random.Shared.Next(2) == 1
                }
            };
        }
    }
} 