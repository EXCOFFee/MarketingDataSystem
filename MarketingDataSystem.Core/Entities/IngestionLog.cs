// Entidad que representa un log de ingestión de datos
using System;
using System.ComponentModel.DataAnnotations; // Para validaciones

namespace MarketingDataSystem.Core.Entities
{
    public class IngestionLog : BaseEntity // Hereda de BaseEntity (Id, etc)
    {
        [Key] // Clave primaria
        public int IdIngestionLog { get; set; } // Identificador único del log
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string MensajeError { get; set; } = string.Empty;
        public int RegistrosProcesados { get; set; }
        // FK
        public int IdFuente { get; set; } // Fuente de datos asociada
        public FuenteDeDatos Fuente { get; set; } = new();
    }
} 