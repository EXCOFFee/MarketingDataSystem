using System.Collections.Generic;
using System.Threading.Tasks;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de Producto.
    /// Cumple con el principio de inversión de dependencias (D de SOLID) y el patrón Service.
    /// </summary>
    public interface IProductoService
    {
        Task<ProductoDto> GetByIdAsync(int id);
        Task<IEnumerable<ProductoDto>> GetAllAsync();
        Task<ProductoDto> CreateAsync(ProductoDto productoDto);
        Task<ProductoDto> UpdateAsync(ProductoDto productoDto);
        Task DeleteAsync(int id);
    }
} 