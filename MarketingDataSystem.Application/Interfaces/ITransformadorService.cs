using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de transformación de datos crudos a normalizados.
    /// Cumple con el principio de segregación de interfaces (I de SOLID) y el patrón Strategy.
    /// </summary>
    public interface ITransformadorService
    {
        DatoNormalizadoDto Transformar(DatoCrudoDto dato);
    }
} 