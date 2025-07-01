// ==================== SERVICIO CRÍTICO DE MONITOREO Y ALERTAS ====================
// Este servicio maneja todas las notificaciones críticas del sistema empresarial
// CAPA: Application - Orquesta notificaciones hacia sistemas externos de monitoreo
// CRITICIDAD: MÁXIMA - Primera línea de defensa ante fallos del sistema
// CANALES: Email, Webhook (Slack, Teams), Logging estructurado para SIEM
// SOLID: Cumple principios S (responsabilidad única), D (inversión dependencias)
// CONFIGURABILIDAD: Completamente configurable vía appsettings.json

using System;
using System.Net.Http;             // Para webhooks HTTP hacia sistemas externos
using System.Text;
using System.Text.Json;            // Serialización JSON para APIs de notificación
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;  // Configuración desde appsettings.json
using Microsoft.Extensions.Logging;        // Logging estructurado integrado
using MarketingDataSystem.Application.Interfaces;

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Servicio crítico de monitoreo y alertas para notificación de fallos empresariales
    /// RESPONSABILIDAD: Detectar, categorizar y notificar eventos críticos del sistema
    /// ARQUITECTURA: Capa Application - orquesta notificaciones hacia múltiples canales
    /// CRITICIDAD: MÁXIMA - Sistema de early warning para fallos que afectan el negocio
    /// SOLID:
    /// - S: Una sola responsabilidad (gestión completa de alertas y notificaciones)
    /// - D: Depende de abstracciones (ILogger, IConfiguration, HttpClient)
    /// CANALES DE NOTIFICACIÓN:
    /// - Email: Administradores y equipos on-call
    /// - Webhook: Slack, Microsoft Teams, PagerDuty, sistemas de monitoreo
    /// - Logs: Logging estructurado para SIEM y herramientas de análisis
    /// TIPOS DE ALERTAS:
    /// - CRÍTICA: Fallos que requieren intervención inmediata (ETL, BD, seguridad)
    /// - ADVERTENCIA: Problemas que necesitan atención pero no bloquean operación
    /// - INFORMACIÓN: Notificaciones de estado y completación de procesos
    /// CONFIGURACIÓN: Completamente configurable vía appsettings.json sin recompilación
    /// RESILIENCIA: Fallos en notificación no afectan la operación principal del sistema
    /// INTEGRACIÓN: Usado por todos los servicios críticos del sistema (ETL, Auth, etc.)
    /// </summary>
    public class AlertaService : IAlertaService
    {
        // ========== DEPENDENCIAS PARA NOTIFICACIONES MULTI-CANAL ==========
        private readonly ILogger<AlertaService> _logger;      // Logging estructurado Microsoft
        private readonly IConfiguration _configuration;       // Configuración desde appsettings.json
        private readonly HttpClient _httpClient;              // Cliente HTTP para webhooks

        // ========== CONFIGURACIÓN EXTERNA DESDE APPSETTINGS.JSON ==========
        private readonly string? _emailAdministrador;   // Email del administrador on-call
        private readonly string? _webhookUrl;           // URL webhook (Slack, Teams, PagerDuty)
        private readonly bool _alertasHabilitadas;      // Switch maestro para habilitar/deshabilitar

        /// <summary>
        /// Constructor con inyección de dependencias para sistema de alertas multi-canal
        /// PATRÓN: Dependency Injection - todas las dependencias son interfaces testeable
        /// CONFIGURACIÓN: Carga configuración desde appsettings.json al inicializar
        /// TESTABILIDAD: Permite mocking completo para pruebas de notificaciones
        /// SOLID: Principio D - depende de abstracciones para máxima flexibilidad
        /// RESILIENCIA: Configuración robusta con valores por defecto seguros
        /// </summary>
        /// <param name="logger">Logger estructurado para trazabilidad de alertas</param>
        /// <param name="configuration">Configuración desde appsettings.json</param>
        /// <param name="httpClient">Cliente HTTP para webhooks hacia sistemas externos</param>
        public AlertaService(
            ILogger<AlertaService> logger,
            IConfiguration configuration,
            HttpClient httpClient)
        {
            // ========== INYECCIÓN DE DEPENDENCIAS ==========
            _logger = logger;                    // Logger Microsoft para trazabilidad
            _configuration = configuration;      // Configuración externa flexible
            _httpClient = httpClient;            // Cliente HTTP para integraciones

            // ========== CARGA DE CONFIGURACIÓN EXTERNA ==========
            // Configuración desde appsettings.json para evitar recompilación
            _emailAdministrador = _configuration["Alertas:EmailAdministrador"];
            _webhookUrl = _configuration["Alertas:WebhookUrl"];
            _alertasHabilitadas = _configuration.GetValue<bool>("Alertas:Habilitadas", false); // Seguro por defecto
        }

        /// <summary>
        /// Envía alerta crítica que requiere intervención INMEDIATA de administradores
        /// CRITICIDAD: MÁXIMA - Para fallos que afectan operación comercial del sistema
        /// CANALES: Email + Webhook + Logging crítico para máxima visibilidad
        /// CASOS DE USO:
        /// - Fallos del pipeline ETL después de todos los reintentos
        /// - Errores de conectividad con base de datos
        /// - Fallos de autenticación o seguridad
        /// - Corrupción de datos críticos
        /// - Caídas de servicios esenciales
        /// ESCALATION: Se envía por TODOS los canales configurados simultáneamente
        /// FORMATO: JSON estructurado compatible con sistemas de monitoreo empresariales
        /// </summary>
        /// <param name="mensaje">Mensaje principal de la alerta crítica</param>
        /// <param name="detalleError">Detalle técnico del error para diagnóstico</param>
        public async Task EnviarAlertaCriticaAsync(string mensaje, string detalleError)
        {
            // ========== VERIFICACIÓN DE HABILITACIÓN GLOBAL ==========
            if (!_alertasHabilitadas)
            {
                _logger.LogWarning("Sistema de alertas deshabilitado globalmente. No se enviará alerta crítica.");
                return; // Exit early si las alertas están deshabilitadas
            }

            // ========== CREACIÓN DE PAYLOAD ESTRUCTURADO ==========
            var alerta = new
            {
                Tipo = "CRITICA",                           // Categorización para sistemas externos
                Mensaje = mensaje,                          // Mensaje legible para humanos
                DetalleError = detalleError,                // Información técnica detallada
                FechaHora = DateTime.Now,                   // Timestamp exacto del incidente
                Sistema = "Marketing Data System",          // Identificación del sistema origen
                Severidad = "HIGH"                          // Nivel de severidad estándar
            };

            // ========== LOGGING CRÍTICO ESTRUCTURADO ==========
            // Emoji para visibilidad inmediata en logs, structured logging para SIEM
            _logger.LogCritical("🚨 ALERTA CRÍTICA: {Mensaje} - {DetalleError}", mensaje, detalleError);

            // ========== NOTIFICACIÓN MULTI-CANAL SIMULTÁNEA ==========
            
            // Canal 1: Webhook (Slack, Teams, PagerDuty, sistemas de monitoreo)
            if (!string.IsNullOrEmpty(_webhookUrl))
            {
                await EnviarWebhookAsync(alerta); // No await individual para paralelismo
            }

            // Canal 2: Email (administradores, equipos on-call)
            if (!string.IsNullOrEmpty(_emailAdministrador))
            {
                await EnviarEmailAsync(alerta); // Notificación directa a responsables
            }

            // ========== VALIDACIÓN DE CONFIGURACIÓN ==========
            // Advertir si no hay canales configurados para alertas críticas
            if (string.IsNullOrEmpty(_webhookUrl) && string.IsNullOrEmpty(_emailAdministrador))
            {
                _logger.LogWarning("⚠️ CONFIGURACIÓN INCOMPLETA: No hay canales de alerta configurados. " +
                                 "Revisar appsettings.json [Alertas:EmailAdministrador] o [Alertas:WebhookUrl]");
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