using System;

// DTO que representa una fuente de datos en el sistema
namespace MarketingDataSystem.Core.DTOs
{
    public class FuenteDeDatosDto
    {
        public int IdFuente { get; set; } // Identificador único de la fuente de datos
        public string Nombre { get; set; } = string.Empty; // Nombre de la fuente de datos
        public string Tipo { get; set; } = string.Empty; // Tipo de fuente (ej: API, archivo, etc)
        public string Origen { get; set; } = string.Empty; // Origen de los datos
        public string Formato { get; set; } = string.Empty; // Formato de los datos (ej: JSON, CSV)
        public string Descripcion { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        // Puedes agregar más propiedades según el dominio
    }
} 