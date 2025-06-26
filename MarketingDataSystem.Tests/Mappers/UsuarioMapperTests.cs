using Xunit;
using MarketingDataSystem.Application.Mappers;
using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Tests.Mappers
{
    public class UsuarioMapperTests
    {
        [Fact]
        public void ToDTO_WithValidEntity_ShouldReturnDto()
        {
            // Arrange
            var usuario = new UsuarioMarketing
            {
                Id = 1,
                Nombre = "Test User",
                Email = "test@example.com",
                Role = "Vendedor"
            };

            // Act
            var result = UsuarioMapper.ToDTO(usuario);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(usuario.Id, result.Id);
            Assert.Equal(usuario.Nombre, result.Nombre);
            Assert.Equal(usuario.Email, result.Email);
            Assert.Equal(usuario.Role, result.Role);
        }

        [Fact]
        public void ToEntity_WithValidDto_ShouldReturnEntity()
        {
            // Arrange
            var usuarioDto = new UsuarioMarketingDto
            {
                Id = 1,
                Nombre = "Test User",
                Email = "test@example.com",
                Role = "Vendedor"
            };

            // Act
            var result = UsuarioMapper.ToEntity(usuarioDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(usuarioDto.Id, result.Id);
            Assert.Equal(usuarioDto.Nombre, result.Nombre);
            Assert.Equal(usuarioDto.Email, result.Email);
            Assert.Equal(usuarioDto.Role, result.Role);
        }

        [Fact]
        public void UpdateEntity_WithValidDto_ShouldUpdateEntity()
        {
            // Arrange
            var usuario = new UsuarioMarketing
            {
                Id = 1,
                Nombre = "Original Name",
                Email = "original@example.com",
                Role = "Marketing"
            };

            var usuarioDto = new UsuarioMarketingDto
            {
                Id = 1,
                Nombre = "Updated Name",
                Email = "updated@example.com",
                Role = "Vendedor"
            };

            // Act
            UsuarioMapper.UpdateEntity(usuario, usuarioDto);

            // Assert
            Assert.Equal(usuarioDto.Nombre, usuario.Nombre);
            Assert.Equal(usuarioDto.Email, usuario.Email);
            Assert.Equal(usuarioDto.Role, usuario.Role);
        }

        [Fact]
        public void UpdateEntity_WithNullEntity_ShouldNotThrow()
        {
            // Arrange
            UsuarioMarketing usuario = null;
            var usuarioDto = new UsuarioMarketingDto
            {
                Id = 1,
                Nombre = "Test User",
                Email = "test@example.com",
                Role = "Vendedor"
            };

            // Act & Assert
            var exception = Record.Exception(() => UsuarioMapper.UpdateEntity(usuario, usuarioDto));
            Assert.Null(exception);
        }

        [Fact]
        public void UpdateEntity_WithNullDto_ShouldNotThrow()
        {
            // Arrange
            var usuario = new UsuarioMarketing
            {
                Id = 1,
                Nombre = "Test User",
                Email = "test@example.com",
                Role = "Vendedor"
            };
            UsuarioMarketingDto usuarioDto = null;

            // Act & Assert
            var exception = Record.Exception(() => UsuarioMapper.UpdateEntity(usuario, usuarioDto));
            Assert.Null(exception);
        }
    }
} 