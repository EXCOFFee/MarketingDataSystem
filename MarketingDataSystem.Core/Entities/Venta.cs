// Entidad que representa una venta en el sistema
using System;
using System.ComponentModel.DataAnnotations; // Para validaciones

namespace MarketingDataSystem.Core.Entities
{
    public class Venta : BaseEntity // Hereda de BaseEntity (Id, etc)
    {
        [Key] // Clave primaria
        public int IdVenta { get; set; } // Identificador único de la venta
        [Required]
        public int IdProducto { get; set; } // Identificador del producto vendido
        [Required]
        public int IdCliente { get; set; } // Identificador del cliente que realizó la compra
        [Required]
        public int Cantidad { get; set; } // Cantidad vendida
        [Required]
        public float PrecioUnitario { get; set; } // Precio unitario de la venta
        public DateTime Fecha { get; set; } // Fecha de la venta
        public string Sucursal { get; set; } = string.Empty; // Sucursal donde se realizó la venta
        public string MetodoPago { get; set; } = string.Empty; // Método de pago
        // Relaciones de navegación
        public Producto Producto { get; set; } = new();
        public Cliente Cliente { get; set; } = new();
    }
} 