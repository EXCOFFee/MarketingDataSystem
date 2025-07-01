using Xunit;
using System.Linq;
using System.Collections.Generic;
using MarketingDataSystem.Application.Services;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Tests
{
    public class EnriquecedorServiceTests
    {
        private readonly EnriquecedorService _service;

        public EnriquecedorServiceTests()
        {
            _service = new EnriquecedorService();
        }

        [Fact]
        public void Constructor_SinParametros_DebeCrearInstancia()
        {
            // Assert
            Assert.NotNull(_service);
        }

        [Fact]
        public void Enriquecer_ConDatos_DebeRetornarDatosEnriquecidos()
        {
            // Arrange
            var datos = new List<DatoNormalizadoDto>
            {
                new() { IdSistema = "ID001", Contenido = "Dato original", Fuente = "Test" }
            };

            // Act
            var resultado = _service.Enriquecer(datos).ToList();

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Contains("enriched_metadata", resultado[0].Contenido);
        }

        [Fact]
        public void Enriquecer_ConListaVacia_DebeRetornarVacia()
        {
            // Arrange
            var datos = new List<DatoNormalizadoDto>();

            // Act
            var resultado = _service.Enriquecer(datos).ToList();

            // Assert
            Assert.Empty(resultado);
        }

        [Fact]
        public void Enriquecer_ConDatosCustomer_DebeAgregarMetadata()
        {
            // Arrange
            var datos = new List<DatoNormalizadoDto>
            {
                new() { 
                    IdSistema = "CUST001", 
                    Contenido = "{\"email\": \"test@example.com\", \"company\": \"Test Corp\"}", 
                    Fuente = "CRM" 
                }
            };

            // Act
            var resultado = _service.Enriquecer(datos).ToList();

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);
            // Verificar que se agregó metadata de enriquecimiento
            Assert.Contains("enriched_metadata", resultado[0].Contenido);
        }
    }
}
