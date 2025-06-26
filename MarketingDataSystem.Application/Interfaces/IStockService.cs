using System.Collections.Generic;
using System.Threading.Tasks;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de Stock.
    /// Cumple con el principio de inversión de dependencias (D de SOLID) y el patrón Service.
    /// </summary>
    public interface IStockService
    {
        Task<StockDto> GetByIdAsync(int id);
        Task<IEnumerable<StockDto>> GetAllAsync();
        Task<StockDto> CreateAsync(StockDto stockDto);
        Task<StockDto> UpdateAsync(StockDto stockDto);
        Task DeleteAsync(int id);
    }
} 