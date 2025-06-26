using System.Collections.Generic;
using System.Threading.Tasks;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de FuenteDeDatos.
    /// Cumple con el principio de inversión de dependencias (D de SOLID) y el patrón Service.
    /// </summary>
    public interface IFuenteDeDatosService
    {
        Task<FuenteDeDatosDto> GetByIdAsync(int id);
        Task<IEnumerable<FuenteDeDatosDto>> GetAllAsync();
        Task<FuenteDeDatosDto> CreateAsync(FuenteDeDatosDto fuenteDto);
        Task<FuenteDeDatosDto> UpdateAsync(FuenteDeDatosDto fuenteDto);
        Task DeleteAsync(int id);
    }
} 