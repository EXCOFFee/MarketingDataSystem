// DTO que representa un producto en el sistema
namespace MarketingDataSystem.Core.DTOs
{
    public class ProductoDto
    {
        public int IdProducto { get; set; } // Identificador único del producto
        public string Nombre { get; set; } = string.Empty; // Nombre del producto
        public string Categoria { get; set; } = string.Empty;
        public string Proveedor { get; set; } = string.Empty;
        public float Precio { get; set; } // Precio del producto
        // Puedes agregar más propiedades según el dominio
    }
} 