// ==================== DATO NORMALIZADO DTO EMPRESARIAL - ETL CORE ====================
// PROPÓSITO: Contenedor de datos procesados y normalizados del pipeline ETL empresarial
// CRITICIDAD: ALTA - Formato estándar para datos en etapas 3-5 del pipeline ETL
// COMPLIANCE: Essential para audit trails y trazabilidad de transformaciones
// VALOR: Estructura unificada que habilita analytics y business intelligence

using System;

namespace MarketingDataSystem.Core.DTOs
{
    /// <summary>
    /// DTO que representa un dato normalizado en el pipeline ETL empresarial
    /// PROPÓSITO: Contenedor estándar para datos procesados, validados y normalizados
    /// ETAPA ETL: Utilizado en etapas 2-5 (Transformación  Enriquecimiento  Deduplicación  Persistencia)
    /// CASOS DE USO:
    /// - Transporte de datos entre servicios ETL
    /// - Audit trail de transformaciones aplicadas
    /// - Base para enriquecimiento y deduplicación
    /// - Input para generación de reportes
    /// </summary>
    public class DatoNormalizadoDto
    {
        /// <summary>
        /// Identificador único del dato normalizado
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Identificador del sistema origen para trazabilidad
        /// </summary>
        public string IdSistema { get; set; } = string.Empty;
        
        /// <summary>
        /// Categoría del dato para clasificación empresarial
        /// </summary>
        public string Categoria { get; set; } = string.Empty;
        
        /// <summary>
        /// Valor numérico del dato para analytics
        /// </summary>
        public float Valor { get; set; }
        
        /// <summary>
        /// Relación con el dato crudo original para audit trail
        /// </summary>
        public int IdDatoCrudo { get; set; }
        
        /// <summary>
        /// Contenido del dato normalizado (puede ser JSON, texto, etc.)
        /// Necesario para EnriquecedorService y otros servicios ETL
        /// </summary>
        public string Contenido { get; set; } = string.Empty;
        
        /// <summary>
        /// Fuente del dato para trazabilidad y audit trails
        /// </summary>
        public string Fuente { get; set; } = string.Empty;
        
        /// <summary>
        /// Fecha de procesamiento para temporal tracking
        /// </summary>
        public DateTime FechaProcesamiento { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Fecha de transformación en el pipeline ETL
        /// </summary>
        public DateTime FechaTransformacion { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Formato de origen detectado (JSON, CSV, XML, etc.)
        /// </summary>
        public string FormatoOrigen { get; set; } = string.Empty;
        
        /// <summary>
        /// Metadatos adicionales para governance y audit trails
        /// </summary>
        public string MetadatosAdicionales { get; set; } = string.Empty;

        /// <summary>
        /// Datos normalizados como string - Para tests
        /// </summary>
        public string DatosNormalizados { get; set; } = string.Empty;

        /// <summary>
        /// Fuente de origen del dato - Para tests
        /// </summary>
        public string FuenteOrigen { get; set; } = string.Empty;
    }
}
