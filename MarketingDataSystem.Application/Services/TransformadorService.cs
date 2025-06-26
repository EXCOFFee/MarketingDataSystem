using MarketingDataSystem.Application.Interfaces;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Servicio de transformación de datos crudos a normalizados.
    /// Cumple con S e I de SOLID y el patrón Strategy.
    /// </summary>
    public class TransformadorService : ITransformadorService
    {
        // Método que transforma un dato crudo en un dato normalizado
        public DatoNormalizadoDto Transformar(DatoCrudoDto dato)
        {
            // Ejemplo de transformación simple (puede extenderse)
            return new DatoNormalizadoDto
            {
                IdSistema = "SISTEMA", // Identificador del sistema origen
                Categoria = "GENERAL", // Categoría genérica
                Valor = 1.0f, // Valor ficticio
                IdDatoCrudo = dato.IdDatoCrudo // Relación con el dato crudo original
            };
        }
    }
} 