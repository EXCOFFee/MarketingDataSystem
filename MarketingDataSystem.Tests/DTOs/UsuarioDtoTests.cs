using Xunit;
using MarketingDataSystem.Core.DTOs;
using System.ComponentModel.DataAnnotations;

namespace MarketingDataSystem.Tests.DTOs
{
    public class UsuarioDtoTests
    {
        [Fact]
        public void UsuarioDto_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Nombre = "Test User",
                Email = "test@example.com",
                Rol = "Vendedor"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(usuarioDto, new ValidationContext(usuarioDto), validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Fact]
        public void UsuarioDto_WithInvalidEmail_ShouldFailValidation()
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Nombre = "Test User",
                Email = "invalid-email",
                Rol = "Vendedor"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(usuarioDto, new ValidationContext(usuarioDto), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(validationResults);
        }

        [Fact]
        public void UsuarioDto_WithEmptyNombre_ShouldFailValidation()
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Nombre = "",
                Email = "test@example.com",
                Rol = "Vendedor"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(usuarioDto, new ValidationContext(usuarioDto), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(validationResults);
        }

        [Fact]
        public void UsuarioDto_WithInvalidRol_ShouldFailValidation()
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Nombre = "Test User",
                Email = "test@example.com",
                Rol = "InvalidRole"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(usuarioDto, new ValidationContext(usuarioDto), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(validationResults);
        }

        [Theory]
        [InlineData("Vendedor")]
        [InlineData("Marketing")]
        [InlineData("Admin")]
        public void UsuarioDto_WithValidRol_ShouldPassValidation(string rol)
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Nombre = "Test User",
                Email = "test@example.com",
                Rol = rol
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(usuarioDto, new ValidationContext(usuarioDto), validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }
    }
} 