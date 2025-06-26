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
    /// Servicio de aplicación para Producto.
    /// Cumple con el principio de responsabilidad única (S de SOLID),
    /// inversión de dependencias (D de SOLID) y el patrón Service.
    /// </summary>
    public class ProductoService : IProductoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork; // Inyección de dependencias (D de SOLID)
            _mapper = mapper;
        }

        public async Task<ProductoDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Productos.GetByIdAsync(id);
            return _mapper.Map<ProductoDto>(entity);
        }

        public async Task<IEnumerable<ProductoDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.Productos.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductoDto>>(entities);
        }

        public async Task<ProductoDto> CreateAsync(ProductoDto productoDto)
        {
            var entity = _mapper.Map<Producto>(productoDto);
            await _unitOfWork.Productos.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProductoDto>(entity);
        }

        public async Task<ProductoDto> UpdateAsync(ProductoDto productoDto)
        {
            var entity = _mapper.Map<Producto>(productoDto);
            _unitOfWork.Productos.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProductoDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Productos.GetByIdAsync(id);
            if (entity != null)
            {
                _unitOfWork.Productos.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
} 