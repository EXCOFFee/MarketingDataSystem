using Xunit;
using MarketingDataSystem.Core.DTOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace MarketingDataSystem.Tests.DTOs
{
    public class VentaDtoTests
    {
        [Fact]
        public void VentaDto_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var ventaDto = new VentaDto
            {
                Fecha = DateTime.Now,
                Monto = 1000,
                ProductoId = 1
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(ventaDto, new ValidationContext(ventaDto), validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Fact]
        public void VentaDto_WithNegativeMonto_ShouldFailValidation()
        {
            // Arrange
            var ventaDto = new VentaDto
            {
                Fecha = DateTime.Now,
                Monto = -1000,
                ProductoId = 1
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(ventaDto, new ValidationContext(ventaDto), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(validationResults);
        }

        [Fact]
        public void VentaDto_WithZeroProductoId_ShouldFailValidation()
        {
            // Arrange
            var ventaDto = new VentaDto
            {
                Fecha = DateTime.Now,
                Monto = 1000,
                ProductoId = 0
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(ventaDto, new ValidationContext(ventaDto), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(validationResults);
        }

        [Fact]
        public void VentaDto_WithFutureDate_ShouldFailValidation()
        {
            // Arrange
            var ventaDto = new VentaDto
            {
                Fecha = DateTime.Now.AddDays(1),
                Monto = 1000,
                ProductoId = 1
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(ventaDto, new ValidationContext(ventaDto), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(validationResults);
        }
    }
} 