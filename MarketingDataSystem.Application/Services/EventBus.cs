using System;
using System.Collections.Generic;
using MarketingDataSystem.Application.Interfaces;

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Implementación del EventBus interno.
    /// Cumple con el principio abierto/cerrado (O de SOLID) y el patrón Observer/EventBus.
    /// </summary>
    public class EventBus : IEventBus
    {
        private readonly Dictionary<string, List<Action>> _suscriptores = new();

        public void Suscribir(string evento, Action handler)
        {
            if (!_suscriptores.ContainsKey(evento))
                _suscriptores[evento] = new List<Action>();
            _suscriptores[evento].Add(handler);
        }

        public void Publicar(string evento)
        {
            if (_suscriptores.ContainsKey(evento))
            {
                foreach (var handler in _suscriptores[evento])
                {
                    handler();
                }
            }
        }
    }
} 