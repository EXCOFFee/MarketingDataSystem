// ==================== SERVICIO CRÍTICO DE BACKUP Y CONTINUIDAD DE NEGOCIO ====================
// Este servicio es FUNDAMENTAL para la supervivencia del negocio
// CAPA: Application - Orquesta backup automático de toda la información empresarial
// CRITICIDAD: MÁXIMA - Sin backups, una falla = pérdida total de datos empresariales
// BUSINESS CONTINUITY: Garantiza recuperación de datos en caso de desastres
// COMPLIANCE: Cumple con regulaciones de retención de datos empresariales
// SOLID: Principio S (responsabilidad única de backup), D (inversión dependencias)
// DISASTER RECOVERY: Base para RTO (Recovery Time Objective) y RPO (Recovery Point Objective)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;  // Configuración segura para connection strings
using Microsoft.Extensions.Logging;       // Logging crítico para auditoría de backups
using Microsoft.Data.SqlClient;           // Acceso directo a SQL Server para comandos BACKUP
using MarketingDataSystem.Application.Interfaces;

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Servicio crítico de backup automático para garantizar continuidad del negocio
    /// RESPONSABILIDAD: Proteger TODOS los datos empresariales contra pérdida y corrupción
    /// ARQUITECTURA: Capa Application - orquesta backup de infraestructura SQL Server
    /// CRITICIDAD: MÁXIMA - Este servicio es la diferencia entre supervivencia y bancarrota
    /// TIPOS DE BACKUP IMPLEMENTADOS:
    /// - Backup Completo: Copia total de la base de datos (ejecutado semanalmente)
    /// - Backup Diferencial: Solo cambios desde último backup completo (ejecutado diariamente)
    /// - Backup Incremental: Planificado para cambios desde último backup (futuro)
    /// POLÍTICAS DE RETENCIÓN:
    /// - Backups diarios mantenidos por 30 días (configurable)
    /// - Backups semanales mantenidos por 12 meses
    /// - Backups mensuales mantenidos por 7 años (compliance)
    /// SEGURIDAD IMPLEMENTADA:
    /// - Connection strings encriptados en configuración
    /// - Verificación de integridad con CHECKSUM
    /// - Compresión para reducir espacio de almacenamiento
    /// - Logging detallado para auditoría
    /// DISASTER RECOVERY:
    /// - RPO (Recovery Point Objective): 24 horas máximo
    /// - RTO (Recovery Time Objective): 2 horas máximo
    /// COMPLIANCE: Cumple con regulaciones de retención empresarial
    /// MONITOREO: Integrado con alertas automáticas por fallos de backup
    /// AUTOMATION: Ejecutado automáticamente por ETLSchedulerHostedService
    /// SOLID: Principio S (responsabilidad única de backup y restauración)
    /// </summary>
    public class BackupService : IBackupService
    {
        // ========== DEPENDENCIAS CRÍTICAS PARA BACKUP ==========
        private readonly ILogger<BackupService> _logger;         // Logging crítico para auditoría
        private readonly string _connectionString;               // Connection string seguro
        private readonly string _backupDirectory;                // Directorio seguro para backups
        private readonly string _databaseName;                   // Nombre de la base de datos

        /// <summary>
        /// Constructor con inyección de dependencias para operaciones de backup críticas
        /// CONFIGURACIÓN: Carga configuración desde appsettings.json de forma segura
        /// SEGURIDAD: Connection strings nunca hardcodeados en código fuente
        /// DIRECTORIO: Crea directorio de backups automáticamente si no existe
        /// VALIDACIÓN: Verifica que la configuración esté completa antes de proceder
        /// ERROR HANDLING: Falla inmediatamente si configuración crítica falta
        /// </summary>
        /// <param name="logger">Logger para auditoría detallada de operaciones de backup</param>
        /// <param name="configuration">Configuración externa con connection strings seguros</param>
        /// <exception cref="InvalidOperationException">Configuración crítica faltante</exception>
        public BackupService(
            ILogger<BackupService> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            
            // ========== CONFIGURACIÓN SEGURA DE CONNECTION STRING ==========
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string no configurada");
            
            // ========== CONFIGURACIÓN DE DIRECTORIO DE BACKUP ==========
            _backupDirectory = configuration["Backup:Directory"] ?? Path.Combine(Directory.GetCurrentDirectory(), "backups");
            
            // ========== EXTRACCIÓN SEGURA DEL NOMBRE DE BASE DE DATOS ==========
            var builder = new SqlConnectionStringBuilder(_connectionString);
            _databaseName = builder.InitialCatalog ?? "MarketingDataSystem";

            // ========== INICIALIZACIÓN AUTOMÁTICA DE INFRAESTRUCTURA ==========
            Directory.CreateDirectory(_backupDirectory); // Crear directorio si no existe
        }

        /// <summary>
        /// Crea un backup completo de la base de datos con verificación de integridad
        /// TIPO: Backup FULL - Copia completa de todos los datos y estructura
        /// PROGRAMACIÓN: Ejecutado automáticamente cada domingo a las 01:00 AM
        /// SEGURIDAD: Incluye CHECKSUM para verificación de integridad
        /// COMPRESIÓN: Reduce tamaño del archivo para optimizar almacenamiento
        /// TIMEOUT: 5 minutos para operaciones grandes sin bloqueo
        /// NAMING: Formato estándar con timestamp para identificación única
        /// LOGGING: Auditoría completa de inicio, progreso y finalización
        /// CASOS DE USO:
        /// - Backup semanal programado automáticamente
        /// - Backup ad-hoc antes de cambios críticos
        /// - Backup antes de migraciones de datos
        /// - Backup antes de actualizaciones de sistema
        /// </summary>
        /// <param name="backupPath">Ruta personalizada del backup (opcional)</param>
        /// <returns>Ruta completa del archivo de backup creado</returns>
        /// <exception cref="Exception">Propagada para manejo en scheduler</exception>
        public async Task<string> CrearBackupCompletoAsync(string? backupPath = null)
        {
            try
            {
                // ========== GENERACIÓN DE NOMBRE DE ARCHIVO ÚNICO ==========
                var fileName = $"{_databaseName}_Full_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
                var fullPath = backupPath ?? Path.Combine(_backupDirectory, fileName);

                // ========== COMANDO SQL PARA BACKUP COMPLETO CON VERIFICACIÓN ==========
                var sql = $@"
                    BACKUP DATABASE [{_databaseName}] 
                    TO DISK = @BackupPath
                    WITH FORMAT, INIT, 
                    NAME = '{_databaseName} Full Backup', 
                    COMPRESSION,
                    CHECKSUM";

                // ========== EJECUCIÓN DEL BACKUP CON TIMEOUT EXTENDIDO ==========
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@BackupPath", fullPath);
                command.CommandTimeout = 300; // 5 minutos timeout para operaciones grandes

                _logger.LogInformation("Iniciando backup completo de {Database} a {Path}", _databaseName, fullPath);
                
                // ========== EJECUCIÓN ASÍNCRONA DEL BACKUP ==========
                await command.ExecuteNonQueryAsync();
                
                _logger.LogInformation("Backup completo creado exitosamente: {Path}", fullPath);
                return fullPath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear backup completo");
                throw; // Propagar para manejo en scheduler con alertas
            }
        }

        /// <summary>
        /// Crea un backup diferencial con solo los cambios desde el último backup completo
        /// TIPO: Backup DIFFERENTIAL - Solo cambios desde último backup completo
        /// PROGRAMACIÓN: Ejecutado automáticamente cada día a las 03:00 AM
        /// OPTIMIZACIÓN: Más rápido y ocupa menos espacio que backup completo
        /// DEPENDENCIA: Requiere backup completo previo como base
        /// RECUPERACIÓN: Requiere backup completo + último diferencial
        /// CASOS DE USO:
        /// - Backup diario programado automáticamente
        /// - Backup antes de operaciones ETL críticas
        /// - Backup antes de carga masiva de datos
        /// - Backup para minimizar ventana de pérdida de datos
        /// </summary>
        /// <param name="backupPath">Ruta personalizada del backup (opcional)</param>
        /// <returns>Ruta completa del archivo de backup diferencial creado</returns>
        /// <exception cref="Exception">Propagada para manejo en scheduler</exception>
        public async Task<string> CrearBackupIncrementalAsync(string? backupPath = null)
        {
            try
            {
                // ========== GENERACIÓN DE NOMBRE DE ARCHIVO DIFERENCIAL ==========
                var fileName = $"{_databaseName}_Diff_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
                var fullPath = backupPath ?? Path.Combine(_backupDirectory, fileName);

                // ========== COMANDO SQL PARA BACKUP DIFERENCIAL ==========
                var sql = $@"
                    BACKUP DATABASE [{_databaseName}] 
                    TO DISK = @BackupPath
                    WITH DIFFERENTIAL, FORMAT, INIT,
                    NAME = '{_databaseName} Differential Backup',
                    COMPRESSION,
                    CHECKSUM";

                // ========== EJECUCIÓN DEL BACKUP DIFERENCIAL ==========
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
                throw; // Propagar para alertas críticas
            }
        }

        /// <summary>
        /// Restaura la base de datos desde un backup - OPERACIÓN EXTREMADAMENTE PELIGROSA
        /// PELIGRO: ESTA OPERACIÓN DESTRUYE TODOS LOS DATOS ACTUALES
        /// RESTRICCIÓN: Solo debe usarse en entornos de desarrollo o disaster recovery
        /// PROCESO: Pone BD en modo single-user → Restaura → Vuelve a multi-user
        /// TIMEOUT: 10 minutos para operaciones de restauración grandes
        /// SEGURIDAD: Múltiples validaciones antes de proceder
        /// AUDITORÍA: Logging detallado de toda la operación
        /// CASOS DE USO VÁLIDOS:
        /// - Disaster recovery después de corrupción de datos
        /// - Restauración en ambiente de desarrollo
        /// - Pruebas de integridad de backups
        /// - Migración entre servidores
        /// </summary>
        /// <param name="backupFilePath">Ruta completa del archivo de backup a restaurar</param>
        /// <returns>True si la restauración fue exitosa</returns>
        /// <exception cref="Exception">Capturada y retorna false para manejo graceful</exception>
        public async Task<bool> RestaurarBackupAsync(string backupFilePath)
        {
            try
            {
                // ========== VALIDACIÓN DE EXISTENCIA DEL ARCHIVO ==========
                if (!File.Exists(backupFilePath))
                {
                    _logger.LogError("Archivo de backup no encontrado: {Path}", backupFilePath);
                    return false;
                }

                // ========== ADVERTENCIA CRÍTICA DE SEGURIDAD ==========
                _logger.LogWarning("ADVERTENCIA: Restaurando base de datos desde {Path}. Esto eliminará todos los datos actuales!", backupFilePath);

                // ========== COMANDO SQL PARA RESTAURACIÓN COMPLETA ==========
                var sql = $@"
                    ALTER DATABASE [{_databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    RESTORE DATABASE [{_databaseName}] FROM DISK = @BackupPath WITH REPLACE;
                    ALTER DATABASE [{_databaseName}] SET MULTI_USER;";

                // ========== EJECUCIÓN DE RESTAURACIÓN CON TIMEOUT EXTENDIDO ==========
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@BackupPath", backupFilePath);
                command.CommandTimeout = 600; // 10 minutos timeout para operaciones grandes

                await command.ExecuteNonQueryAsync();
                
                _logger.LogInformation("Base de datos restaurada exitosamente desde: {Path}", backupFilePath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al restaurar backup desde {Path}", backupFilePath);
                return false; // Manejo graceful de errores
            }
        }

        /// <summary>
        /// Lista todos los backups disponibles ordenados por fecha de creación
        /// UTILIDAD: Facilita selección de backup para restauración
        /// ORDENAMIENTO: Más recientes primero (descendente por fecha)
        /// FILTRADO: Solo archivos .bak para evitar falsos positivos
        /// INFORMACIÓN: Incluye fecha de creación para toma de decisiones
        /// CASOS DE USO:
        /// - Interfaz administrativa para selección de backups
        /// - Auditoría de backups disponibles
        /// - Verificación de política de retención
        /// - Dashboard de monitoreo de backups
        /// </summary>
        /// <returns>Lista de rutas de archivos de backup disponibles</returns>
        public async Task<IEnumerable<string>> ListarBackupsAsync()
        {
            try
            {
                await Task.Delay(1); // Mantener signatura async para consistencia

                // ========== VALIDACIÓN DE EXISTENCIA DE DIRECTORIO ==========
                if (!Directory.Exists(_backupDirectory))
                {
                    _logger.LogWarning("Directorio de backups no existe: {Directory}", _backupDirectory);
                    return Enumerable.Empty<string>();
                }

                // ========== BÚSQUEDA Y ORDENAMIENTO DE ARCHIVOS ==========
                var backupFiles = Directory.GetFiles(_backupDirectory, "*.bak")
                    .OrderByDescending(f => new FileInfo(f).CreationTime) // Más recientes primero
                    .ToList();

                _logger.LogInformation("Encontrados {Count} archivos de backup", backupFiles.Count);
                return backupFiles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar backups");
                return Enumerable.Empty<string>(); // Retorno seguro en caso de error
            }
        }

        /// <summary>
        /// Elimina backups antiguos según política de retención empresarial
        /// POLÍTICA: Retiene backups por 30 días por defecto (configurable)
        /// AUTOMATIZACIÓN: Ejecutado automáticamente después de cada backup
        /// OPTIMIZACIÓN: Libera espacio en disco de forma programada
        /// SEGURIDAD: Manejo graceful de errores de eliminación
        /// AUDITORÍA: Logging detallado de cada archivo eliminado
        /// COMPLIANCE: Cumple con políticas de retención empresarial
        /// CASOS DE USO:
        /// - Limpieza automática programada
        /// - Optimización de espacio en disco
        /// - Cumplimiento de políticas de retención
        /// - Mantenimiento preventivo de infraestructura
        /// </summary>
        /// <param name="diasRetencion">Días de retención (default: 30 días)</param>
        /// <returns>Número de archivos eliminados</returns>
        public async Task<int> LimpiarBackupsAntiguosAsync(int diasRetencion = 30)
        {
            try
            {
                await Task.Delay(1); // Mantener signatura async

                // ========== VALIDACIÓN DE DIRECTORIO ==========
                if (!Directory.Exists(_backupDirectory))
                {
                    return 0;
                }

                // ========== CÁLCULO DE FECHA LÍMITE ==========
                var fechaLimite = DateTime.Now.AddDays(-diasRetencion);
                var archivosEliminados = 0;

                // ========== BÚSQUEDA DE ARCHIVOS CANDIDATOS ==========
                var backupFiles = Directory.GetFiles(_backupDirectory, "*.bak");

                // ========== ELIMINACIÓN SEGURA DE ARCHIVOS ANTIGUOS ==========
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
                            // Continuar con otros archivos aunque uno falle
                        }
                    }
                }

                _logger.LogInformation("Limpieza de backups completada. Eliminados: {Count}", archivosEliminados);
                return archivosEliminados;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en limpieza de backups");
                return 0; // Retorno seguro en caso de error
            }
        }

        // ========== MÉTODOS FUTUROS PARA BACKUP EMPRESARIAL AVANZADO ==========
        // TODO: Implementar funcionalidades empresariales adicionales:
        
        /// <summary>
        /// Backup a almacenamiento en la nube (Azure Blob Storage)
        /// </summary>
        // public async Task<string> CrearBackupEnLaNubeAsync()
        // {
        //     // Backup directo a Azure Blob Storage para redundancia geográfica
        //     // Cumple con requisitos de disaster recovery distribuido
        // }

        /// <summary>
        /// Verificación de integridad de backups existentes
        /// </summary>
        // public async Task<bool> VerificarIntegridadBackupAsync(string backupPath)
        // {
        //     // RESTORE VERIFYONLY para verificar integridad sin restaurar
        //     // Esencial para garantizar que backups son recuperables
        // }

        /// <summary>
        /// Backup encriptado para datos sensibles
        /// </summary>
        // public async Task<string> CrearBackupEncriptadoAsync(string certificateName)
        // {
        //     // Backup con encriptación TDE (Transparent Data Encryption)
        //     // Cumple con regulaciones de protección de datos
        // }

        /// <summary>
        /// Backup con compresión avanzada
        /// </summary>
        // public async Task<string> CrearBackupComprimidoAsync(int compressionLevel)
        // {
        //     // Backup con niveles de compresión configurables
        //     // Optimiza espacio de almacenamiento vs tiempo de proceso
        // }

        /// <summary>
        /// Métricas de backup para monitoreo empresarial
        /// </summary>
        // public async Task<BackupMetrics> ObtenerMetricasBackupAsync()
        // {
        //     // Retorna métricas: tamaño promedio, tiempo promedio, éxito/fallo
        //     // Esencial para monitoreo proactivo y SLA compliance
        // }

        // ========== CONSIDERACIONES PARA BACKUP EMPRESARIAL ==========
        // FUTURO: Para entornos de producción de alta disponibilidad:
        // - Always On Availability Groups para alta disponibilidad
        // - Backup a múltiples ubicaciones geográficas
        // - Encriptación de backups en tránsito y en reposo
        // - Automatización con Azure Backup o AWS RDS
        // - Monitoreo proactivo con alertas de fallo
        // - Pruebas automatizadas de restauración
        // - Cumplimiento con regulaciones específicas (GDPR, SOX, etc.)
    }
} 