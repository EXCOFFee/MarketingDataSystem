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
    public class StockControllerIntegrationTests : BaseApiTest
    {
        public StockControllerIntegrationTests(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task ActualizarStock_ConDatosValidos_DebeActualizar()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var stockUpdate = new StockDto
            {
                ProductoId = 1,
                Cantidad = 150,
                StockMinimo = 20,
                StockMaximo = 500,
                Ubicacion = "Almacén Central - A1",
                FechaActualizacion = DateTime.Now
            };

            var json = JsonConvert.SerializeObject(stockUpdate);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/stock/actualizar", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<StockDto>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.Equal(150, resultado.Cantidad);
            Assert.Equal("Almacén Central - A1", resultado.Ubicacion);
        }

        [Fact]
        public async Task ActualizarStock_ConCantidadNegativa_DebeRetornarBadRequest()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var stockInvalido = new StockDto
            {
                ProductoId = 1,
                Cantidad = -10, // Cantidad negativa - inválida
                StockMinimo = 20,
                StockMaximo = 500
            };

            var json = JsonConvert.SerializeObject(stockInvalido);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/stock/actualizar", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ConsultarStock_ConFiltros_DebeRetornarResultadosFiltrados()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Parámetros de filtro
            var filtros = new
            {
                stockMinimoBajo = true,
                categoria = "Electrónicos",
                ubicacion = "Almacén Central"
            };

            var queryString = $"?stockMinimoBajo={filtros.stockMinimoBajo}" +
                            $"&categoria={filtros.categoria}" +
                            $"&ubicacion={Uri.EscapeDataString(filtros.ubicacion)}";

            // Act
            var response = await _client.GetAsync($"/api/stock/consultar{queryString}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var stocks = JsonConvert.DeserializeObject<List<StockDto>>(responseContent);
            
            Assert.NotNull(stocks);
            // Verificar que los resultados cumplen con los filtros aplicados
            if (stocks.Any())
            {
                Assert.All(stocks, s => Assert.True(s.Cantidad <= s.StockMinimo || s.StockMinimo == 0));
            }
        }

        [Fact]
        public async Task ObtenerStockPorProducto_ConProductoExistente_DebeRetornarStock()
        {
            // Arrange  
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var productoId = 1;

            // Act
            var response = await _client.GetAsync($"/api/stock/producto/{productoId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var stock = JsonConvert.DeserializeObject<StockDto>(responseContent);
            
            Assert.NotNull(stock);
            Assert.Equal(productoId, stock.ProductoId);
        }

        [Fact]
        public async Task ObtenerStockPorProducto_ConProductoInexistente_DebeRetornarNotFound()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var productoIdInexistente = 99999;

            // Act
            var response = await _client.GetAsync($"/api/stock/producto/{productoIdInexistente}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GenerarAlertasStock_ConStockBajo_DebeGenerarAlertas()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.PostAsync("/api/stock/generar-alertas", null);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.NotNull((int)resultado.alertasGeneradas);
            Assert.True((int)resultado.alertasGeneradas >= 0);
        }

        [Fact]
        public async Task MovimientoStock_ConMovimientoValido_DebeRegistrarMovimiento()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var movimiento = new
            {
                ProductoId = 1,
                TipoMovimiento = "Entrada", // Entrada, Salida, Ajuste
                Cantidad = 50,
                Motivo = "Reposición de inventario",
                Origen = "Proveedor XYZ",
                Destino = "Almacén Central - A1",
                Referencia = "OC-2023-001"
            };

            var json = JsonConvert.SerializeObject(movimiento);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/stock/movimiento", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.Equal(1, (int)resultado.productoId);
            Assert.Equal(50, (int)resultado.cantidad);
            Assert.Equal("Entrada", (string)resultado.tipoMovimiento);
        }

        [Fact]
        public async Task MovimientoStock_ConCantidadInsuficiente_DebeRetornarBadRequest()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var movimientoInvalido = new
            {
                ProductoId = 1,
                TipoMovimiento = "Salida",
                Cantidad = 99999, // Cantidad muy alta que excede el stock disponible
                Motivo = "Venta"
            };

            var json = JsonConvert.SerializeObject(movimientoInvalido);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/stock/movimiento", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("stock insuficiente", responseContent.ToLower());
        }

        [Fact]
        public async Task HistorialMovimientos_ConFechas_DebeRetornarHistorial()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var fechaInicio = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            var fechaFin = DateTime.Now.ToString("yyyy-MM-dd");
            var productoId = 1;

            // Act
            var response = await _client.GetAsync($"/api/stock/historial/{productoId}?fechaInicio={fechaInicio}&fechaFin={fechaFin}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var movimientos = JsonConvert.DeserializeObject<List<dynamic>>(responseContent);
            
            Assert.NotNull(movimientos);
            // El historial puede estar vacío si no hay movimientos, pero debe ser una lista válida
        }

        [Fact]
        public async Task InventarioFisico_ConRecuento_DebeActualizarStock()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var inventarioFisico = new[]
            {
                new
                {
                    ProductoId = 1,
                    CantidadFisica = 145,
                    Ubicacion = "Almacén Central - A1",
                    FechaRecuento = DateTime.Now,
                    ResponsableRecuento = "Juan Pérez"
                },
                new
                {
                    ProductoId = 2,
                    CantidadFisica = 89,
                    Ubicacion = "Almacén Central - B2",
                    FechaRecuento = DateTime.Now,
                    ResponsableRecuento = "María García"
                }
            };

            var json = JsonConvert.SerializeObject(inventarioFisico);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/stock/inventario-fisico", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.Equal(2, (int)resultado.productosActualizados);
            Assert.True((int)resultado.ajustesRealizados >= 0);
        }

        [Fact]
        public async Task ReservarStock_ConReservaValida_DebeReservarCorrectamente()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var reserva = new
            {
                ProductoId = 1,
                CantidadReservada = 10,
                MotivoReserva = "Pedido cliente #12345",
                FechaVencimientoReserva = DateTime.Now.AddDays(7),
                ClienteId = 1
            };

            var json = JsonConvert.SerializeObject(reserva);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/stock/reservar", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.Equal(1, (int)resultado.productoId);
            Assert.Equal(10, (int)resultado.cantidadReservada);
            Assert.NotNull((string)resultado.codigoReserva);
        }

        [Fact]
        public async Task LiberarReserva_ConCodigoValido_DebeLiberarStock()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Primero crear una reserva
            var reserva = new
            {
                ProductoId = 1,
                CantidadReservada = 5,
                MotivoReserva = "Test liberación"
            };

            var jsonReserva = JsonConvert.SerializeObject(reserva);
            var contentReserva = new StringContent(jsonReserva, Encoding.UTF8, "application/json");
            
            var responseReserva = await _client.PostAsync("/api/stock/reservar", contentReserva);
            var reservaCreada = JsonConvert.DeserializeObject<dynamic>(await responseReserva.Content.ReadAsStringAsync());
            var codigoReserva = (string)reservaCreada.codigoReserva;

            // Act - Liberar la reserva
            var response = await _client.DeleteAsync($"/api/stock/liberar-reserva/{codigoReserva}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.True((bool)resultado.reservaLiberada);
            Assert.Equal(5, (int)resultado.cantidadLiberada);
        }

        [Fact]
        public async Task GenerarReporteStock_ConParametros_DebeGenerarReporte()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var parametrosReporte = new
            {
                TipoReporte = "StockGeneral", // StockGeneral, StockBajo, MovimientosStock
                FechaInicio = DateTime.Now.AddDays(-30),
                FechaFin = DateTime.Now,
                IncluirMovimientos = true,
                AgruparPorCategoria = true
            };

            var json = JsonConvert.SerializeObject(parametrosReporte);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/stock/generar-reporte", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                         response.Content.Headers.ContentType?.MediaType);
            
            var contentLength = response.Content.Headers.ContentLength;
            Assert.True(contentLength > 0, "El reporte debe tener contenido");
        }

        [Fact]
        public async Task ConfigurarAlertasStock_ConParametros_DebeConfigurarAlertas()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var configuracionAlertas = new
            {
                ProductoId = 1,
                StockMinimoAlerta = 15,
                StockMaximoAlerta = 1000,
                EmailNotificacion = "admin@empresa.com",
                AlertaActiva = true,
                FrecuenciaRevision = "Diaria" // Diaria, Semanal, Mensual
            };

            var json = JsonConvert.SerializeObject(configuracionAlertas);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/stock/configurar-alertas", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.True((bool)resultado.configuracionGuardada);
            Assert.Equal(1, (int)resultado.productoId);
        }

        [Fact]
        public async Task TransferirStock_EntreUbicaciones_DebeTransferirCorrectamente()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var transferencia = new
            {
                ProductoId = 1,
                Cantidad = 25,
                UbicacionOrigen = "Almacén Central - A1",
                UbicacionDestino = "Almacén Central - B1",
                MotivoTransferencia = "Reorganización de inventario",
                ResponsableTransferencia = "Carlos López"
            };

            var json = JsonConvert.SerializeObject(transferencia);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/stock/transferir", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.True((bool)resultado.transferenciaExitosa);
            Assert.Equal(25, (int)resultado.cantidadTransferida);
            Assert.NotNull((string)resultado.codigoTransferencia);
        }

        [Fact]
        public async Task ObtenerMetricasStock_ConDashboard_DebeRetornarMetricas()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/api/stock/metricas-dashboard");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var metricas = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(metricas);
            Assert.NotNull((int)metricas.totalProductos);
            Assert.NotNull((int)metricas.productosStockBajo);
            Assert.NotNull((decimal)metricas.valorTotalInventario);
            Assert.NotNull((int)metricas.movimientosHoy);
            Assert.NotNull((double)metricas.rotacionPromedio);
        }

        [Fact]
        public async Task ValidarConsistenciaStock_ConAuditoria_DebeDetectarInconsistencias()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.PostAsync("/api/stock/validar-consistencia", null);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.NotNull((int)resultado.productosValidados);
            Assert.NotNull((int)resultado.inconsistenciasDetectadas);
            Assert.NotNull((bool)resultado.auditoriaCompleta);
            
            if ((int)resultado.inconsistenciasDetectadas > 0)
            {
                Assert.NotNull(resultado.detalleInconsistencias);
            }
        }

        [Fact]
        public async Task SincronizarStockExterno_ConSistemasExternos_DebeSincronizar()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var configuracionSincronizacion = new
            {
                SistemaExterno = "ERP_PRINCIPAL",
                FuerzaSincronizacion = false, // Solo sincronizar cambios
                ProductosEspecificos = new[] { 1, 2, 3 }, // Opcional: productos específicos
                IncluirPrecios = true
            };

            var json = JsonConvert.SerializeObject(configuracionSincronizacion);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/stock/sincronizar-externo", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.True((bool)resultado.sincronizacionExitosa);
            Assert.NotNull((int)resultado.registrosSincronizados);
            Assert.NotNull((string)resultado.horaUltimaSincronizacion);
        }
    }
} 