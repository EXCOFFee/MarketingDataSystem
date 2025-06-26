using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MarketingDataSystem.Application.Interfaces;

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Servicio de alertas que notifica fallos críticos del sistema.
    /// Cumple con el principio de responsabilidad única (S de SOLID).
    /// </summary>
    public class AlertaService : IAlertaService
    {
        private readonly ILogger<AlertaService> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        // Configuración desde appsettings.json
        private readonly string? _emailAdministrador;
        private readonly string? _webhookUrl;
        private readonly bool _alertasHabilitadas;

        public AlertaService(
            ILogger<AlertaService> logger,
            IConfiguration configuration,
            HttpClient httpClient)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;

            // Cargar configuración
            _emailAdministrador = _configuration["Alertas:EmailAdministrador"];
            _webhookUrl = _configuration["Alertas:WebhookUrl"];
            _alertasHabilitadas = _configuration.GetValue<bool>("Alertas:Habilitadas", false);
        }

        /// <summary>
        /// Envía alerta crítica cuando falla el ETL después de todos los reintentos
        /// </summary>
        public async Task EnviarAlertaCriticaAsync(string mensaje, string detalleError)
        {
            if (!_alertasHabilitadas)
            {
                _logger.LogWarning("Alertas deshabilitadas. No se enviará alerta crítica.");
                return;
            }

            var alerta = new
            {
                Tipo = "CRITICA",
                Mensaje = mensaje,
                DetalleError = detalleError,
                FechaHora = DateTime.Now,
                Sistema = "Marketing Data System",
                Severidad = "HIGH"
            };

            _logger.LogCritical("🚨 ALERTA CRÍTICA: {Mensaje} - {DetalleError}", mensaje, detalleError);

            // Enviar por webhook si está configurado
            if (!string.IsNullOrEmpty(_webhookUrl))
            {
                await EnviarWebhookAsync(alerta);
            }

            // Enviar por email si está configurado
            if (!string.IsNullOrEmpty(_emailAdministrador))
            {
                await EnviarEmailAsync(alerta);
            }

            // Si no hay configuración, al menos logueamos
            if (string.IsNullOrEmpty(_webhookUrl) && string.IsNullOrEmpty(_emailAdministrador))
            {
                _logger.LogWarning("No hay configuración de alertas. Revisar appsettings.json [Alertas:EmailAdministrador] o [Alertas:WebhookUrl]");
            }
        }

        /// <summary>
        /// Envía alerta de advertencia
        /// </summary>
        public async Task EnviarAlertaAdvertenciaAsync(string mensaje)
        {
            if (!_alertasHabilitadas)
            {
                return;
            }

            var alerta = new
            {
                Tipo = "ADVERTENCIA",
                Mensaje = mensaje,
                FechaHora = DateTime.Now,
                Sistema = "Marketing Data System",
                Severidad = "MEDIUM"
            };

            _logger.LogWarning("⚠️  ADVERTENCIA: {Mensaje}", mensaje);

            if (!string.IsNullOrEmpty(_webhookUrl))
            {
                await EnviarWebhookAsync(alerta);
            }
        }

        /// <summary>
        /// Envía notificación informativa
        /// </summary>
        public async Task EnviarNotificacionAsync(string mensaje)
        {
            if (!_alertasHabilitadas)
            {
                return;
            }

            var alerta = new
            {
                Tipo = "INFO",
                Mensaje = mensaje,
                FechaHora = DateTime.Now,
                Sistema = "Marketing Data System",
                Severidad = "LOW"
            };

            _logger.LogInformation("ℹ️  NOTIFICACIÓN: {Mensaje}", mensaje);

            if (!string.IsNullOrEmpty(_webhookUrl))
            {
                await EnviarWebhookAsync(alerta);
            }
        }

        /// <summary>
        /// Envía webhook a Slack, Teams, o sistema de monitoreo
        /// </summary>
        private async Task EnviarWebhookAsync(object alerta)
        {
            try
            {
                var json = JsonSerializer.Serialize(alerta, new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_webhookUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Webhook enviado exitosamente");
                }
                else
                {
                    _logger.LogError("Error al enviar webhook: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar webhook");
            }
        }

        /// <summary>
        /// Envía email (simulado - en producción usar SendGrid, SMTP, etc.)
        /// </summary>
        private async Task EnviarEmailAsync(object alerta)
        {
            try
            {
                // TODO: Implementar envío real de email
                // Podría usar SendGrid, SMTP, etc.
                
                _logger.LogInformation("📧 EMAIL simulado enviado a {Email}: {Alerta}", 
                    _emailAdministrador, 
                    JsonSerializer.Serialize(alerta));

                // Simular latencia de envío
                await Task.Delay(100);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar email");
            }
        }
    }
} 