using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using MarketingDataSystem.Application.Services;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Core.DTOs;
using MarketingDataSystem.Core.Entities;
using AutoMapper;

namespace MarketingDataSystem.Tests
{
    public class VentaServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IVentaRepository> _ventaRepositoryMock;
        private readonly Mock<IProductoRepository> _productoRepositoryMock;
        private readonly VentaService _service;

        public VentaServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _ventaRepositoryMock = new Mock<IVentaRepository>();
            _productoRepositoryMock = new Mock<IProductoRepository>();
            
            // Configurar los repositorios en el UnitOfWork
            _unitOfWorkMock.Setup(u => u.Ventas).Returns(_ventaRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Productos).Returns(_productoRepositoryMock.Object);
            
            _service = new VentaService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void Constructor_ConParametrosValidos_DebeCrearInstancia()
        {
            // Assert
            Assert.NotNull(_service);
        }

        [Fact]
        public async Task GetAllAsync_DebeRetornarListaVentas()
        {
            // Arrange
            var ventas = new List<Venta>
            {
                new() { IdVenta = 1, IdProducto = 1, IdCliente = 1, Cantidad = 1, PrecioUnitario = 100 }
            };
            var ventasDto = new List<VentaDto>
            {
                new() { IdVenta = 1, IdProducto = 1, IdCliente = 1, Cantidad = 1, PrecioUnitario = 100 }
            };

            _ventaRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(ventas);
            _mapperMock.Setup(m => m.Map<IEnumerable<VentaDto>>(ventas)).Returns(ventasDto);

            // Act
            var resultado = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);
        }

        [Fact]
        public async Task GetByIdAsync_ConIdValido_DebeRetornarVenta()
        {
            // Arrange
            var venta = new Venta { IdVenta = 1, IdProducto = 1, IdCliente = 1 };
            var ventaDto = new VentaDto { IdVenta = 1, IdProducto = 1, IdCliente = 1 };

            _ventaRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(venta);
            _mapperMock.Setup(m => m.Map<VentaDto>(venta)).Returns(ventaDto);

            // Act
            var resultado = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.IdVenta);
        }
    }
}
