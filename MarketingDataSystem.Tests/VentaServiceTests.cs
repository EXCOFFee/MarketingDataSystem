using Xunit;
using Moq;
using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Application.Services;
using MarketingDataSystem.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MarketingDataSystem.Tests
{
    public class VentaServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IVentaRepository> _mockVentaRepository;
        private readonly Mock<IProductoRepository> _mockProductoRepository;
        private readonly VentaService _ventaService;

        public VentaServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockVentaRepository = new Mock<IVentaRepository>();
            _mockProductoRepository = new Mock<IProductoRepository>();

            _mockUnitOfWork.Setup(uow => uow.VentaRepository).Returns(_mockVentaRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.ProductoRepository).Returns(_mockProductoRepository.Object);

            _ventaService = new VentaService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetAllVentas_ShouldReturnAllVentas()
        {
            // Arrange
            var ventas = new List<Venta>
            {
                new Venta { Id = 1, Fecha = DateTime.Now, Monto = 1000, ProductoId = 1 },
                new Venta { Id = 2, Fecha = DateTime.Now, Monto = 2000, ProductoId = 2 }
            };

            _mockVentaRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(ventas);

            // Act
            var result = await _ventaService.GetAllVentasAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(ventas[0].Id, result.First().Id);
            Assert.Equal(ventas[1].Id, result.Last().Id);
        }

        [Fact]
        public async Task GetVentaById_WithValidId_ShouldReturnVenta()
        {
            // Arrange
            var venta = new Venta { Id = 1, Fecha = DateTime.Now, Monto = 1000, ProductoId = 1 };
            _mockVentaRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(venta);

            // Act
            var result = await _ventaService.GetVentaByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(venta.Id, result.Id);
            Assert.Equal(venta.Monto, result.Monto);
        }

        [Fact]
        public async Task CreateVenta_WithValidData_ShouldCreateVenta()
        {
            // Arrange
            var ventaDto = new VentaDto
            {
                Fecha = DateTime.Now,
                Monto = 1000,
                ProductoId = 1
            };

            var producto = new Producto { Id = 1, Stock = 10 };
            _mockProductoRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(producto);

            _mockVentaRepository.Setup(repo => repo.AddAsync(It.IsAny<Venta>()))
                .ReturnsAsync((Venta v) => v);

            // Act
            var result = await _ventaService.CreateVentaAsync(ventaDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ventaDto.Monto, result.Monto);
            Assert.Equal(ventaDto.ProductoId, result.ProductoId);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateVenta_WithValidData_ShouldUpdateVenta()
        {
            // Arrange
            var ventaDto = new VentaDto
            {
                Id = 1,
                Fecha = DateTime.Now,
                Monto = 2000,
                ProductoId = 1
            };

            var existingVenta = new Venta { Id = 1, Monto = 1000, ProductoId = 1 };
            _mockVentaRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(existingVenta);

            // Act
            var result = await _ventaService.UpdateVentaAsync(ventaDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ventaDto.Monto, result.Monto);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteVenta_WithValidId_ShouldDeleteVenta()
        {
            // Arrange
            var venta = new Venta { Id = 1, Monto = 1000, ProductoId = 1 };
            _mockVentaRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(venta);

            // Act
            await _ventaService.DeleteVentaAsync(1);

            // Assert
            _mockVentaRepository.Verify(repo => repo.Delete(venta), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetVentasByProducto_WithValidProductoId_ShouldReturnVentas()
        {
            // Arrange
            var ventas = new List<Venta>
            {
                new Venta { Id = 1, Fecha = DateTime.Now, Monto = 1000, ProductoId = 1 },
                new Venta { Id = 2, Fecha = DateTime.Now, Monto = 2000, ProductoId = 1 }
            };

            _mockVentaRepository.Setup(repo => repo.GetVentasByProductoAsync(1))
                .ReturnsAsync(ventas);

            // Act
            var result = await _ventaService.GetVentasByProductoAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, v => Assert.Equal(1, v.ProductoId));
        }

        [Fact]
        public async Task GetVentasByDateRange_WithValidDates_ShouldReturnVentas()
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(-7);
            var endDate = DateTime.Now;
            var ventas = new List<Venta>
            {
                new Venta { Id = 1, Fecha = DateTime.Now.AddDays(-5), Monto = 1000, ProductoId = 1 },
                new Venta { Id = 2, Fecha = DateTime.Now.AddDays(-3), Monto = 2000, ProductoId = 2 }
            };

            _mockVentaRepository.Setup(repo => repo.GetVentasByDateRangeAsync(startDate, endDate))
                .ReturnsAsync(ventas);

            // Act
            var result = await _ventaService.GetVentasByDateRangeAsync(startDate, endDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, v => Assert.True(v.Fecha >= startDate && v.Fecha <= endDate));
        }
    }
} 