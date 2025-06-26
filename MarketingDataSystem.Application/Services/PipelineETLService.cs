// ==================== SERVICIO ORQUESTADOR PIPELINE ETL ====================
// Este servicio es el corazón del sistema de procesamiento de datos
// PATRÓN: Orquestador/Facade - coordina múltiples servicios para ejecutar el pipeline completo
// SOLID: Cumple principios S (responsabilidad única), D (inversión dependencias), O (abierto/cerrado)
// PIPELINE: Extract (recolectar) → Transform (transformar) → Load (cargar)

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketingDataSystem.Application.Interfaces;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Servicio orquestador principal del pipeline ETL (Extract, Transform, Load)
    /// RESPONSABILIDAD: Coordinar la ejecución secuencial de todas las etapas del procesamiento de datos
    /// PATRÓN: Facade - Simplifica la interacción con múltiples servicios complejos
    /// ARQUITECTURA: Sigue principios SOLID para máxima testabilidad y mantenibilidad
    /// EJECUCIÓN: Se ejecuta automáticamente cada día a las 02:00 AM vía ETLSchedulerHostedService
    /// </summary>
    public class PipelineETLService
    {
        // ========== DEPENDENCIAS DEL PIPELINE ETL ==========
        // Todas las dependencias son inyectadas siguiendo principio de Inversión de Dependencias (SOLID)
        private readonly IValidadorService _validador;           // Etapa 1: Validación de datos crudos
        private readonly ITransformadorService _transformador;   // Etapa 2: Transformación de formatos
        private readonly IEnriquecedorService _enriquecedor;     // Etapa 3: Enriquecimiento con datos externos
        private readonly IDeduplicadorService _deduplicador;     // Etapa 4: Eliminación de duplicados
        private readonly ILoggerService _logger;                 // Logging especializado para ETL
        private readonly IEventBus _eventBus;                    // Comunicación por eventos (patrón Publisher/Subscriber)

        /// <summary>
        /// Constructor con inyección de dependencias múltiples
        /// PATRÓN: Dependency Injection - todas las dependencias son interfaces (testeable)
        /// SOLID: Principio D - depende de abstracciones, no de implementaciones concretas
        /// </summary>
        /// <param name="validador">Servicio para validar datos crudos según reglas de negocio</param>
        /// <param name="transformador">Servicio para transformar formatos (JSON, CSV, XML)</param>
        /// <param name="enriquecedor">Servicio para enriquecer datos con información externa</param>
        /// <param name="deduplicador">Servicio para eliminar registros duplicados</param>
        /// <param name="logger">Logger especializado para procesos ETL</param>
        /// <param name="eventBus">Bus de eventos para comunicación desacoplada</param>
        public PipelineETLService(
            IValidadorService validador,
            ITransformadorService transformador,
            IEnriquecedorService enriquecedor,
            IDeduplicadorService deduplicador,
            ILoggerService logger,
            IEventBus eventBus)
        {
            _validador = validador;         // Principio D de SOLID - Inversión de dependencias
            _transformador = transformador;
            _enriquecedor = enriquecedor;
            _deduplicador = deduplicador;
            _logger = logger;
            _eventBus = eventBus;
        }

        /// <summary>
        /// Método principal del pipeline ETL - ejecuta las 4 etapas secuencialmente
        /// FLUJO: Datos Crudos → Validación → Transformación → Enriquecimiento → Deduplicación → Datos Finales
        /// PATRÓN: Pipeline Pattern - cada etapa procesa y pasa resultado a la siguiente
        /// FUNCIONAL: Utiliza LINQ para procesamiento funcional (map, filter, reduce)
        /// </summary>
        /// <param name="datosCrudos">Datos sin procesar de múltiples fuentes (JSON, CSV, XML)</param>
        /// <returns>Datos procesados, normalizados y listos para persistencia</returns>
        public IEnumerable<DatoNormalizadoDto> Procesar(IEnumerable<DatoCrudoDto> datosCrudos)
        {
            // ========== INICIO DEL PIPELINE ==========
            _logger.LogInfo("Inicio del pipeline ETL");
            
            // ========== ETAPA 1: VALIDACIÓN ==========
            // Filtrar solo datos válidos según reglas de negocio (formato, campos requeridos, etc.)
            var validos = datosCrudos.Where(_validador.Validar).ToList();
            _logger.LogInfo($"Datos válidos: {validos.Count}");
            
            // ========== ETAPA 2: TRANSFORMACIÓN ==========
            // Convertir datos crudos a formato normalizado estándar
            var normalizados = validos.Select(_transformador.Transformar).ToList();
            
            // ========== ETAPA 3: ENRIQUECIMIENTO ==========
            // Agregar información adicional (datos de productos, geolocalización, etc.)
            var enriquecidos = _enriquecedor.Enriquecer(normalizados).ToList();
            
            // ========== ETAPA 4: DEDUPLICACIÓN ==========
            // Eliminar registros duplicados usando algoritmos de matching
            var deduplicados = _deduplicador.Deduplicar(enriquecidos).ToList();
            
            _logger.LogInfo($"Datos deduplicados: {deduplicados.Count}");
            return deduplicados; // Datos finales listos para base de datos
        }

        /// <summary>
        /// Método principal público para ejecutar todo el proceso ETL completo
        /// LLAMADO POR: ETLSchedulerHostedService automáticamente a las 02:00 AM diariamente
        /// FLUJO COMPLETO: Extract (recolectar) → Transform (procesar) → Load (persistir) → Evento
        /// ASÍNCRONO: Permite operaciones I/O no bloqueantes (APIs, archivos, base de datos)
        /// EVENTOS: Emite 'CargaFinalizada' para disparar generación automática de reportes
        /// </summary>
        /// <returns>Task que representa la operación asíncrona completa</returns>
        /// <exception cref="Exception">Propaga cualquier error del pipeline para manejo en scheduler</exception>
        public async Task EjecutarETLAsync()
        {
            try
            {
                _logger.LogInfo("Iniciando proceso ETL programado");
                
                // ========== FASE 1: EXTRACT (EXTRACCIÓN) ==========
                // Recolectar datos de múltiples fuentes: APIs, CSV, XML, bases de datos
                var datosCrudos = await RecolectarDatosAsync();
                
                // ========== FASE 2: TRANSFORM (TRANSFORMACIÓN) ==========
                // Procesar todos los datos usando el pipeline de 4 etapas
                var datosProcesados = Procesar(datosCrudos);
                
                // ========== FASE 3: LOAD (CARGA) ==========
                // Persistir datos procesados en la base de datos principal
                await PersistirDatosAsync(datosProcesados);
                
                // ========== FASE 4: NOTIFICACIÓN POR EVENTOS ==========
                // Emitir evento para disparar generación automática de reportes Excel
                _eventBus.Publicar("CargaFinalizada");
                
                _logger.LogInfo("Proceso ETL completado exitosamente");
            }
            catch (System.Exception ex)
            {
                // Log del error y re-lanzar para manejo en ETLSchedulerHostedService
                _logger.LogError($"Error en proceso ETL: {ex.Message}");
                throw; // El scheduler manejará reintentos y alertas críticas
            }
        }

        private async Task<IEnumerable<DatoCrudoDto>> RecolectarDatosAsync()
        {
            var datosCrudos = new List<DatoCrudoDto>();
            
            _logger.LogInfo("Iniciando recolección de datos de múltiples fuentes");
            
            try
            {
                // 1. Simular datos de API de sucursal (JSON)
                var datosAPI = await RecolectarDesdeSucursalAPI();
                datosCrudos.AddRange(datosAPI);
                
                // 2. Simular datos de archivo CSV
                var datosCSV = await RecolectarDesdeArchivoCSV();
                datosCrudos.AddRange(datosCSV);
                
                // 3. Simular datos de API pública de productos
                var datosProductos = await RecolectarDesdeAPIProductos();
                datosCrudos.AddRange(datosProductos);
                
                _logger.LogInfo($"Recolectados {datosCrudos.Count} registros de todas las fuentes");
                return datosCrudos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en recolección de datos: {ex.Message}");
                throw;
            }
        }

        private async Task<List<DatoCrudoDto>> RecolectarDesdeSucursalAPI()
        {
            _logger.LogInfo("Recolectando datos de API de sucursal");
            await Task.Delay(500); // Simular llamada API
            
            // Simular respuesta JSON de API de sucursal
            var ventasJSON = @"[
                {""id"": 1, ""fecha"": ""2024-01-15"", ""cliente"": ""Cliente A"", ""producto"": ""PROD001"", ""cantidad"": 5, ""precio"": 100.50},
                {""id"": 2, ""fecha"": ""2024-01-15"", ""cliente"": ""Cliente B"", ""producto"": ""PROD002"", ""cantidad"": 3, ""precio"": 75.25},
                {""id"": 3, ""fecha"": ""2024-01-15"", ""cliente"": ""Cliente C"", ""producto"": ""PROD001"", ""cantidad"": 2, ""precio"": 100.50}
            ]";
            
            return new List<DatoCrudoDto>
            {
                new DatoCrudoDto 
                { 
                    Contenido = ventasJSON, 
                    Timestamp = DateTime.Now, 
                    IdFuente = 1,
                    NombreFuente = "API_JSON_Sucursal"
                }
            };
        }

        private async Task<List<DatoCrudoDto>> RecolectarDesdeArchivoCSV()
        {
            _logger.LogInfo("Recolectando datos de archivo CSV");
            await Task.Delay(200); // Simular lectura de archivo
            
            // Simular contenido CSV
            var csvContent = @"id,fecha,cliente,producto,cantidad,precio
4,2024-01-15,Cliente D,PROD003,1,200.00
5,2024-01-15,Cliente E,PROD002,4,75.25
6,2024-01-15,Cliente F,PROD001,3,100.50";
            
            return new List<DatoCrudoDto>
            {
                new DatoCrudoDto 
                { 
                    Contenido = csvContent, 
                    Timestamp = DateTime.Now, 
                    IdFuente = 2,
                    NombreFuente = "CSV_SistemaLegado"
                }
            };
        }

        private async Task<List<DatoCrudoDto>> RecolectarDesdeAPIProductos()
        {
            _logger.LogInfo("Recolectando datos de API pública de productos");
            await Task.Delay(300); // Simular llamada API externa
            
            // Simular respuesta XML de API de productos
            var xmlContent = @"<?xml version=""1.0""?>
<productos>
    <producto id=""PROD001"" nombre=""Laptop Dell"" categoria=""Electronica"" proveedor=""Dell Inc""/>
    <producto id=""PROD002"" nombre=""Mouse Logitech"" categoria=""Accesorios"" proveedor=""Logitech""/>
    <producto id=""PROD003"" nombre=""Monitor Samsung"" categoria=""Electronica"" proveedor=""Samsung""/>
</productos>";
            
            return new List<DatoCrudoDto>
            {
                new DatoCrudoDto 
                { 
                    Contenido = xmlContent, 
                    Timestamp = DateTime.Now, 
                    IdFuente = 3,
                    NombreFuente = "API_XML_Productos"
                }
            };
        }

        private async Task PersistirDatosAsync(IEnumerable<DatoNormalizadoDto> datos)
        {
            // Implementación simplificada - en producción esto guardaría en la base de datos
            await Task.Delay(100); // Simular operación asíncrona
            _logger.LogInfo($"Persistidos {datos.Count()} registros en la base de datos");
        }
    }
} 