// Entidad que representa un dato normalizado en el sistema
using System.Collections.Generic; // Para colecciones
using System.ComponentModel.DataAnnotations; // Para validaciones

namespace MarketingDataSystem.Core.Entities
{
    public class DatoNormalizado : BaseEntity // Hereda de BaseEntity (Id, etc)
    {
        [Key] // Clave primaria
        public int IdDatoNormalizado { get; set; } // Identificador único del dato normalizado
        [Required]
        public string IdSistema { get; set; } = string.Empty; // Identificador del sistema origen
        public string Categoria { get; set; } = string.Empty; // Categoría del dato
        public float Valor { get; set; } // Valor numérico del dato
        // FK
        public int IdDatoCrudo { get; set; } // Relación con el dato crudo original
        public DatoCrudo DatoCrudo { get; set; } = null!; // Dato crudo asociado
        // Relación con Metadata
        public ICollection<Metadata> Metadatas { get; set; } = new List<Metadata>(); // Metadatos asociados
    }
} 