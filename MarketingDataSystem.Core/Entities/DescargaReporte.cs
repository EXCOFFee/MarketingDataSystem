// Entidad que representa una descarga de reporte realizada por un usuario
using System;
using System.ComponentModel.DataAnnotations; // Para validaciones
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketingDataSystem.Core.Entities
{
    public class DescargaReporte : BaseEntity // Hereda de BaseEntity (Id, etc)
    {
        [Key] // Clave primaria
        public int IdDescarga { get; set; } // Identificador único de la descarga
        public int IdUsuario { get; set; } // Usuario que realizó la descarga
        public int IdReporte { get; set; } // Reporte descargado
        public DateTime FechaDescarga { get; set; } // Fecha de la descarga
        public string UsuarioDescarga { get; set; } = string.Empty; // Usuario que descargó el reporte
        public string Formato { get; set; } = string.Empty;
        // Relaciones de navegación
        public UsuarioMarketing Usuario { get; set; } = new();
        public Reporte Reporte { get; set; } = new();
    }
} 