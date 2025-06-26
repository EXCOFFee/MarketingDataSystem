using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using MarketingDataSystem.Application.Services;
using MarketingDataSystem.Core.DTOs;
using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using AutoMapper;

public class ProductoServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ProductoService _service;

    public ProductoServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _service = new ProductoService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsProductoDto_WhenProductoExists()
    {
        var producto = new Producto { IdProducto = 1, Nombre = "Lapicera" };
        var productoDto = new ProductoDto { IdProducto = 1, Nombre = "Lapicera" };
        _unitOfWorkMock.Setup(u => u.Productos.GetByIdAsync(1)).ReturnsAsync(producto);
        _mapperMock.Setup(m => m.Map<ProductoDto>(producto)).Returns(productoDto);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Lapicera", result.Nombre);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllProductos()
    {
        var productos = new List<Producto> { new Producto { IdProducto = 1, Nombre = "Lapicera" } };
        var productosDto = new List<ProductoDto> { new ProductoDto { IdProducto = 1, Nombre = "Lapicera" } };
        _unitOfWorkMock.Setup(u => u.Productos.GetAllAsync()).ReturnsAsync(productos);
        _mapperMock.Setup(m => m.Map<IEnumerable<ProductoDto>>(productos)).Returns(productosDto);

        var result = await _service.GetAllAsync();

        Assert.Single(result);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedProductoDto()
    {
        var productoDto = new ProductoDto { Nombre = "Nuevo" };
        var producto = new Producto { Nombre = "Nuevo" };
        _mapperMock.Setup(m => m.Map<Producto>(productoDto)).Returns(producto);
        _mapperMock.Setup(m => m.Map<ProductoDto>(producto)).Returns(productoDto);
        _unitOfWorkMock.Setup(u => u.Productos.AddAsync(producto)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _service.CreateAsync(productoDto);

        Assert.Equal("Nuevo", result.Nombre);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedProductoDto()
    {
        var productoDto = new ProductoDto { IdProducto = 1, Nombre = "Actualizado" };
        var producto = new Producto { IdProducto = 1, Nombre = "Actualizado" };
        _mapperMock.Setup(m => m.Map<Producto>(productoDto)).Returns(producto);
        _mapperMock.Setup(m => m.Map<ProductoDto>(producto)).Returns(productoDto);
        _unitOfWorkMock.Setup(u => u.Productos.Update(producto));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _service.UpdateAsync(productoDto);

        Assert.Equal("Actualizado", result.Nombre);
    }

    [Fact]
    public async Task DeleteAsync_RemovesProducto()
    {
        var producto = new Producto { IdProducto = 1 };
        _unitOfWorkMock.Setup(u => u.Productos.GetByIdAsync(1)).ReturnsAsync(producto);
        _unitOfWorkMock.Setup(u => u.Productos.Delete(producto));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        await _service.DeleteAsync(1);
        _unitOfWorkMock.Verify(u => u.Productos.Delete(producto), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
} 