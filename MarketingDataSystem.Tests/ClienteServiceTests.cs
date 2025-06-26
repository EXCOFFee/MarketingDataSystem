using Xunit;
using Moq;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using MarketingDataSystem.Application.Services;
using MarketingDataSystem.Core.DTOs;
using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;

public class ClienteServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ClienteService _service;

    public ClienteServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _service = new ClienteService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsClienteDto_WhenClienteExists()
    {
        var cliente = new Cliente { IdCliente = 1, Nombre = "Juan", Email = "juan@mail.com" };
        var clienteDto = new ClienteDto { IdCliente = 1, Nombre = "Juan", Email = "juan@mail.com" };
        _unitOfWorkMock.Setup(u => u.Clientes.GetByIdAsync(1)).ReturnsAsync(cliente);
        _mapperMock.Setup(m => m.Map<ClienteDto>(cliente)).Returns(clienteDto);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Juan", result.Nombre);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllClientes()
    {
        var clientes = new List<Cliente> { new Cliente { IdCliente = 1, Nombre = "Juan" } };
        var clientesDto = new List<ClienteDto> { new ClienteDto { IdCliente = 1, Nombre = "Juan" } };
        _unitOfWorkMock.Setup(u => u.Clientes.GetAllAsync()).ReturnsAsync(clientes);
        _mapperMock.Setup(m => m.Map<IEnumerable<ClienteDto>>(clientes)).Returns(clientesDto);

        var result = await _service.GetAllAsync();

        Assert.Single(result);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedClienteDto()
    {
        var clienteDto = new ClienteDto { Nombre = "Nuevo" };
        var cliente = new Cliente { Nombre = "Nuevo" };
        _mapperMock.Setup(m => m.Map<Cliente>(clienteDto)).Returns(cliente);
        _mapperMock.Setup(m => m.Map<ClienteDto>(cliente)).Returns(clienteDto);
        _unitOfWorkMock.Setup(u => u.Clientes.AddAsync(cliente)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _service.CreateAsync(clienteDto);

        Assert.Equal("Nuevo", result.Nombre);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedClienteDto()
    {
        var clienteDto = new ClienteDto { IdCliente = 1, Nombre = "Actualizado" };
        var cliente = new Cliente { IdCliente = 1, Nombre = "Actualizado" };
        _mapperMock.Setup(m => m.Map<Cliente>(clienteDto)).Returns(cliente);
        _mapperMock.Setup(m => m.Map<ClienteDto>(cliente)).Returns(clienteDto);
        _unitOfWorkMock.Setup(u => u.Clientes.Update(cliente));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _service.UpdateAsync(clienteDto);

        Assert.Equal("Actualizado", result.Nombre);
    }

    [Fact]
    public async Task DeleteAsync_RemovesCliente()
    {
        var cliente = new Cliente { IdCliente = 1 };
        _unitOfWorkMock.Setup(u => u.Clientes.GetByIdAsync(1)).ReturnsAsync(cliente);
        _unitOfWorkMock.Setup(u => u.Clientes.Delete(cliente));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        await _service.DeleteAsync(1);
        _unitOfWorkMock.Verify(u => u.Clientes.Delete(cliente), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
} 