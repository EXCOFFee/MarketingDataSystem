// Entidad que representa un reporte generado en el sistema
using System;
using System.Collections.Generic; // Para colecciones
using System.ComponentModel.DataAnnotations; // Para validaciones

namespace MarketingDataSystem.Core.Entities
{
    public class Reporte : BaseEntity // Hereda de BaseEntity (Id, etc)
    {
        [Key] // Clave primaria
        public int IdReporte { get; set; } // Identificador único del reporte
        public string Nombre { get; set; } = string.Empty; // Nombre del reporte
        public string Descripcion { get; set; } = string.Empty; // Descripción del reporte
        [Required]
        public string NombreArchivo { get; set; } = string.Empty; // Nombre del archivo generado
        [Required]
        public DateTime FechaGeneracion { get; set; } // Fecha de generación del reporte
        [Required]
        public string RutaArchivo { get; set; } = string.Empty; // Ruta física del archivo
        // Relación con descargas
        public ICollection<DescargaReporte> Descargas { get; set; } = new List<DescargaReporte>(); // Descargas asociadas a este reporte
    }
} 