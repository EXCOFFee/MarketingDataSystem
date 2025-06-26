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
    /// Servicio de aplicación para Reporte.
    /// Cumple con el principio de responsabilidad única (S de SOLID),
    /// inversión de dependencias (D de SOLID) y el patrón Service.
    /// </summary>
    public class ReporteService : IReporteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReporteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork; // Inyección de dependencias (D de SOLID)
            _mapper = mapper;
        }

        public async Task<ReporteDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Reportes.GetByIdAsync(id);
            return _mapper.Map<ReporteDto>(entity);
        }

        public async Task<IEnumerable<ReporteDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.Reportes.GetAllAsync();
            return _mapper.Map<IEnumerable<ReporteDto>>(entities);
        }

        public async Task<ReporteDto> CreateAsync(ReporteDto reporteDto)
        {
            var entity = _mapper.Map<Reporte>(reporteDto);
            await _unitOfWork.Reportes.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ReporteDto>(entity);
        }

        public async Task<ReporteDto> UpdateAsync(ReporteDto reporteDto)
        {
            var entity = _mapper.Map<Reporte>(reporteDto);
            _unitOfWork.Reportes.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ReporteDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Reportes.GetByIdAsync(id);
            if (entity != null)
            {
                _unitOfWork.Reportes.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
} 