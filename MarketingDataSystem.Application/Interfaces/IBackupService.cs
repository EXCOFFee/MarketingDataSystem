using System.Threading.Tasks;

namespace MarketingDataSystem.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de backups automáticos.
    /// Cumple con el principio de inversión de dependencias (D de SOLID).
    /// </summary>
    public interface IBackupService
    {
        /// <summary>
        /// Crea un backup completo de la base de datos
        /// </summary>
        /// <param name="backupPath">Ruta donde guardar el backup</param>
        /// <returns>Ruta del archivo de backup creado</returns>
        Task<string> CrearBackupCompletoAsync(string? backupPath = null);

        /// <summary>
        /// Crea un backup incremental
        /// </summary>
        /// <param name="backupPath">Ruta donde guardar el backup</param>
        /// <returns>Ruta del archivo de backup creado</returns>
        Task<string> CrearBackupIncrementalAsync(string? backupPath = null);

        /// <summary>
        /// Restaura la base de datos desde un backup
        /// </summary>
        /// <param name="backupFilePath">Ruta del archivo de backup</param>
        /// <returns>True si la restauración fue exitosa</returns>
        Task<bool> RestaurarBackupAsync(string backupFilePath);

        /// <summary>
        /// Lista todos los backups disponibles
        /// </summary>
        /// <returns>Lista de rutas de archivos de backup</returns>
        Task<IEnumerable<string>> ListarBackupsAsync();

        /// <summary>
        /// Elimina backups antiguos según la política de retención
        /// </summary>
        /// <param name="diasRetencion">Días a mantener los backups</param>
        /// <returns>Número de backups eliminados</returns>
        Task<int> LimpiarBackupsAntiguosAsync(int diasRetencion = 30);
    }
} 