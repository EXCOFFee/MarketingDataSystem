// DTO que representa un dato crudo ingresado al sistema
using System;

namespace MarketingDataSystem.Core.DTOs
{
    public class DatoCrudoDto
    {
        public int Id { get; set; } // Identificador único del dato crudo
        public string Contenido { get; set; } = string.Empty; // Contenido original del dato
        public DateTime FechaIngestion { get; set; } // Fecha de ingreso al sistema
        public int IdFuente { get; set; } // Identificador de la fuente de datos
        public string NombreFuente { get; set; } = string.Empty; // Nombre de la fuente de datos (opcional)
        public int IdDatoCrudo { get; set; } // Identificador alternativo (puede coincidir con Id)
        public DateTime Timestamp { get; set; } // Marca temporal del dato
        // Puedes agregar más propiedades según el dominio
    }
} 