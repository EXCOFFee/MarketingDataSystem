using MarketingDataSystem.Application.Interfaces;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Servicio de validación de datos crudos.
    /// Cumple con S e I de SOLID y el patrón Strategy.
    /// </summary>
    public class ValidadorService : IValidadorService
    {
        // Método que valida si un dato crudo es válido
        public bool Validar(DatoCrudoDto dato)
        {
            // Valida que el contenido no sea nulo o vacío y que el timestamp sea válido
            return !string.IsNullOrWhiteSpace(dato.Contenido) && dato.Timestamp != default;
        }
    }
} 