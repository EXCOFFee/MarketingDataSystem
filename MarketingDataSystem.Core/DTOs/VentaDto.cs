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

        // ========== PROPIEDADES ADICIONALES PARA COMPATIBILIDAD CON TESTS ==========
        /// <summary>
        /// Alias para IdProducto - Compatibilidad con tests existentes
        /// </summary>
        public int ProductoId 
        { 
            get => IdProducto; 
            set => IdProducto = value; 
        }

        /// <summary>
        /// Monto total de la venta (Cantidad × PrecioUnitario) - Propiedad calculada para tests
        /// </summary>
        public decimal Monto { get => (decimal)Cantidad * (decimal)PrecioUnitario; set { } }

        /// <summary>
        /// Total de la venta - Alias para Monto para compatibilidad
        /// </summary>
        public decimal Total { get => Monto; set { } }

        /// <summary>
        /// Alias para IdVenta - Compatibilidad con tests existentes
        /// </summary>
        public int Id 
        { 
            get => IdVenta; 
            set => IdVenta = value; 
        }

        /// <summary>
        /// Alias para IdCliente - Compatibilidad con tests existentes
        /// </summary>
        public int ClienteId 
        { 
            get => IdCliente; 
            set => IdCliente = value; 
        }
    }
} 