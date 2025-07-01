using Xunit;
using System.Linq;
using System.Collections.Generic;
using MarketingDataSystem.Application.Services;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Tests
{
    public class DeduplicadorServiceTests
    {
        private readonly DeduplicadorService _service;

        public DeduplicadorServiceTests()
        {
            _service = new DeduplicadorService();
        }

        [Fact]
        public void Constructor_SinParametros_DebeCrearInstancia()
        {
            // Assert
            Assert.NotNull(_service);
        }

        [Fact]
        public void Deduplicar_ConDatosDuplicados_DebeEliminarDuplicados()
        {
            // Arrange
            var datos = new List<DatoNormalizadoDto>
            {
                new() { IdSistema = "ID001", Contenido = "Dato 1" },
                new() { IdSistema = "ID001", Contenido = "Dato 1 duplicado" },
                new() { IdSistema = "ID002", Contenido = "Dato 2" }
            };

            // Act
            var resultado = _service.Deduplicar(datos).ToList();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal("ID001", resultado[0].IdSistema);
            Assert.Equal("ID002", resultado[1].IdSistema);
        }

        [Fact]
        public void Deduplicar_ConDatosUnicos_DebeRetornarTodos()
        {
            // Arrange
            var datos = new List<DatoNormalizadoDto>
            {
                new() { IdSistema = "ID001", Contenido = "Dato 1" },
                new() { IdSistema = "ID002", Contenido = "Dato 2" }
            };

            // Act
            var resultado = _service.Deduplicar(datos).ToList();

            // Assert
            Assert.Equal(2, resultado.Count);
        }

        [Fact]
        public void Deduplicar_ConListaVacia_DebeRetornarVacia()
        {
            // Arrange
            var datos = new List<DatoNormalizadoDto>();

            // Act
            var resultado = _service.Deduplicar(datos).ToList();

            // Assert
            Assert.Empty(resultado);
        }
    }
}
