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
    /// Servicio de aplicación para Stock.
    /// Cumple con el principio de responsabilidad única (S de SOLID),
    /// inversión de dependencias (D de SOLID) y el patrón Service.
    /// </summary>
    public class StockService : IStockService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StockService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork; // Inyección de dependencias (D de SOLID)
            _mapper = mapper;
        }

        public async Task<StockDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Stocks.GetByIdAsync(id);
            return _mapper.Map<StockDto>(entity);
        }

        public async Task<IEnumerable<StockDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.Stocks.GetAllAsync();
            return _mapper.Map<IEnumerable<StockDto>>(entities);
        }

        public async Task<StockDto> CreateAsync(StockDto stockDto)
        {
            var entity = _mapper.Map<Stock>(stockDto);
            await _unitOfWork.Stocks.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<StockDto>(entity);
        }

        public async Task<StockDto> UpdateAsync(StockDto stockDto)
        {
            var entity = _mapper.Map<Stock>(stockDto);
            _unitOfWork.Stocks.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<StockDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Stocks.GetByIdAsync(id);
            if (entity != null)
            {
                _unitOfWork.Stocks.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
} 