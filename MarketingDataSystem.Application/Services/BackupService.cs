using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using MarketingDataSystem.Application.Interfaces;

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Servicio de backups automáticos para SQL Server.
    /// Cumple con el principio de responsabilidad única (S de SOLID).
    /// </summary>
    public class BackupService : IBackupService
    {
        private readonly ILogger<BackupService> _logger;
        private readonly string _connectionString;
        private readonly string _backupDirectory;
        private readonly string _databaseName;

        public BackupService(
            ILogger<BackupService> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string no configurada");
            
            _backupDirectory = configuration["Backup:Directory"] ?? Path.Combine(Directory.GetCurrentDirectory(), "backups");
            
            // Extraer nombre de la base de datos del connection string
            var builder = new SqlConnectionStringBuilder(_connectionString);
            _databaseName = builder.InitialCatalog ?? "MarketingDataSystem";

            // Crear directorio de backups si no existe
            Directory.CreateDirectory(_backupDirectory);
        }

        /// <summary>
        /// Crea un backup completo de la base de datos
        /// </summary>
        public async Task<string> CrearBackupCompletoAsync(string? backupPath = null)
        {
            try
            {
                var fileName = $"{_databaseName}_Full_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
                var fullPath = backupPath ?? Path.Combine(_backupDirectory, fileName);

                var sql = $@"
                    BACKUP DATABASE [{_databaseName}] 
                    TO DISK = @BackupPath
                    WITH FORMAT, INIT, 
                    NAME = '{_databaseName} Full Backup', 
                    COMPRESSION,
                    CHECKSUM";

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@BackupPath", fullPath);
                command.CommandTimeout = 300; // 5 minutos timeout

                _logger.LogInformation("Iniciando backup completo de {Database} a {Path}", _databaseName, fullPath);
                
                await command.ExecuteNonQueryAsync();
                
                _logger.LogInformation("Backup completo creado exitosamente: {Path}", fullPath);
                return fullPath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear backup completo");
                throw;
            }
        }

        /// <summary>
        /// Crea un backup incremental (diferencial)
        /// </summary>
        public async Task<string> CrearBackupIncrementalAsync(string? backupPath = null)
        {
            try
            {
                var fileName = $"{_databaseName}_Diff_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
                var fullPath = backupPath ?? Path.Combine(_backupDirectory, fileName);

                var sql = $@"
                    BACKUP DATABASE [{_databaseName}] 
                    TO DISK = @BackupPath
                    WITH DIFFERENTIAL, FORMAT, INIT,
                    NAME = '{_databaseName} Differential Backup',
                    COMPRESSION,
                    CHECKSUM";

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@BackupPath", fullPath);
                command.CommandTimeout = 300;

                _logger.LogInformation("Iniciando backup incremental de {Database} a {Path}", _databaseName, fullPath);
                
                await command.ExecuteNonQueryAsync();
                
                _logger.LogInformation("Backup incremental creado exitosamente: {Path}", fullPath);
                return fullPath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear backup incremental");
                throw;
            }
        }

        /// <summary>
        /// Restaura la base de datos desde un backup (PELIGROSO - Solo para desarrollo)
        /// </summary>
        public async Task<bool> RestaurarBackupAsync(string backupFilePath)
        {
            try
            {
                if (!File.Exists(backupFilePath))
                {
                    _logger.LogError("Archivo de backup no encontrado: {Path}", backupFilePath);
                    return false;
                }

                // ADVERTENCIA: Esto es peligroso en producción
                _logger.LogWarning("ADVERTENCIA: Restaurando base de datos desde {Path}. Esto eliminará todos los datos actuales!", backupFilePath);

                var sql = $@"
                    ALTER DATABASE [{_databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    RESTORE DATABASE [{_databaseName}] FROM DISK = @BackupPath WITH REPLACE;
                    ALTER DATABASE [{_databaseName}] SET MULTI_USER;";

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@BackupPath", backupFilePath);
                command.CommandTimeout = 600; // 10 minutos timeout

                await command.ExecuteNonQueryAsync();
                
                _logger.LogInformation("Base de datos restaurada exitosamente desde: {Path}", backupFilePath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al restaurar backup desde {Path}", backupFilePath);
                return false;
            }
        }

        /// <summary>
        /// Lista todos los backups disponibles en el directorio
        /// </summary>
        public async Task<IEnumerable<string>> ListarBackupsAsync()
        {
            try
            {
                await Task.Delay(1); // Para mantener la signatura async

                if (!Directory.Exists(_backupDirectory))
                {
                    _logger.LogWarning("Directorio de backups no existe: {Directory}", _backupDirectory);
                    return Enumerable.Empty<string>();
                }

                var backupFiles = Directory.GetFiles(_backupDirectory, "*.bak")
                    .OrderByDescending(f => new FileInfo(f).CreationTime)
                    .ToList();

                _logger.LogInformation("Encontrados {Count} archivos de backup", backupFiles.Count);
                return backupFiles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar backups");
                return Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// Elimina backups antiguos según la política de retención
        /// </summary>
        public async Task<int> LimpiarBackupsAntiguosAsync(int diasRetencion = 30)
        {
            try
            {
                await Task.Delay(1); // Para mantener la signatura async

                if (!Directory.Exists(_backupDirectory))
                {
                    return 0;
                }

                var fechaLimite = DateTime.Now.AddDays(-diasRetencion);
                var archivosEliminados = 0;

                var backupFiles = Directory.GetFiles(_backupDirectory, "*.bak");

                foreach (var archivo in backupFiles)
                {
                    var fileInfo = new FileInfo(archivo);
                    if (fileInfo.CreationTime < fechaLimite)
                    {
                        try
                        {
                            File.Delete(archivo);
                            archivosEliminados++;
                            _logger.LogInformation("Backup antiguo eliminado: {File}", archivo);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "No se pudo eliminar backup: {File}", archivo);
                        }
                    }
                }

                _logger.LogInformation("Limpieza de backups completada. Eliminados: {Count}", archivosEliminados);
                return archivosEliminados;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en limpieza de backups");
                return 0;
            }
        }
    }
} 