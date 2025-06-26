using System;

namespace MarketingDataSystem.Core.Entities
{
    /// <summary>
    /// Clase base para todas las entidades del sistema
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Identificador único de la entidad
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Fecha de última modificación del registro
        /// </summary>
        public DateTime? FechaModificacion { get; set; }

        /// <summary>
        /// Indica si el registro está activo
        /// </summary>
        public bool Activo { get; set; } = true;
    }
} 