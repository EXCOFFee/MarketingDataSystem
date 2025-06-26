using System;
using MarketingDataSystem.Application.Interfaces;

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Servicio de logging/auditoría.
    /// Cumple con S e I de SOLID y el patrón Strategy.
    /// </summary>
    public class LoggerService : ILoggerService
    {
        public void LogInfo(string mensaje)
        {
            Console.WriteLine($"[INFO] {mensaje}");
        }
        public void LogWarning(string mensaje)
        {
            Console.WriteLine($"[WARN] {mensaje}");
        }
        public void LogError(string mensaje)
        {
            Console.WriteLine($"[ERROR] {mensaje}");
        }
    }
} 