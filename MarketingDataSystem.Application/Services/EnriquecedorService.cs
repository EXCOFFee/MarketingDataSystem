// Servicio de enriquecimiento de datos normalizados
using System.Collections.Generic;
using MarketingDataSystem.Application.Interfaces;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Servicio de enriquecimiento de datos.
    /// Cumple con S e I de SOLID y el patrón Strategy.
    /// </summary>
    public class EnriquecedorService : IEnriquecedorService
    {
        // Método que enriquece una colección de datos normalizados
        public IEnumerable<DatoNormalizadoDto> Enriquecer(IEnumerable<DatoNormalizadoDto> datos)
        {
            // Implementación simple: enriquecer datos (por ejemplo, agregar metadatos o validaciones adicionales)
            foreach (var dato in datos)
            {
                // Aquí se podría agregar lógica de enriquecimiento
                // Por ejemplo, validar o agregar información adicional
            }
            return datos; // Retorna los datos enriquecidos
        }
    }
} 