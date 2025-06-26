using System;

namespace MarketingDataSystem.Core.DTOs
{
    public class IngestionLogDto
    {
        public int Id { get; set; }
        public int IdFuente { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string MensajeError { get; set; } = string.Empty;
        public int RegistrosProcesados { get; set; }
        public string NombreFuente { get; set; } = string.Empty;
    }
} 