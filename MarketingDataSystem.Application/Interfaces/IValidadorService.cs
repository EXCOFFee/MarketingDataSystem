using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de validación de datos crudos.
    /// Cumple con el principio de segregación de interfaces (I de SOLID) y el patrón Strategy.
    /// </summary>
    public interface IValidadorService
    {
        bool Validar(DatoCrudoDto dato);
    }
} 