using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MarketingDataSystem.Application.Services;
using MarketingDataSystem.Application.Interfaces;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Tests
{
    public class PipelineETLServiceTests
    {
        private readonly Mock<IValidadorService> _validadorMock;
        private readonly Mock<ITransformadorService> _transformadorMock;
        private readonly Mock<IEnriquecedorService> _enriquecedorMock;
        private readonly Mock<IDeduplicadorService> _deduplicadorMock;
        private readonly Mock<ILoggerService> _loggerMock;
        private readonly Mock<IEventBus> _eventBusMock;
        private readonly PipelineETLService _pipelineService;

        public PipelineETLServiceTests()
        {
            _validadorMock = new Mock<IValidadorService>();
            _transformadorMock = new Mock<ITransformadorService>();
            _enriquecedorMock = new Mock<IEnriquecedorService>();
            _deduplicadorMock = new Mock<IDeduplicadorService>();
            _loggerMock = new Mock<ILoggerService>();
            _eventBusMock = new Mock<IEventBus>();

            _pipelineService = new PipelineETLService(
                _validadorMock.Object,
                _transformadorMock.Object,
                _enriquecedorMock.Object,
                _deduplicadorMock.Object,
                _loggerMock.Object,
                _eventBusMock.Object
            );
        }

        [Fact]
        public void Procesar_ConDatosValidos_DebeRetornarDatosProcesados()
        {
            // Arrange
            var datosCrudos = new List<DatoCrudoDto>
            {
                new DatoCrudoDto { Contenido = "dato1", Timestamp = DateTime.Now },
                new DatoCrudoDto { Contenido = "dato2", Timestamp = DateTime.Now }
            };

            var datosNormalizados = new List<DatoNormalizadoDto>
            {
                new DatoNormalizadoDto { IdSistema = "SYS1", Categoria = "CAT1" },
                new DatoNormalizadoDto { IdSistema = "SYS2", Categoria = "CAT2" }
            };

            _validadorMock.Setup(v => v.Validar(It.IsAny<DatoCrudoDto>())).Returns(true);
            _transformadorMock.Setup(t => t.Transformar(It.IsAny<DatoCrudoDto>()))
                .Returns((DatoCrudoDto d) => new DatoNormalizadoDto { IdSistema = "TRANS" });
            _enriquecedorMock.Setup(e => e.Enriquecer(It.IsAny<IEnumerable<DatoNormalizadoDto>>()))
                .Returns(datosNormalizados);
            _deduplicadorMock.Setup(d => d.Deduplicar(It.IsAny<IEnumerable<DatoNormalizadoDto>>()))
                .Returns(datosNormalizados);

            // Act
            var resultado = _pipelineService.Procesar(datosCrudos);

            // Assert
            Assert.Equal(2, resultado.Count());
            _validadorMock.Verify(v => v.Validar(It.IsAny<DatoCrudoDto>()), Times.Exactly(2));
            _transformadorMock.Verify(t => t.Transformar(It.IsAny<DatoCrudoDto>()), Times.Exactly(2));
            _enriquecedorMock.Verify(e => e.Enriquecer(It.IsAny<IEnumerable<DatoNormalizadoDto>>()), Times.Once);
            _deduplicadorMock.Verify(d => d.Deduplicar(It.IsAny<IEnumerable<DatoNormalizadoDto>>()), Times.Once);
        }

        [Fact]
        public void Procesar_ConDatosInvalidos_DebeFiltrarDatosInvalidos()
        {
            // Arrange
            var datosCrudos = new List<DatoCrudoDto>
            {
                new DatoCrudoDto { Contenido = "valido", Timestamp = DateTime.Now },
                new DatoCrudoDto { Contenido = "", Timestamp = DateTime.Now }, // Inválido
                new DatoCrudoDto { Contenido = "valido2", Timestamp = DateTime.Now }
            };

            _validadorMock.Setup(v => v.Validar(It.Is<DatoCrudoDto>(d => !string.IsNullOrEmpty(d.Contenido)))).Returns(true);
            _validadorMock.Setup(v => v.Validar(It.Is<DatoCrudoDto>(d => string.IsNullOrEmpty(d.Contenido)))).Returns(false);

            // Act
            var resultado = _pipelineService.Procesar(datosCrudos);

            // Assert
            _validadorMock.Verify(v => v.Validar(It.IsAny<DatoCrudoDto>()), Times.Exactly(3));
            _transformadorMock.Verify(t => t.Transformar(It.IsAny<DatoCrudoDto>()), Times.Exactly(2)); // Solo los válidos
        }

        [Fact]
        public async Task EjecutarETLAsync_ConProcesoExitoso_DebeEmitirEventoCargaFinalizada()
        {
            // Arrange
            _validadorMock.Setup(v => v.Validar(It.IsAny<DatoCrudoDto>())).Returns(true);
            _transformadorMock.Setup(t => t.Transformar(It.IsAny<DatoCrudoDto>()))
                .Returns(new DatoNormalizadoDto { IdSistema = "TEST" });
            _enriquecedorMock.Setup(e => e.Enriquecer(It.IsAny<IEnumerable<DatoNormalizadoDto>>()))
                .Returns(new List<DatoNormalizadoDto> { new DatoNormalizadoDto() });
            _deduplicadorMock.Setup(d => d.Deduplicar(It.IsAny<IEnumerable<DatoNormalizadoDto>>()))
                .Returns(new List<DatoNormalizadoDto> { new DatoNormalizadoDto() });

            // Act
            await _pipelineService.EjecutarETLAsync();

            // Assert
            _eventBusMock.Verify(e => e.Publicar("CargaFinalizada"), Times.Once);
            _loggerMock.Verify(l => l.LogInfo("Proceso ETL completado exitosamente"), Times.Once);
        }

        [Fact]
        public async Task EjecutarETLAsync_ConError_DebeLoguearErrorYRelanzarExcepcion()
        {
            // Arrange
            _validadorMock.Setup(v => v.Validar(It.IsAny<DatoCrudoDto>()))
                .Throws(new Exception("Error de validación"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _pipelineService.EjecutarETLAsync());
            Assert.Contains("Error de validación", exception.Message);
            _loggerMock.Verify(l => l.LogError(It.IsAny<string>()), Times.Once);
            _eventBusMock.Verify(e => e.Publicar("CargaFinalizada"), Times.Never);
        }
    }
} 