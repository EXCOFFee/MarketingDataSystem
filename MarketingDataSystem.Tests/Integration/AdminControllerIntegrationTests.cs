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
    public class AdminControllerIntegrationTests : BaseApiTest
    {
        public AdminControllerIntegrationTests(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task GestionarUsuarios_ConPermisos_DebePermitirAcceso()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var nuevoUsuario = new CreateUserDto
            {
                Username = "usuario_admin_test",
                Password = "Password123!",
                Email = "admin.test@empresa.com",
                Role = "Usuario"
            };

            var json = JsonConvert.SerializeObject(nuevoUsuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/admin/usuarios", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var usuarioCreado = JsonConvert.DeserializeObject<UserDto>(responseContent);
            
            Assert.NotNull(usuarioCreado);
            Assert.Equal("usuario_admin_test", usuarioCreado.Username);
            Assert.Equal("Usuario", usuarioCreado.Role);
        }

        [Fact]
        public async Task GestionarUsuarios_SinPermisos_DebeRetornarForbidden()
        {
            // Arrange
            var token = await ObtenerTokenUsuarioAsync(); // Token de usuario regular
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var nuevoUsuario = new CreateUserDto
            {
                Username = "usuario_sin_permisos",
                Password = "Password123!",
                Email = "sinpermisos@empresa.com"
            };

            var json = JsonConvert.SerializeObject(nuevoUsuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/admin/usuarios", content);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task ConfigurarSistema_ConParametros_DebeAplicarCambios()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var configuracionSistema = new
            {
                ConfiguracionETL = new
                {
                    FrecuenciaEjecucion = "Diaria",
                    HoraEjecucion = "02:00",
                    TimeoutProcesamiento = 3600, // 1 hora en segundos
                    ReintentosFalla = 3
                },
                ConfiguracionBackup = new
                {
                    BackupAutomatico = true,
                    FrecuenciaBackup = "Diaria",
                    HoraBackup = "01:00",
                    MantenertBackups = 30
                },
                ConfiguracionAlertas = new
                {
                    EmailAlertas = "admin@empresa.com",
                    AlertasStockBajo = true,
                    AlertasErroresETL = true,
                    AlertasAccesosSospechosos = true
                },
                ConfiguracionSeguridad = new
                {
                    TiempoSesion = 480, // 8 horas en minutos
                    RequiereCambioPassword = true,
                    IntentosMaximosLogin = 5,
                    BloqueoTiempo = 30 // minutos
                }
            };

            var json = JsonConvert.SerializeObject(configuracionSistema);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/admin/configurar-sistema", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.True((bool)resultado.configuracionGuardada);
            Assert.Contains("Sistema configurado exitosamente", (string)resultado.mensaje);
        }

        [Fact]
        public async Task ObtenerEstadisticasSistema_ConMetricas_DebeRetornarDashboard()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/api/admin/estadisticas-sistema");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var estadisticas = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(estadisticas);
            
            // Verificar métricas del sistema
            Assert.NotNull((int)estadisticas.totalUsuarios);
            Assert.NotNull((int)estadisticas.usuariosActivos);
            Assert.NotNull((int)estadisticas.sesionesActivas);
            
            // Verificar métricas de datos
            Assert.NotNull((int)estadisticas.totalClientes);
            Assert.NotNull((int)estadisticas.totalProductos);
            Assert.NotNull((int)estadisticas.totalVentas);
            
            // Verificar métricas de ETL
            Assert.NotNull((string)estadisticas.ultimaEjecucionETL);
            Assert.NotNull((int)estadisticas.registrosProcesadosHoy);
            Assert.NotNull((double)estadisticas.tasaExitoETL);
            
            // Verificar métricas de sistema
            Assert.NotNull((double)estadisticas.usoMemoria);
            Assert.NotNull((double)estadisticas.usoCPU);
            Assert.NotNull((double)estadisticas.espacioDisco);
        }

        [Fact]
        public async Task GestionarRoles_ConNuevoRol_DebeCrearRol()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var nuevoRol = new
            {
                Nombre = "Analista",
                Descripcion = "Rol para analistas de datos",
                Permisos = new[]
                {
                    "Ver.Reportes",
                    "Generar.Reportes",
                    "Ver.Clientes",
                    "Ver.Productos",
                    "Ver.Ventas"
                }
            };

            var json = JsonConvert.SerializeObject(nuevoRol);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/admin/roles", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var rolCreado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(rolCreado);
            Assert.Equal("Analista", (string)rolCreado.nombre);
            Assert.Equal(5, ((dynamic[])rolCreado.permisos).Length);
        }

        [Fact]
        public async Task AsignarRolUsuario_ConRolValido_DebeAsignarCorrectamente()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var asignacionRol = new
            {
                UsuarioId = 2, // Usuario existente
                RolNuevo = "Analista",
                RolAnterior = "Usuario",
                MotivoAsignacion = "Promoción por desempeño"
            };

            var json = JsonConvert.SerializeObject(asignacionRol);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/admin/asignar-rol", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.True((bool)resultado.rolAsignado);
            Assert.Equal("Analista", (string)resultado.nuevoRol);
        }

        [Fact]
        public async Task MonitorearSesionesActivas_ConDetalles_DebeRetornarSesiones()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/api/admin/sesiones-activas");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var sesiones = JsonConvert.DeserializeObject<List<dynamic>>(responseContent);
            
            Assert.NotNull(sesiones);
            
            // Debe haber al menos la sesión actual del admin
            Assert.True(sesiones.Count >= 1);
            
            var sesionActual = sesiones.FirstOrDefault(s => (string)s.usuario == "admin");
            Assert.NotNull(sesionActual);
            Assert.NotNull((string)sesionActual.ipAddress);
            Assert.NotNull((DateTime)sesionActual.inicioSesion);
        }

        [Fact]
        public async Task CerrarSesionUsuario_ConSesionValida_DebeCerrarSesion()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Primero obtener las sesiones activas
            var responseSesiones = await _client.GetAsync("/api/admin/sesiones-activas");
            var sesiones = JsonConvert.DeserializeObject<List<dynamic>>(await responseSesiones.Content.ReadAsStringAsync());
            
            // Buscar una sesión que no sea la del admin actual
            var sesionACerrar = sesiones.FirstOrDefault(s => (string)s.usuario != "admin");
            
            if (sesionACerrar != null)
            {
                var sesionId = (string)sesionACerrar.sesionId;

                // Act
                var response = await _client.DeleteAsync($"/api/admin/cerrar-sesion/{sesionId}");

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                
                var responseContent = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
                
                Assert.NotNull(resultado);
                Assert.True((bool)resultado.sesionCerrada);
            }
        }

        [Fact]
        public async Task GenerarReporteAuditoria_ConFechas_DebeGenerarReporte()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var parametrosAuditoria = new
            {
                FechaInicio = DateTime.Now.AddDays(-30),
                FechaFin = DateTime.Now,
                TipoEvento = "Todos", // Todos, Login, Cambios, Errores
                Usuario = "", // Vacío para todos los usuarios
                IncluirDetalles = true
            };

            var json = JsonConvert.SerializeObject(parametrosAuditoria);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/admin/reporte-auditoria", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                         response.Content.Headers.ContentType?.MediaType);
            
            var contentLength = response.Content.Headers.ContentLength;
            Assert.True(contentLength > 0, "El reporte de auditoría debe tener contenido");
        }

        [Fact]
        public async Task MantenimientoSistema_ConLimpiezaDatos_DebeEjecutarMantenimiento()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var configuracionMantenimiento = new
            {
                LimpiarLogsAntiguos = true,
                DiasRetencionLogs = 90,
                LimpiarSesionesExpiradas = true,
                OptimizarBaseDatos = true,
                LimpiarArchivosTemporales = true,
                CompactarBackups = true
            };

            var json = JsonConvert.SerializeObject(configuracionMantenimiento);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/admin/mantenimiento-sistema", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.True((bool)resultado.mantenimientoCompletado);
            Assert.NotNull((int)resultado.logsEliminados);
            Assert.NotNull((int)resultado.sesionesLimpiadas);
            Assert.NotNull((string)resultado.tiempoEjecucion);
        }

        [Fact]
        public async Task ConfigurarNotificaciones_ConParametros_DebeGuardarConfiguracion()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var configuracionNotificaciones = new
            {
                Email = new
                {
                    ServidorSMTP = "smtp.empresa.com",
                    Puerto = 587,
                    Usuario = "sistema@empresa.com",
                    UsarSSL = true,
                    EmailsDestino = new[] { "admin@empresa.com", "soporte@empresa.com" }
                },
                TiposNotificacion = new
                {
                    ErroresETL = true,
                    StockBajo = true,
                    BackupFallido = true,
                    AccesosSospechosos = true,
                    RendimientoBajo = false
                },
                Frecuencia = new
                {
                    Inmediata = new[] { "ErroresETL", "BackupFallido" },
                    Diaria = new[] { "StockBajo", "RendimientoBajo" },
                    Semanal = new[] { "AccesosSospechosos" }
                }
            };

            var json = JsonConvert.SerializeObject(configuracionNotificaciones);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/admin/configurar-notificaciones", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.True((bool)resultado.configuracionGuardada);
            Assert.Contains("Notificaciones configuradas", (string)resultado.mensaje);
        }

        [Fact]
        public async Task MonitorearRendimientoSistema_ConMetricas_DebeRetornarRendimiento()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/api/admin/rendimiento-sistema");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var rendimiento = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(rendimiento);
            
            // Métricas de CPU y memoria
            Assert.NotNull((double)rendimiento.usoCPU);
            Assert.NotNull((double)rendimiento.usoMemoria);
            Assert.True((double)rendimiento.usoCPU >= 0 && (double)rendimiento.usoCPU <= 100);
            Assert.True((double)rendimiento.usoMemoria >= 0 && (double)rendimiento.usoMemoria <= 100);
            
            // Métricas de base de datos
            Assert.NotNull((int)rendimiento.conexionesActivas);
            Assert.NotNull((double)rendimiento.tiempoRespuestaPromedio);
            
            // Métricas de aplicación
            Assert.NotNull((int)rendimiento.requestsPorMinuto);
            Assert.NotNull((double)rendimiento.tiempoProcesamientoETL);
        }

        [Fact]
        public async Task RestaurarSistema_ConPuntoRestauracion_DebeRestaurar()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var parametrosRestauracion = new
            {
                TipoRestauracion = "ConfiguracionSistema", // ConfiguracionSistema, DatosCompletos, SoloUsuarios
                FechaRestauracion = DateTime.Now.AddDays(-1),
                ConfirmacionRestauracion = "CONFIRMAR_RESTAURACION",
                MantenertDatosActuales = false,
                NotificarUsuarios = true
            };

            var json = JsonConvert.SerializeObject(parametrosRestauracion);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/admin/restaurar-sistema", content);

            // Assert
            // Nota: En un entorno real, esto podría requerir confirmación adicional
            // Para tests, simulamos una respuesta exitosa
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.Contains("restauración", (string)resultado.mensaje);
        }

        [Fact]
        public async Task ExportarConfiguracionCompleta_ConSistema_DebeExportarConfiguracion()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var parametrosExportacion = new
            {
                IncluirUsuarios = true,
                IncluirRoles = true,
                IncluirConfiguracionETL = true,
                IncluirConfiguracionBackup = true,
                IncluirAlertas = true,
                FormatoExportacion = "JSON" // JSON, XML, YAML
            };

            var json = JsonConvert.SerializeObject(parametrosExportacion);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/admin/exportar-configuracion", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var configuracionExportada = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(configuracionExportada);
            Assert.NotNull(configuracionExportada.usuarios);
            Assert.NotNull(configuracionExportada.roles);
            Assert.NotNull(configuracionExportada.configuracionETL);
            Assert.NotNull((string)configuracionExportada.fechaExportacion);
        }

        [Fact]
        public async Task ImportarConfiguracion_ConArchivoValido_DebeImportarConfiguracion()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Crear configuración de prueba para importar
            var configuracionImport = new
            {
                configuracionETL = new
                {
                    frecuenciaEjecucion = "Diaria",
                    horaEjecucion = "03:00"
                },
                alertas = new[]
                {
                    new { tipo = "StockBajo", activa = true },
                    new { tipo = "ErrorETL", activa = true }
                },
                fechaCreacion = DateTime.Now
            };

            var contenidoArchivo = JsonConvert.SerializeObject(configuracionImport);
            var content = new MultipartFormDataContent();
            var fileContent = new StringContent(contenidoArchivo);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Add(fileContent, "archivo", "configuracion.json");
            content.Add(new StringContent("true"), "sobrescribirExistente");

            // Act
            var response = await _client.PostAsync("/api/admin/importar-configuracion", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.True((bool)resultado.importacionExitosa);
            Assert.NotNull((int)resultado.elementosImportados);
        }

        [Fact]
        public async Task ValidarIntegridadSistema_ConVerificacionCompleta_DebeValidarIntegridad()
        {
            // Arrange
            var token = await ObtenerTokenAdminAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var parametrosValidacion = new
            {
                ValidarBaseDatos = true,
                ValidarArchivos = true,
                ValidarConfiguracion = true,
                ValidarPermisos = true,
                GenerarReporte = true
            };

            var json = JsonConvert.SerializeObject(parametrosValidacion);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/admin/validar-integridad", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<dynamic>(responseContent);
            
            Assert.NotNull(resultado);
            Assert.True((bool)resultado.validacionCompleta);
            Assert.NotNull((int)resultado.elementosValidados);
            Assert.NotNull((int)resultado.problemasDetectados);
            
            if ((int)resultado.problemasDetectados > 0)
            {
                Assert.NotNull(resultado.detalleProblemas);
            }
        }
    }
} 