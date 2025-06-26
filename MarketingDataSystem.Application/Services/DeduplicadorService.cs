// Servicio de deduplicación de datos normalizados
using System.Collections.Generic;
using System.Linq;
using MarketingDataSystem.Application.Interfaces;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Servicio de deduplicación de datos.
    /// Cumple con S e I de SOLID y el patrón Strategy.
    /// </summary>
    public class DeduplicadorService : IDeduplicadorService
    {
        // Método que elimina duplicados en una colección de datos normalizados
        public IEnumerable<DatoNormalizadoDto> Deduplicar(IEnumerable<DatoNormalizadoDto> datos)
        {
            // Implementación simple: eliminar duplicados basados en IdSistema
            return datos.GroupBy(d => d.IdSistema).Select(g => g.First());
        }
    }
} 