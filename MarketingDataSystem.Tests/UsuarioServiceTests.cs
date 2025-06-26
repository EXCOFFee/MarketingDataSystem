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
    public class UsuarioServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUsuarioMarketingRepository> _mockUsuarioRepository;
        private readonly UsuarioMarketingService _usuarioService;

        public UsuarioServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUsuarioRepository = new Mock<IUsuarioMarketingRepository>();

            _mockUnitOfWork.Setup(uow => uow.UsuariosMarketing).Returns(_mockUsuarioRepository.Object);

            _usuarioService = new UsuarioMarketingService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetAllUsuarios_ShouldReturnAllUsuarios()
        {
            // Arrange
            var usuarios = new List<UsuarioMarketing>
            {
                new UsuarioMarketing { Id = 1, Nombre = "Usuario1", Email = "usuario1@test.com", Rol = "Vendedor" },
                new UsuarioMarketing { Id = 2, Nombre = "Usuario2", Email = "usuario2@test.com", Rol = "Marketing" }
            };

            _mockUsuarioRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(usuarios);

            // Act
            var result = await _usuarioService.GetAllUsuariosAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(usuarios[0].Id, result.First().Id);
            Assert.Equal(usuarios[1].Id, result.Last().Id);
        }

        [Fact]
        public async Task GetUsuarioById_WithValidId_ShouldReturnUsuario()
        {
            // Arrange
            var usuario = new Usuario { Id = 1, Nombre = "Usuario1", Email = "usuario1@test.com", Rol = "Vendedor" };
            _mockUsuarioRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(usuario);

            // Act
            var result = await _usuarioService.GetUsuarioByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(usuario.Id, result.Id);
            Assert.Equal(usuario.Nombre, result.Nombre);
            Assert.Equal(usuario.Email, result.Email);
        }

        [Fact]
        public async Task CreateUsuario_WithValidData_ShouldCreateUsuario()
        {
            // Arrange
            var usuarioDto = new UsuarioMarketingDto
            {
                Nombre = "Nuevo Usuario",
                Email = "nuevo@test.com",
                Rol = "Vendedor"
            };

            _mockUsuarioRepository.Setup(repo => repo.AddAsync(It.IsAny<UsuarioMarketing>()))
                .ReturnsAsync((UsuarioMarketing u) => u);

            // Act
            var result = await _usuarioService.CreateUsuarioAsync(usuarioDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(usuarioDto.Nombre, result.Nombre);
            Assert.Equal(usuarioDto.Email, result.Email);
            Assert.Equal(usuarioDto.Rol, result.Rol);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUsuario_WithValidData_ShouldUpdateUsuario()
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Id = 1,
                Nombre = "Usuario Actualizado",
                Email = "actualizado@test.com",
                Rol = "Marketing"
            };

            var existingUsuario = new Usuario { Id = 1, Nombre = "Usuario Original", Email = "original@test.com", Rol = "Vendedor" };
            _mockUsuarioRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(existingUsuario);

            // Act
            var result = await _usuarioService.UpdateUsuarioAsync(usuarioDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(usuarioDto.Nombre, result.Nombre);
            Assert.Equal(usuarioDto.Email, result.Email);
            Assert.Equal(usuarioDto.Rol, result.Rol);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUsuario_WithValidId_ShouldDeleteUsuario()
        {
            // Arrange
            var usuario = new Usuario { Id = 1, Nombre = "Usuario1", Email = "usuario1@test.com", Rol = "Vendedor" };
            _mockUsuarioRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(usuario);

            // Act
            await _usuarioService.DeleteUsuarioAsync(1);

            // Assert
            _mockUsuarioRepository.Verify(repo => repo.Delete(usuario), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetUsuariosByRol_WithValidRol_ShouldReturnUsuarios()
        {
            // Arrange
            var usuarios = new List<Usuario>
            {
                new Usuario { Id = 1, Nombre = "Usuario1", Email = "usuario1@test.com", Rol = "Vendedor" },
                new Usuario { Id = 2, Nombre = "Usuario2", Email = "usuario2@test.com", Rol = "Vendedor" }
            };

            _mockUsuarioRepository.Setup(repo => repo.GetUsuariosByRolAsync("Vendedor"))
                .ReturnsAsync(usuarios);

            // Act
            var result = await _usuarioService.GetUsuariosByRolAsync("Vendedor");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, u => Assert.Equal("Vendedor", u.Rol));
        }

        [Fact]
        public async Task GetUsuarioByEmail_WithValidEmail_ShouldReturnUsuario()
        {
            // Arrange
            var usuario = new Usuario { Id = 1, Nombre = "Usuario1", Email = "usuario1@test.com", Rol = "Vendedor" };
            _mockUsuarioRepository.Setup(repo => repo.GetUsuarioByEmailAsync("usuario1@test.com"))
                .ReturnsAsync(usuario);

            // Act
            var result = await _usuarioService.GetUsuarioByEmailAsync("usuario1@test.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(usuario.Email, result.Email);
            Assert.Equal(usuario.Nombre, result.Nombre);
        }
    }
} 