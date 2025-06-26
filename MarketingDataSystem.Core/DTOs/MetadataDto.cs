using System;

namespace MarketingDataSystem.Core.DTOs
{
    public class MetadataDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int IdDatoNormalizado { get; set; }
    }
} 