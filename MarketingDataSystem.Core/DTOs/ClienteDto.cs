// ==================== DATA TRANSFER OBJECT - CLIENTE ====================
// Este DTO define el contrato de comunicación para datos de clientes
// PROPÓSITO: Transferencia segura de datos entre capas de la aplicación
// DIFERENCIA CON ENTITY: Sin relaciones navegacionales, sin anotaciones EF
// USO: APIs REST, serialización JSON, validaciones de entrada
// SEGURIDAD: Expone solo datos necesarios, oculta detalles internos

namespace MarketingDataSystem.Core.DTOs
{
    /// <summary>
    /// Data Transfer Object para cliente - contrato de comunicación de datos
    /// RESPONSABILIDAD: Definir estructura de datos para transferencia entre capas
    /// SERIALIZACIÓN: Se convierte automáticamente a/desde JSON en APIs REST
    /// VALIDACIÓN: FluentValidation aplica reglas usando este DTO como entrada
    /// MAPPING: AutoMapper convierte automáticamente entre ClienteDto ↔ Cliente Entity
    /// SEGURIDAD: No incluye propiedades navegacionales o datos sensibles internos
    /// </summary>
    public class ClienteDto
    {
        // ========== IDENTIFICADOR ÚNICO ==========
        /// <summary>
        /// Identificador único del cliente en el sistema
        /// AUTO-GENERADO: Asignado por base de datos en creación (0 para nuevos clientes)
        /// USO: URLs REST (GET /api/cliente/123), referencias en ventas
        /// </summary>
        public int IdCliente { get; set; }

        // ========== INFORMACIÓN BÁSICA DEL CLIENTE ==========
        /// <summary>
        /// Nombre completo del cliente
        /// VALIDACIÓN: FluentValidation verifica longitud, caracteres permitidos
        /// REQUERIDO: Campo obligatorio según reglas de negocio
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Dirección de correo electrónico del cliente
        /// VALIDACIÓN: FluentValidation verifica formato RFC 5321
        /// ÚNICO: No puede haber dos clientes con el mismo email
        /// COMUNICACIÓN: Usado para envío de reportes, notificaciones
        /// </summary>
        public string Email { get; set; } = string.Empty;

        // ========== NOTAS DE DISEÑO ==========
        // DELIBERADAMENTE OMITIDO: Propiedades navegacionales (Ventas)
        // RAZÓN: Los DTOs deben ser objetos planos para serialización
        // ALTERNATIVA: Si se necesitan ventas, crear ClienteConVentasDto separado
        // EXTENSIBILIDAD: Se pueden agregar más campos según evolucione el dominio
    }
} 