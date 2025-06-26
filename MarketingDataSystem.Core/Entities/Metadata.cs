// Entidad que representa un metadato asociado a un dato normalizado
using System.ComponentModel.DataAnnotations; // Para validaciones
using System;

namespace MarketingDataSystem.Core.Entities
{
    public class Metadata : BaseEntity // Hereda de BaseEntity (Id, etc)
    {
        [Key] // Clave primaria
        public int IdMetadata { get; set; } // Identificador único del metadato
        [Required]
        public string Tipo { get; set; } = string.Empty; // Tipo de metadato
        [Required]
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; } = string.Empty; // Descripción del metadato
        // FK
        public int IdDatoNormalizado { get; set; } // Relación con el dato normalizado
        public DatoNormalizado DatoNormalizado { get; set; } = new(); // Dato normalizado asociado
    }
} 