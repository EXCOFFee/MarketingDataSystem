// Entidad que representa un producto en el sistema
using System.Collections.Generic; // Para colecciones
using System.ComponentModel.DataAnnotations; // Para validaciones

namespace MarketingDataSystem.Core.Entities
{
    public class Producto : BaseEntity // Hereda de BaseEntity (Id, etc)
    {
        [Key] // Clave primaria
        public int IdProducto { get; set; } // Identificador único del producto
        [Required]
        public string Nombre { get; set; } = string.Empty; // Nombre del producto
        public string Categoria { get; set; } = string.Empty; // Categoría del producto
        public string Proveedor { get; set; } = string.Empty; // Proveedor del producto
        public float Precio { get; set; } // Precio del producto
        // Relación con stock y ventas
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>(); // Stocks asociados
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>(); // Ventas asociadas
    }
} 