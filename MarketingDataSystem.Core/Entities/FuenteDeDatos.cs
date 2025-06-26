// Entidad que representa una fuente de datos en el sistema
using System.Collections.Generic; // Para colecciones
using System.ComponentModel.DataAnnotations; // Para validaciones

namespace MarketingDataSystem.Core.Entities
{
    public class FuenteDeDatos : BaseEntity // Hereda de BaseEntity (Id, etc)
    {
        [Key] // Clave primaria
        public int IdFuente { get; set; } // Identificador único de la fuente de datos
        [Required]
        public string Nombre { get; set; } = string.Empty; // Nombre de la fuente de datos
        public string Tipo { get; set; } = string.Empty; // Tipo de fuente (ej: API, archivo, etc)
        public string Origen { get; set; } = string.Empty; // Origen de los datos
        public string Formato { get; set; } = string.Empty; // Formato de los datos (ej: JSON, CSV)
        public string Descripcion { get; set; } = string.Empty; // Descripción de la fuente
        public string Url { get; set; } = string.Empty; // URL de la fuente (si aplica)
        // Relación con datos crudos
        public ICollection<DatoCrudo> DatosCrudos { get; set; } = new List<DatoCrudo>(); // Datos crudos asociados a la fuente
        public ICollection<IngestionLog> Logs { get; set; } = new List<IngestionLog>();
    }
} 