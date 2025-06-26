using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MarketingDataSystem.Core.DTOs;
using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Application.Interfaces;

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Servicio de aplicación para UsuarioMarketing.
    /// Cumple con el principio de responsabilidad única (S de SOLID),
    /// inversión de dependencias (D de SOLID) y el patrón Service.
    /// </summary>
    public class UsuarioMarketingService : IUsuarioMarketingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsuarioMarketingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork; // Inyección de dependencias (D de SOLID)
            _mapper = mapper;
        }

        public async Task<UsuarioMarketingDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.UsuariosMarketing.GetByIdAsync(id);
            return _mapper.Map<UsuarioMarketingDto>(entity);
        }

        public async Task<IEnumerable<UsuarioMarketingDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.UsuariosMarketing.GetAllAsync();
            return _mapper.Map<IEnumerable<UsuarioMarketingDto>>(entities);
        }

        public async Task<UsuarioMarketingDto> CreateAsync(UsuarioMarketingDto usuarioDto)
        {
            var entity = _mapper.Map<UsuarioMarketing>(usuarioDto);
            await _unitOfWork.UsuariosMarketing.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UsuarioMarketingDto>(entity);
        }

        public async Task<UsuarioMarketingDto> UpdateAsync(UsuarioMarketingDto usuarioDto)
        {
            var entity = _mapper.Map<UsuarioMarketing>(usuarioDto);
            _unitOfWork.UsuariosMarketing.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UsuarioMarketingDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.UsuariosMarketing.GetByIdAsync(id);
            if (entity != null)
            {
                _unitOfWork.UsuariosMarketing.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            var usuarios = await _unitOfWork.UsuariosMarketing.FindAsync(u => u.Email == email);
            return usuarios != null && usuarios.Any();
        }
    }
} 