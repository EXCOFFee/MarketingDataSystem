using System.Collections.Generic;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Interfaces
{
    public interface IDeduplicadorService
    {
        IEnumerable<DatoNormalizadoDto> Deduplicar(IEnumerable<DatoNormalizadoDto> datos);
    }
} 