using System.Collections.Generic;
using System.Threading.Tasks;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de Reporte.
    /// Cumple con el principio de inversión de dependencias (D de SOLID) y el patrón Service.
    /// </summary>
    public interface IReporteService
    {
        Task<ReporteDto> GetByIdAsync(int id);
        Task<IEnumerable<ReporteDto>> GetAllAsync();
        Task<ReporteDto> CreateAsync(ReporteDto reporteDto);
        Task<ReporteDto> UpdateAsync(ReporteDto reporteDto);
        Task DeleteAsync(int id);
    }
} 