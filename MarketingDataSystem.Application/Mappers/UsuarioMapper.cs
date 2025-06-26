using MarketingDataSystem.Core.DTOs;
using MarketingDataSystem.Core.Entities;

namespace MarketingDataSystem.Application.Mappers
{
    /// <summary>
    /// Mapeador para la entidad Usuario
    /// </summary>
    public static class UsuarioMapper
    {
        /// <summary>
        /// Convierte una entidad Usuario a UsuarioDTO
        /// </summary>
        public static UsuarioMarketingDto ToDTO(UsuarioMarketing usuario)
        {
            return new UsuarioMarketingDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Role = usuario.Role
            };
        }

        /// <summary>
        /// Convierte un UsuarioCreateDTO a entidad Usuario
        /// </summary>
        public static UsuarioMarketing ToEntity(UsuarioMarketingDto dto)
        {
            return new UsuarioMarketing
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Email = dto.Email,
                Role = dto.Role
            };
        }

        /// <summary>
        /// Actualiza una entidad Usuario con los datos de UsuarioUpdateDTO
        /// </summary>
        public static void UpdateEntity(UsuarioMarketing usuario, UsuarioMarketingDto dto)
        {
            if (usuario == null || dto == null) return;

            usuario.Nombre = dto.Nombre;
            usuario.Email = dto.Email;
            usuario.Role = dto.Role;
        }
    }
} 