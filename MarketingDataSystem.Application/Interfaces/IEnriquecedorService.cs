using System.Collections.Generic;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Interfaces
{
    public interface IEnriquecedorService
    {
        IEnumerable<DatoNormalizadoDto> Enriquecer(IEnumerable<DatoNormalizadoDto> datos);
    }
} 