using System;

namespace MarketingDataSystem.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el EventBus interno.
    /// Cumple con el principio abierto/cerrado (O de SOLID) y el patrón Observer/EventBus.
    /// </summary>
    public interface IEventBus
    {
        void Suscribir(string evento, Action handler);
        void Publicar(string evento);
    }
} 