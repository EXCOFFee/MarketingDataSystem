// DTO que representa un reporte generado en el sistema
namespace MarketingDataSystem.Core.DTOs
{
    public class ReporteDto
    {
        public int IdReporte { get; set; } // Identificador único del reporte
        public string Nombre { get; set; } = string.Empty; // Nombre del reporte
        public string Descripcion { get; set; } = string.Empty; // Descripción del reporte
        public string NombreArchivo { get; set; } = string.Empty; // Nombre del archivo generado
        public DateTime FechaGeneracion { get; set; } // Fecha de generación del reporte
        public string RutaArchivo { get; set; } = string.Empty;
    }
} 