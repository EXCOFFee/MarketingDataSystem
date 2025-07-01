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
        /// Stock mínimo permitido para alertas
        /// </summary>
        public int StockMinimo { get; set; }

        /// <summary>
        /// Stock máximo recomendado
        /// </summary>
        public int StockMaximo { get; set; }

        /// <summary>
        /// Ubicación física del stock
        /// </summary>
        public string Ubicacion { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de última actualización del stock
        /// </summary>
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;
    }
} 