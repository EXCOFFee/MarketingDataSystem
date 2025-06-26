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
    /// Servicio de aplicación para FuenteDeDatos.
    /// Cumple con el principio de responsabilidad única (S de SOLID),
    /// inversión de dependencias (D de SOLID) y el patrón Service.
    /// </summary>
    public class FuenteDeDatosService : IFuenteDeDatosService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FuenteDeDatosService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork; // Inyección de dependencias (D de SOLID)
            _mapper = mapper;
        }

        public async Task<FuenteDeDatosDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.FuentesDeDatos.GetByIdAsync(id);
            return _mapper.Map<FuenteDeDatosDto>(entity);
        }

        public async Task<IEnumerable<FuenteDeDatosDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.FuentesDeDatos.GetAllAsync();
            return _mapper.Map<IEnumerable<FuenteDeDatosDto>>(entities);
        }

        public async Task<FuenteDeDatosDto> CreateAsync(FuenteDeDatosDto fuenteDto)
        {
            var entity = _mapper.Map<FuenteDeDatos>(fuenteDto);
            await _unitOfWork.FuentesDeDatos.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<FuenteDeDatosDto>(entity);
        }

        public async Task<FuenteDeDatosDto> UpdateAsync(FuenteDeDatosDto fuenteDto)
        {
            var entity = _mapper.Map<FuenteDeDatos>(fuenteDto);
            _unitOfWork.FuentesDeDatos.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<FuenteDeDatosDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.FuentesDeDatos.GetByIdAsync(id);
            if (entity != null)
            {
                _unitOfWork.FuentesDeDatos.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
} 