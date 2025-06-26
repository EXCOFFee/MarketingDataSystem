namespace MarketingDataSystem.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de logging/auditoría.
    /// Cumple con el principio de segregación de interfaces (I de SOLID) y el patrón Strategy.
    /// </summary>
    public interface ILoggerService
    {
        void LogInfo(string mensaje);
        void LogWarning(string mensaje);
        void LogError(string mensaje);
    }
} 