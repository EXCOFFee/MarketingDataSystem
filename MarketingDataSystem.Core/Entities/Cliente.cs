// ==================== ENTIDAD DE DOMINIO - CLIENTE ====================
// Esta entidad representa un cliente en el sistema de marketing
// CAPA: Core/Domain - Entidad central del negocio con reglas y relaciones
// PATRÓN: Domain Entity - representa conceptos de negocio con identidad única
// ORM: Entity Framework mapeará esta clase a tabla en base de datos
// RELACIONES: Un cliente puede tener múltiples ventas (1:N)

using System.Collections.Generic; // Para colecciones de entidades relacionadas
using System.ComponentModel.DataAnnotations; // Para anotaciones de validación y mapeo

namespace MarketingDataSystem.Core.Entities
{
    /// <summary>
    /// Entidad de dominio que representa un cliente del sistema de marketing
    /// RESPONSABILIDAD: Encapsular datos y comportamiento de un cliente
    /// IDENTIDAD: Cada cliente es único por su IdCliente (Primary Key)
    /// RELACIONES: Agregado raíz para sus ventas asociadas
    /// PERSISTENCIA: Entity Framework mapea a tabla "Clientes"
    /// PII: Contiene información personal identificable (protección GDPR/LGPD)
    /// </summary>
    public class Cliente : BaseEntity // Hereda propiedades comunes (timestamps, soft delete, etc.)
    {
        // ========== CLAVE PRIMARIA ==========
        [Key] // Anotación EF: Define como Primary Key en base de datos
        public int IdCliente { get; set; } // Identificador único auto-incremental

        // ========== INFORMACIÓN PERSONAL IDENTIFICABLE (PII) ==========
        [Required] // Validación: Campo obligatorio en base de datos
        public string Nombre { get; set; } = string.Empty; // Nombre completo del cliente

        [Required] // Validación: Email es obligatorio y único
        public string Email { get; set; } = string.Empty; // Correo electrónico para comunicación

        // ========== PROPIEDADES NAVEGACIONALES (RELACIONES) ==========
        /// <summary>
        /// Colección de ventas realizadas por este cliente
        /// RELACIÓN: 1:N (Un cliente puede tener muchas ventas)
        /// LAZY LOADING: Entity Framework carga ventas cuando se accede
        /// AGREGADO: Cliente es el agregado raíz que gestiona sus ventas
        /// </summary>
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>(); // Inicialización para evitar null
    }
} 