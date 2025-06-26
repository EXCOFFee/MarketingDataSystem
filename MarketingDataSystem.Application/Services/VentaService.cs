using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MarketingDataSystem.Core.DTOs;
using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Servicio de aplicación para Venta.
    /// Cumple con el principio de responsabilidad única (S de SOLID),
    /// inversión de dependencias (D de SOLID) y el patrón Service.
    /// </summary>
    public class VentaService : IVentaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VentaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork; // Inyección de dependencias (D de SOLID)
            _mapper = mapper;
        }

        public async Task<VentaDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Ventas.GetByIdAsync(id);
            return _mapper.Map<VentaDto>(entity);
        }

        public async Task<IEnumerable<VentaDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.Ventas.GetAllAsync();
            return _mapper.Map<IEnumerable<VentaDto>>(entities);
        }

        public async Task<VentaDto> CreateAsync(VentaDto ventaDto)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var entity = _mapper.Map<Venta>(ventaDto);
                    await _unitOfWork.Ventas.AddAsync(entity);
                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return _mapper.Map<VentaDto>(entity);
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<VentaDto> UpdateAsync(VentaDto ventaDto)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var entity = _mapper.Map<Venta>(ventaDto);
                    _unitOfWork.Ventas.Update(entity);
                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return _mapper.Map<VentaDto>(entity);
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Ventas.GetByIdAsync(id);
            if (entity != null)
            {
                _unitOfWork.Ventas.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
} 