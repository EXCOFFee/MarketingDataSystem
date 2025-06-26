// ==================== VALIDADOR DE REGLAS DE NEGOCIO - CLIENTE ====================
// Este validador implementa todas las reglas de validación para clientes
// FRAMEWORK: FluentValidation - Validaciones declarativas y expresivas
// SEGURIDAD: Incluye protección anti-XSS y validaciones RFC
// PROPÓSITO: Garantizar integridad de datos antes de persistencia
// SOLID: Principio S - Una sola responsabilidad (validar ClienteDto)

using FluentValidation;
using MarketingDataSystem.Core.DTOs;
using System.Text.RegularExpressions;

namespace MarketingDataSystem.Application.Validators
{
    /// <summary>
    /// Validador especializado para ClienteDto con reglas de negocio completas
    /// FRAMEWORK: FluentValidation - validaciones declarativas y testeable
    /// RESPONSABILIDAD: Validar integridad, formato y seguridad de datos de cliente
    /// SOLID: Principio S - Una sola responsabilidad (validación de clientes)
    /// SEGURIDAD: Protección anti-XSS, validación RFC, sanitización de entrada
    /// EJECUCIÓN: Se ejecuta automáticamente en ClienteController antes de persistir
    /// TESTEABLE: Reglas declarativas fáciles de probar unitariamente
    /// </summary>
    public class ClienteDtoValidator : AbstractValidator<ClienteDto>
    {
        /// <summary>
        /// Constructor que define todas las reglas de validación para clientes
        /// PATRÓN: Builder pattern usando API fluida de FluentValidation
        /// EJECUCIÓN: Estas reglas se evalúan automáticamente cuando se valida un ClienteDto
        /// </summary>
        public ClienteDtoValidator()
        {
            // ========== VALIDACIÓN DE EMAIL (RFC 5321 COMPLIANT) ==========
            RuleFor(x => x.Email)
                .NotEmpty()                    // REGLA: Email es obligatorio
                .WithMessage("El email es obligatorio")
                .EmailAddress()                // REGLA: Formato RFC 5321 válido
                .WithMessage("El formato del email no es válido")
                .MaximumLength(320)            // REGLA: RFC 5321 - máximo 320 caracteres para email
                .WithMessage("El email no puede exceder 320 caracteres");

            // ========== VALIDACIÓN DE NOMBRE (CARACTERES Y LONGITUD) ==========
            RuleFor(x => x.Nombre)
                .NotEmpty()                    // REGLA: Nombre es obligatorio
                .WithMessage("El nombre es obligatorio")
                .MinimumLength(2)              // REGLA: Mínimo 2 caracteres para nombres válidos
                .WithMessage("El nombre debe tener al menos 2 caracteres")
                .MaximumLength(100)            // REGLA: Máximo 100 caracteres para base de datos
                .WithMessage("El nombre no puede exceder 100 caracteres")
                .Matches(@"^[a-zA-ZÀ-ÿ\u00f1\u00d1\s]+$")  // REGLA: Solo letras, acentos y espacios
                .WithMessage("El nombre solo puede contener letras y espacios");

            // ========== VALIDACIÓN CONDICIONAL DE ID ==========
            // Solo validar ID si es una actualización (ID > 0)
            When(x => x.IdCliente > 0, () =>
            {
                RuleFor(x => x.IdCliente)
                    .GreaterThan(0)            // REGLA: ID válido para actualizaciones
                    .WithMessage("El ID del cliente debe ser mayor a 0");
            });

            // ========== VALIDACIONES DE SEGURIDAD ANTI-XSS ==========
            // Protección contra Cross-Site Scripting (XSS)
            RuleFor(x => x)
                .Must(NoContenerScripts)       // REGLA: Bloquear scripts maliciosos
                .WithMessage("Los datos no pueden contener scripts maliciosos");
        }

        /// <summary>
        /// Método de seguridad que valida campos contra ataques XSS (Cross-Site Scripting)
        /// SEGURIDAD: Protección crítica contra inyección de código malicioso
        /// PATRONES: Detecta scripts HTML, JavaScript, VBScript, eventos DOM y embeds
        /// SCOPE: Valida todos los campos de texto del ClienteDto
        /// NIVEL: Protección básica - en producción se recomienda biblioteca especializada
        /// </summary>
        /// <param name="cliente">Objeto ClienteDto a validar contra XSS</param>
        /// <returns>true si es seguro, false si contiene patrones maliciosos</returns>
        private bool NoContenerScripts(ClienteDto cliente)
        {
            // ========== PATRONES DE DETECCIÓN XSS ==========
            // Lista de expresiones regulares para detectar código malicioso
            var scriptPatterns = new[]
            {
                @"<script[^>]*>.*?</script>",  // Tags <script> con contenido
                @"javascript:",                // Protocolo javascript: en URLs
                @"vbscript:",                 // Protocolo VBScript (IE legacy)
                @"on\w+\s*=",                 // Eventos DOM (onclick, onload, etc.)
                @"<iframe[^>]*>",             // iFrames para cargar contenido externo
                @"<object[^>]*>",             // Objects embebidos
                @"<embed[^>]*>"               // Embeds de plugins
            };

            // ========== CAMPOS A VALIDAR ==========
            // Array con todos los campos de texto del DTO
            var fields = new[] { cliente.Nombre, cliente.Email };

            // ========== VALIDACIÓN EXHAUSTIVA ==========
            foreach (var field in fields)
            {
                if (string.IsNullOrEmpty(field)) continue; // Skip campos vacíos

                // Verificar cada patrón malicioso contra cada campo
                foreach (var pattern in scriptPatterns)
                {
                    // Matching case-insensitive para mayor cobertura
                    if (Regex.IsMatch(field, pattern, RegexOptions.IgnoreCase))
                        return false; // DETECTADO: Código malicioso encontrado
                }
            }

            return true; // SEGURO: No se detectaron patrones maliciosos
        }
    }
} 