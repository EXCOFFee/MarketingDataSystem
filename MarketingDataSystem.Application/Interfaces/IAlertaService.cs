using System.Threading.Tasks;

namespace MarketingDataSystem.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de alertas del sistema.
    /// Cumple con el principio de inversión de dependencias (D de SOLID).
    /// </summary>
    public interface IAlertaService
    {
        /// <summary>
        /// Envía una alerta crítica cuando falla el proceso ETL
        /// </summary>
        /// <param name="mensaje">Mensaje de la alerta</param>
        /// <param name="detalleError">Detalle del error ocurrido</param>
        /// <returns>Task que representa la operación asíncrona</returns>
        Task EnviarAlertaCriticaAsync(string mensaje, string detalleError);

        /// <summary>
        /// Envía una alerta de advertencia
        /// </summary>
        /// <param name="mensaje">Mensaje de advertencia</param>
        /// <returns>Task que representa la operación asíncrona</returns>
        Task EnviarAlertaAdvertenciaAsync(string mensaje);

        /// <summary>
        /// Envía una notificación informativa
        /// </summary>
        /// <param name="mensaje">Mensaje informativo</param>
        /// <returns>Task que representa la operación asíncrona</returns>
        Task EnviarNotificacionAsync(string mensaje);
    }
} 