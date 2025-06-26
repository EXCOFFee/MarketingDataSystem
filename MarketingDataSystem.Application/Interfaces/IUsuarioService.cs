using System.Collections.Generic;
using System.Threading.Tasks;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de usuarios de marketing
    /// </summary>
    public interface IUsuarioService
    {
        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        Task<UsuarioMarketingDto> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene todos los usuarios
        /// </summary>
        Task<IEnumerable<UsuarioMarketingDto>> GetAllAsync();

        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        Task<UsuarioMarketingDto> CreateAsync(UsuarioMarketingDto usuarioDto);

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        Task<UsuarioMarketingDto> UpdateAsync(UsuarioMarketingDto usuarioDto);

        /// <summary>
        /// Elimina un usuario
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// Autentica un usuario
        /// </summary>
        Task<UsuarioMarketingDto> AuthenticateAsync(string username, string password);

        /// <summary>
        /// Verifica si existe un usuario con el email proporcionado
        /// </summary>
        Task<bool> ExistsByEmailAsync(string email);
    }
} 