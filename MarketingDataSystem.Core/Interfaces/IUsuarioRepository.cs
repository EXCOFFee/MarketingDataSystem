using System.Threading.Tasks;
using MarketingDataSystem.Core.Entities;

namespace MarketingDataSystem.Core.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de usuarios
    /// </summary>
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        /// <summary>
        /// Obtiene un usuario por su nombre de usuario
        /// </summary>
        Task<Usuario> GetByUsernameAsync(string username);

        /// <summary>
        /// Verifica si existe un usuario con el email proporcionado
        /// </summary>
        Task<bool> ExistsByEmailAsync(string email);

        /// <summary>
        /// Actualiza el último acceso del usuario
        /// </summary>
        Task UpdateLastAccessAsync(int userId);
    }
} 