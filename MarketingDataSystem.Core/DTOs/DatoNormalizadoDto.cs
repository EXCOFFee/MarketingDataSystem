// DTO que representa un dato normalizado en el sistema
namespace MarketingDataSystem.Core.DTOs
{
    public class DatoNormalizadoDto
    {
        public int Id { get; set; } // Identificador único del dato normalizado
        public string IdSistema { get; set; } = string.Empty; // Identificador del sistema origen
        public string Categoria { get; set; } = string.Empty; // Categoría del dato
        public float Valor { get; set; } // Valor numérico del dato
        public int IdDatoCrudo { get; set; } // Relación con el dato crudo original
        // Puedes agregar más propiedades según el dominio
    }
} 