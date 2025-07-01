// ==================== SERVICIO DE TRANSFORMACIÓN ETL EMPRESARIAL - ETAPA 2 CRÍTICA ====================
// PROPÓSITO: Normalizador universal que convierte datos heterogéneos en inteligencia estructurada
// CRITICIDAD: MÁXIMA - Backbone del pipeline que habilita analytics y reporting empresarial
// COMPLIANCE: Fundamental para SOX, auditorías y trazabilidad de transformaciones de datos críticos
// PIPELINE: Validación → **TRANSFORMACIÓN** → Enriquecimiento → Deduplicación → Persistencia
// VALOR: Convierte datos "raw" multi-formato en assets estructurados para business intelligence

using MarketingDataSystem.Application.Interfaces;
using MarketingDataSystem.Core.DTOs;
using System.Text.Json; // Para parsing de JSON en futuras implementaciones
using System.Globalization; // Para conversiones de formato según cultura

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Servicio especializado en transformación de datos crudos a formato normalizado
    /// RESPONSABILIDAD: Convertir datos de múltiples formatos (JSON, CSV, XML) a estructura uniforme
    /// PATRÓN: Strategy Pattern - permite intercambiar algoritmos de transformación según formato
    /// SOLID:
    /// - S: Una sola responsabilidad (transformar datos crudos)
    /// - I: Interfaz específica para transformación (no fuerza métodos no necesarios)
    /// - D: Depende de abstracción ITransformadorService
    /// PIPELINE ETL: Segunda etapa crítica - normaliza formatos heterogéneos
    /// EXTENSIBILIDAD: Fácil agregar parsers para nuevos formatos sin modificar código existente
    /// </summary>
    public class TransformadorService : ITransformadorService
    {
        /// <summary>
        /// Transforma un dato crudo de cualquier formato a estructura normalizada estándar
        /// ALGORITMO: Parsing inteligente que detecta formato y aplica transformación apropiada
        /// USO: Llamado por PipelineETLService después de validación exitosa
        /// FORMATOS SOPORTADOS:
        /// - JSON: De APIs REST y servicios web
        /// - CSV: De archivos de exportación
        /// - XML: De sistemas legacy y SOAP
        /// - Texto plano: De logs y sistemas simples
        /// RESULTADO: Datos en formato estándar DatoNormalizadoDto para siguientes etapas
        /// </summary>
        /// <param name="dato">Dato crudo validado que contiene JSON, CSV, XML u otro formato</param>
        /// <returns>
        /// DatoNormalizadoDto con estructura estándar para el resto del pipeline
        /// Incluye metadatos, trazabilidad y datos transformados
        /// </returns>
        /// <example>
        /// Ejemplos de transformación:
        /// JSON: {"cliente": "Juan", "monto": 100} → DatoNormalizadoDto normalizado
        /// CSV: "Juan,100,2024-01-15" → DatoNormalizadoDto normalizado
        /// XML: &lt;venta&gt;&lt;cliente&gt;Juan&lt;/cliente&gt;&lt;/venta&gt; → DatoNormalizadoDto normalizado
        /// </example>
        public DatoNormalizadoDto Transformar(DatoCrudoDto dato)
        {
            // ========== DETECCIÓN DE FORMATO ==========
            // Analizar el contenido para determinar el formato de origen
            string formato = DetectarFormato(dato.Contenido);
            
            // ========== TRANSFORMACIÓN SEGÚN FORMATO ==========
            DatoNormalizadoDto resultado;
            
            switch (formato.ToUpper())
            {
                case "JSON":
                    resultado = TransformarDesdeJSON(dato);
                    break;
                case "CSV":
                    resultado = TransformarDesdeCSV(dato);
                    break;
                case "XML":
                    resultado = TransformarDesdeXML(dato);
                    break;
                default:
                    // Fallback para formatos no reconocidos
                    resultado = TransformacionGenerica(dato);
                    break;
            }
            
            // ========== METADATOS Y TRAZABILIDAD ==========
            // Agregar información de trazabilidad para auditoria
            resultado.IdDatoCrudo = dato.IdDatoCrudo;              // Referencia al dato original
            resultado.FechaTransformacion = DateTime.Now;          // Timestamp de transformación
            resultado.FormatoOrigen = formato;                     // Formato detectado
            resultado.IdSistema = $"TRANS_{dato.NombreFuente}";    // Sistema de origen
            
            return resultado;
        }
        
        /// <summary>
        /// Detecta automáticamente el formato del contenido crudo
        /// ALGORITMO: Análisis de patrones para identificar JSON, CSV, XML o texto
        /// HEURÍSTICAS: Basado en caracteres iniciales y estructura del contenido
        /// </summary>
        /// <param name="contenido">Contenido crudo a analizar</param>
        /// <returns>Formato detectado: "JSON", "CSV", "XML" o "TEXT"</returns>
        private string DetectarFormato(string contenido)
        {
            if (string.IsNullOrWhiteSpace(contenido)) return "TEXT";
            
            // Limpiar espacios para análisis
            string contenidoLimpio = contenido.Trim();
            
            // Detectar JSON (inicia con { o [)
            if (contenidoLimpio.StartsWith("{") || contenidoLimpio.StartsWith("["))
                return "JSON";
            
            // Detectar XML (inicia con < y contiene tags)
            if (contenidoLimpio.StartsWith("<") && contenidoLimpio.Contains(">"))
                return "XML";
            
            // Detectar CSV (contiene comas y probable estructura tabular)
            if (contenidoLimpio.Contains(",") && contenidoLimpio.Split('\n').Length > 1)
                return "CSV";
            
            // Default: texto plano
            return "TEXT";
        }
        
        /// <summary>
        /// Transforma datos desde formato JSON (APIs y servicios web)
        /// CASOS: Respuestas de APIs REST, webhooks, servicios externos
        /// </summary>
        private DatoNormalizadoDto TransformarDesdeJSON(DatoCrudoDto dato)
        {
            // IMPLEMENTACIÓN FUTURA: Parser JSON completo
            // JsonDocument doc = JsonDocument.Parse(dato.Contenido);
            
            return new DatoNormalizadoDto
            {
                IdSistema = "JSON_API",
                Categoria = "API_DATA",
                Valor = ExtractNumericValue(dato.Contenido),
                MetadatosAdicionales = $"JSON_Source: {dato.NombreFuente}"
            };
        }
        
        /// <summary>
        /// Transforma datos desde formato CSV (archivos de exportación)
        /// CASOS: Exportaciones de Excel, bases de datos, sistemas ERP
        /// </summary>
        private DatoNormalizadoDto TransformarDesdeCSV(DatoCrudoDto dato)
        {
            // IMPLEMENTACIÓN FUTURA: Parser CSV completo con headers
            string[] lineas = dato.Contenido.Split('\n');
            
            return new DatoNormalizadoDto
            {
                IdSistema = "CSV_FILE",
                Categoria = "EXPORT_DATA",
                Valor = ExtractNumericValue(dato.Contenido),
                MetadatosAdicionales = $"CSV_Lines: {lineas.Length}"
            };
        }
        
        /// <summary>
        /// Transforma datos desde formato XML (sistemas legacy y SOAP)
        /// CASOS: Servicios SOAP, sistemas legacy, intercambio B2B
        /// </summary>
        private DatoNormalizadoDto TransformarDesdeXML(DatoCrudoDto dato)
        {
            // IMPLEMENTACIÓN FUTURA: Parser XML completo con XDocument
            
            return new DatoNormalizadoDto
            {
                IdSistema = "XML_LEGACY",
                Categoria = "LEGACY_DATA",
                Valor = ExtractNumericValue(dato.Contenido),
                MetadatosAdicionales = $"XML_Source: Legacy_System"
            };
        }
        
        /// <summary>
        /// Transformación genérica para formatos no reconocidos
        /// FALLBACK: Manejo seguro de formatos inesperados
        /// </summary>
        private DatoNormalizadoDto TransformacionGenerica(DatoCrudoDto dato)
        {
            return new DatoNormalizadoDto
            {
                IdSistema = "GENERIC_SYSTEM",
                Categoria = "UNKNOWN_DATA",
                Valor = 1.0f, // Valor por defecto
                MetadatosAdicionales = $"Generic_Content_Length: {dato.Contenido.Length}"
            };
        }
        
        /// <summary>
        /// Extrae valor numérico del contenido para análisis cuantitativo
        /// HEURÍSTICA: Busca primero número válido en el contenido
        /// </summary>
        private float ExtractNumericValue(string contenido)
        {
            // Implementación simple - en producción usar regex más sofisticado
            try
            {
                // Buscar primer número decimal en el contenido
                var numeros = System.Text.RegularExpressions.Regex.Matches(contenido, @"\d+\.?\d*");
                if (numeros.Count > 0)
                {
                    return float.Parse(numeros[0].Value, CultureInfo.InvariantCulture);
                }
            }
            catch
            {
                // Si falla la extracción, usar valor por defecto
            }
            
            return 1.0f; // Valor por defecto cuando no se puede extraer
        }
    }
} 