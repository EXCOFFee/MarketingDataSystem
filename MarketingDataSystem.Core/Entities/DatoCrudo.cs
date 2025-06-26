// Entidad que representa un dato crudo ingresado al sistema
using System;
using System.Collections.Generic; // Para colecciones
using System.ComponentModel.DataAnnotations; // Para validaciones

namespace MarketingDataSystem.Core.Entities
{
    public class DatoCrudo : BaseEntity // Hereda de BaseEntity (Id, etc)
    {
        [Key] // Clave primaria
        public int IdDatoCrudo { get; set; } // Identificador único del dato crudo
        [Required]
        public string Contenido { get; set; } = string.Empty; // Contenido original del dato
        public DateTime FechaIngestion { get; set; } // Fecha de ingreso al sistema
        public int IdFuente { get; set; } // Identificador de la fuente de datos
        public FuenteDeDatos Fuente { get; set; } = null!; // Fuente de datos asociada
        public DateTime Timestamp { get; set; } // Marca temporal del dato
        // Relación con datos normalizados
        public ICollection<DatoNormalizado> DatosNormalizados { get; set; } = new List<DatoNormalizado>(); // Datos normalizados derivados
    }
} 