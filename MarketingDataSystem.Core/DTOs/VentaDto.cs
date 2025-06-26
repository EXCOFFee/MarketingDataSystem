// DTO que representa una venta en el sistema
namespace MarketingDataSystem.Core.DTOs
{
    public class VentaDto
    {
        public int IdVenta { get; set; } // Identificador único de la venta
        public int IdProducto { get; set; } // Identificador del producto vendido
        public int IdCliente { get; set; } // Identificador del cliente que realizó la compra
        public int Cantidad { get; set; } // Cantidad vendida
        public float PrecioUnitario { get; set; } // Precio unitario de la venta
        public DateTime Fecha { get; set; } // Fecha de la venta
        public string Sucursal { get; set; } = string.Empty;
    }
} 