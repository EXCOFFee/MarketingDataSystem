using System.Collections.Generic;
using System.Threading.Tasks;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de Cliente.
    /// Cumple con el principio de inversión de dependencias (D de SOLID) y el patrón Service.
    /// </summary>
    public interface IClienteService
    {
        Task<ClienteDto> GetByIdAsync(int id);
        Task<IEnumerable<ClienteDto>> GetAllAsync();
        Task<ClienteDto> CreateAsync(ClienteDto clienteDto);
        Task<ClienteDto> UpdateAsync(ClienteDto clienteDto);
        Task DeleteAsync(int id);
    }
} 