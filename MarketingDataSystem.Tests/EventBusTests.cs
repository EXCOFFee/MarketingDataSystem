using Xunit;
using MarketingDataSystem.Application.Services;
using MarketingDataSystem.Application.Interfaces;

namespace MarketingDataSystem.Tests
{
    public class EventBusTests
    {
        private readonly IEventBus _eventBus;
        private int _handlerExecutionCount;

        public EventBusTests()
        {
            _eventBus = new EventBus();
            _handlerExecutionCount = 0;
        }

        [Fact]
        public void Suscribir_ConEventoYHandler_DebePermitirSuscripcion()
        {
            // Arrange
            var eventoNombre = "TestEvent";
            var handlerEjecutado = false;

            // Act
            _eventBus.Suscribir(eventoNombre, () => handlerEjecutado = true);
            _eventBus.Publicar(eventoNombre);

            // Assert
            Assert.True(handlerEjecutado);
        }

        [Fact]
        public void Publicar_ConEventoSinSuscriptores_NoDebeGenerarError()
        {
            // Arrange
            var eventoNombre = "EventoSinSuscriptores";

            // Act & Assert - No debe lanzar excepción
            _eventBus.Publicar(eventoNombre);
        }

        [Fact]
        public void Suscribir_MultiplesHandlersAlMismoEvento_DebeEjecutarTodos()
        {
            // Arrange
            var eventoNombre = "EventoMultiple";
            var contador1 = 0;
            var contador2 = 0;
            var contador3 = 0;

            // Act
            _eventBus.Suscribir(eventoNombre, () => contador1++);
            _eventBus.Suscribir(eventoNombre, () => contador2++);
            _eventBus.Suscribir(eventoNombre, () => contador3++);
            
            _eventBus.Publicar(eventoNombre);

            // Assert
            Assert.Equal(1, contador1);
            Assert.Equal(1, contador2);
            Assert.Equal(1, contador3);
        }

        [Fact]
        public void Publicar_CargaFinalizada_DebeActivarGeneradorReporte()
        {
            // Arrange
            var reporteGenerado = false;
            var eventoNombre = "CargaFinalizada";

            // Act
            _eventBus.Suscribir(eventoNombre, () => reporteGenerado = true);
            _eventBus.Publicar(eventoNombre);

            // Assert
            Assert.True(reporteGenerado);
        }

        [Fact]
        public void EventBus_ConMultiplesEventosDiferentes_DebeGestionarlosIndependientemente()
        {
            // Arrange
            var evento1Ejecutado = false;
            var evento2Ejecutado = false;

            // Act
            _eventBus.Suscribir("Evento1", () => evento1Ejecutado = true);
            _eventBus.Suscribir("Evento2", () => evento2Ejecutado = true);

            _eventBus.Publicar("Evento1");

            // Assert
            Assert.True(evento1Ejecutado);
            Assert.False(evento2Ejecutado); // No debe ejecutarse
        }

        [Fact]
        public void EventBus_PatronObserver_DebeDesacoplarPublicadorDeSuscriptor()
        {
            // Arrange
            var mensajeRecibido = "";
            var eventoNombre = "NotificacionSistema";

            // Act - Simulamos que un servicio se suscribe a notificaciones
            _eventBus.Suscribir(eventoNombre, () => mensajeRecibido = "Evento procesado");
            
            // Otro componente publica el evento sin conocer quién está suscrito
            _eventBus.Publicar(eventoNombre);

            // Assert
            Assert.Equal("Evento procesado", mensajeRecibido);
        }
    }
} 