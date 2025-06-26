// DTO que representa un registro de stock en el sistema
namespace MarketingDataSystem.Core.DTOs
{
    public class StockDto
    {
        public int IdStock { get; set; } // Identificador único del registro de stock
        public int IdProducto { get; set; } // Identificador del producto asociado
        public int Cantidad { get; set; } // Cantidad disponible en stock
        public DateTime Fecha { get; set; }
        public string Sucursal { get; set; } = string.Empty;
    }
} 