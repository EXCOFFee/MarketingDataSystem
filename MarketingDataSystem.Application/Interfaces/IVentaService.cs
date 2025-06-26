using System.Collections.Generic;
using System.Threading.Tasks;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de Venta.
    /// Cumple con el principio de inversión de dependencias (D de SOLID) y el patrón Service.
    /// </summary>
    public interface IVentaService
    {
        Task<VentaDto> GetByIdAsync(int id);
        Task<IEnumerable<VentaDto>> GetAllAsync();
        Task<VentaDto> CreateAsync(VentaDto ventaDto);
        Task<VentaDto> UpdateAsync(VentaDto ventaDto);
        Task DeleteAsync(int id);
    }
} 