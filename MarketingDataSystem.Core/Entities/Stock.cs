// Entidad que representa un registro de stock en el sistema
using System;
using System.ComponentModel.DataAnnotations; // Para validaciones

namespace MarketingDataSystem.Core.Entities
{
    public class Stock : BaseEntity // Hereda de BaseEntity (Id, etc)
    {
        [Key] // Clave primaria
        public int IdStock { get; set; } // Identificador único del registro de stock
        [Required]
        public int IdProducto { get; set; } // Identificador del producto asociado
        [Required]
        public int Cantidad { get; set; } // Cantidad disponible en stock
        public DateTime Fecha { get; set; } // Fecha del registro de stock
        public string Sucursal { get; set; } = string.Empty; // Sucursal asociada al stock
        public string Ubicacion { get; set; } = string.Empty; // Ubicación del stock
        // Relación con producto
        public Producto Producto { get; set; } = new();
    }
} 