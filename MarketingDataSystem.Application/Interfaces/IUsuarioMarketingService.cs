using System.Collections.Generic;
using System.Threading.Tasks;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de UsuarioMarketing.
    /// Cumple con el principio de inversión de dependencias (D de SOLID) y el patrón Service.
    /// </summary>
    public interface IUsuarioMarketingService
    {
        Task<UsuarioMarketingDto> GetByIdAsync(int id);
        Task<IEnumerable<UsuarioMarketingDto>> GetAllAsync();
        Task<UsuarioMarketingDto> CreateAsync(UsuarioMarketingDto usuarioDto);
        Task<UsuarioMarketingDto> UpdateAsync(UsuarioMarketingDto usuarioDto);
        Task DeleteAsync(int id);
        Task<bool> ExistsByEmailAsync(string email);
    }
} 