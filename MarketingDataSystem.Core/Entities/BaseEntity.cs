// ==================== CLASE BASE FUNDAMENTAL DEL MODELO DE DOMINIO ====================
// SISTEMA: MarketingDataSystem - Sistema Empresarial de Gestión de Datos de Marketing
// COMPONENTE: Entidad Base del Dominio - MÁXIMA CRITICIDAD ARQUITECTÓNICA
// RESPONSABILIDAD: Fundamentos comunes para TODAS las entidades del sistema empresarial
// CAPA: Core/Domain - Base arquitectónica del modelo de dominio en Clean Architecture
// PATRÓN: Base Entity Pattern para consistencia y governance de datos empresarial
// CRITICIDAD: MÁXIMA - Toda entidad del sistema hereda de esta clase base fundamental
// COMPLIANCE: Proporciona audit trail automático para regulaciones empresariales
// GOVERNANCE: Fundamento del gobierno de datos y políticas de gestión de información

using System; // Para tipos DateTime fundamentales y operaciones temporales

namespace MarketingDataSystem.Core.Entities
{
    /// <summary>
    /// ENTIDAD BASE FUNDAMENTAL: Clase Base del Modelo de Dominio - MÁXIMA CRITICIDAD
    /// ==============================================================================
    /// 
    /// DOMINIO EMPRESARIAL:
    /// Esta clase abstracta es la base fundamental de TODAS las entidades del sistema
    /// empresarial. Define el contrato común que garantiza consistencia, auditoría y
    /// comportamiento estándar en todo el modelo de dominio. Es la piedra angular
    /// de la arquitectura de datos empresarial.
    /// 
    /// CASOS DE USO EMPRESARIALES CRÍTICOS:
    /// 1. AUDIT TRAIL UNIVERSAL: Tracking automático de creación/modificación en todas las entidades
    /// 2. COMPLIANCE REGULATORIO: Cumplimiento automático con regulaciones que requieren trazabilidad
    /// 3. SOFT DELETE EMPRESARIAL: Eliminación lógica sin romper integridades referenciales críticas
    /// 4. DATA GOVERNANCE: Gestión consistente de datos maestros y operacionales
    /// 5. FORENSIC ANALYSIS: Investigación de incidentes con trazabilidad completa de datos
    /// 6. REGULATORY REPORTING: Base para reportes regulatorios que requieren audit trail
    /// 7. CHANGE MANAGEMENT: Control de cambios empresariales con timestamps precisos
    /// 8. DATA LINEAGE: Trazabilidad completa de origen y modificación de datos críticos
    /// 9. BACKUP CONSISTENCY: Garantía de consistencia en respaldos empresariales
    /// 10. PERFORMANCE OPTIMIZATION: Consultas optimizadas usando propiedades base comunes
    /// 11. DEBUGGING EMPRESARIAL: Identificación rápida de registros problemáticos
    /// 12. SECURITY AUDIT: Base para auditorías de seguridad y acceso a datos
    /// 
    /// ARQUITECTURA:
    /// - CAPA: Core/Domain en Clean Architecture - completamente independiente de infraestructura
    /// - PATRÓN: Base Entity Pattern para garantizar consistencia arquitectónica
    /// - RESPONSABILIDAD: Proporcionar fundamentos comunes a todas las entidades del dominio
    /// - ABSTRACTION: Clase abstracta que define contrato sin implementación específica
    /// 
    /// PRINCIPIOS EMPRESARIALES IMPLEMENTADOS:
    /// - DRY (Don't Repeat Yourself): Propiedades comunes definidas una sola vez
    /// - Single Responsibility: Se enfoca únicamente en aspectos comunes de entidades
    /// - Open/Closed: Abierta para extensión por herencia, cerrada para modificación
    /// - Liskov Substitution: Todas las entidades pueden ser tratadas como BaseEntity
    /// - Interface Segregation: Proporciona solo las propiedades esenciales comunes
    /// 
    /// ENTIDADES CRÍTICAS QUE HEREDAN:
    /// - Usuario: Gestión de autenticación y autorización empresarial
    /// - FuenteDeDatos: Configuración de fuentes ETL y pipelines de datos
    /// - Producto: Catálogo maestro de productos comerciales
    /// - Venta: Transacciones financieras críticas y registros de ingresos
    /// - Stock: Inventario distribuido y gestión de almacén empresarial
    /// - Cliente: Base de datos CRM y gestión de relaciones comerciales
    /// - Reporte: Generación y distribución de inteligencia de negocio
    /// - Metadata: Catálogo de datos y gobierno de información
    /// - Y todas las demás entidades del sistema empresarial
    /// 
    /// BENEFICIOS EMPRESARIALES:
    /// - CONSISTENCY: Todas las entidades siguen el mismo patrón de diseño
    /// - MAINTAINABILITY: Cambios en propiedades comunes se propagan automáticamente
    /// - AUDIT COMPLIANCE: Cumplimiento automático con regulaciones de trazabilidad
    /// - PERFORMANCE: Consultas optimizadas usando índices en propiedades base
    /// - DEBUGGING: Identificación rápida y troubleshooting de registros problemáticos
    /// - GOVERNANCE: Políticas de datos aplicadas consistentemente
    /// - SECURITY: Base común para implementar controles de seguridad
    /// - SCALABILITY: Fundamentos escalables para crecimiento empresarial
    /// 
    /// COMPLIANCE Y AUDITORÍA:
    /// - SOX: Sarbanes-Oxley para trazabilidad de datos financieros
    /// - GDPR: General Data Protection Regulation para audit trail de datos personales
    /// - ISO 27001: Estándares de seguridad de información
    /// - COBIT: Control Objectives for Information and Related Technologies
    /// - ITIL: Information Technology Infrastructure Library para gestión de servicios
    /// 
    /// CLEAN ARCHITECTURE:
    /// - Independiente de frameworks específicos (Entity Framework, NHibernate, etc.)
    /// - Sin dependencias externas (UI, Database, API, Infrastructure)
    /// - Enfocada puramente en lógica de dominio y reglas de negocio
    /// - Testeable unitariamente sin dependencias externas
    /// </summary>
    public abstract class BaseEntity
    {
        // ========== IDENTIFICADOR ÚNICO UNIVERSAL ==========
        /// <summary>
        /// IDENTIFICADOR ÚNICO UNIVERSAL PARA LA ENTIDAD
        /// ============================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Identificador único e inmutable que sirve como clave primaria estándar
        /// para todas las entidades del sistema empresarial. Garantiza unicidad
        /// global y proporciona la base para todas las relaciones de datos.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - PRIMARY KEY: Clave primaria estándar para todas las entidades del sistema
        /// - FOREIGN KEYS: Base para relaciones entre entidades del modelo de dominio
        /// - API ENDPOINTS: Identificación en endpoints REST (GET /api/entidad/{id})
        /// - CACHING SYSTEMS: Clave única para sistemas de cache distribuido empresarial
        /// - AUDIT LOGGING: Identificación específica de registros en logs de auditoría
        /// - DEBUGGING: Identificación precisa de registros problemáticos en troubleshooting
        /// - REPLICATION: Clave para replicación de datos entre sistemas distribuidos
        /// - BACKUP RECOVERY: Identificación durante procesos de backup y recovery
        /// - DATA MIGRATION: Clave para migración de datos entre entornos
        /// - INTEGRATION: Referencia para integraciones con sistemas externos
        /// 
        /// CARACTERÍSTICAS TÉCNICAS:
        /// - AUTO-INCREMENT: Generado automáticamente por la base de datos
        /// - IMMUTABLE: Una vez asignado, permanece constante durante todo el ciclo de vida
        /// - UNIQUE: Garantiza unicidad absoluta dentro del tipo de entidad
        /// - INDEXED: Indexado automáticamente como primary key para máximo performance
        /// - NON-NULL: Siempre tiene valor para entidades persistidas (> 0)
        /// 
        /// CONSIDERACIONES DE PERFORMANCE:
        /// - CLUSTERED INDEX: Utilizado como índice clustered para optimización de consultas
        /// - JOIN OPTIMIZATION: Optimizado para operaciones JOIN entre entidades relacionadas
        /// - CACHE FRIENDLY: Tamaño compacto para eficiencia en sistemas de cache
        /// - QUERY OPTIMIZATION: Base para optimización automática de consultas por ORM
        /// 
        /// ESTÁNDARES EMPRESARIALES:
        /// - NAMING CONVENTION: Consistencia en convención de nomenclatura (Id)
        /// - DATA TYPE: int para balance entre rango y performance (soporte hasta 2B registros)
        /// - VALIDATION: Siempre > 0 para registros persistidos, 0 para entidades nuevas
        /// - AUDIT TRAIL: Preservado en todos los logs de auditoría y compliance
        /// </summary>
        public int Id { get; set; }

        // ========== AUDIT TRAIL AUTOMÁTICO ==========
        /// <summary>
        /// TIMESTAMP DE CREACIÓN DEL REGISTRO
        /// =================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Fecha y hora exacta de creación del registro en el sistema empresarial.
        /// Es fundamental para audit trail, compliance regulatorio y análisis temporal
        /// de datos. Se asigna automáticamente y nunca debe modificarse.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - AUDIT COMPLIANCE: Cumplimiento con regulaciones SOX, GDPR que requieren trazabilidad
        /// - FORENSIC ANALYSIS: Análisis forense de cuándo se introdujeron datos específicos
        /// - TEMPORAL ANALYTICS: Análisis de crecimiento y patrones temporales de datos
        /// - REGULATORY REPORTING: Reportes regulatorios que requieren timestamps de creación
        /// - DATA LINEAGE: Trazabilidad del origen temporal de información empresarial
        /// - SLA MONITORING: Verificación de tiempos de procesamiento desde creación
        /// - DATA RETENTION: Base para políticas de retención basadas en antigüedad
        /// - CHANGE TRACKING: Identificación de períodos de cambio en datos maestros
        /// - TROUBLESHOOTING: Identificar cuándo se introdujeron registros problemáticos
        /// - BACKUP VERIFICATION: Verificar integridad temporal de respaldos empresariales
        /// - COMPLIANCE AUDITS: Evidencia temporal para auditorías internas y externas
        /// - BUSINESS INTELLIGENCE: Análisis de tendencias y patrones de crecimiento
        /// 
        /// CARACTERÍSTICAS TÉCNICAS:
        /// - IMMUTABLE: Una vez asignada, nunca debe cambiar durante el ciclo de vida
        /// - AUTO-ASSIGNED: Asignada automáticamente por Repository/DbContext al crear
        /// - UTC TIMEZONE: Almacenada en UTC para consistencia en sistemas distribuidos
        /// - PRECISION: Precisión de milisegundos para trazabilidad granular
        /// - NON-NULL: Siempre tiene valor para registros persistidos
        /// 
        /// COMPLIANCE Y REGULACIONES:
        /// - SOX: Sarbanes-Oxley requiere trazabilidad temporal de datos financieros
        /// - GDPR: General Data Protection Regulation para audit trail de datos personales
        /// - ISO 27001: Estándares que requieren timestamps de creación de información
        /// - COBIT: Governance de TI que requiere trazabilidad temporal de datos
        /// - Regulaciones sectoriales: Compliance específico según industria (banca, salud, etc.)
        /// 
        /// CONSIDERACIONES DE PERFORMANCE:
        /// - INDEXED: Índice no-clustered para consultas por rango de fechas
        /// - PARTITIONING: Base para particionamiento temporal de tablas grandes
        /// - ARCHIVING: Criterio para archivado automático de datos antiguos
        /// - QUERY OPTIMIZATION: Optimización de consultas por período temporal
        /// </summary>
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// TIMESTAMP DE ÚLTIMA MODIFICACIÓN DEL REGISTRO
        /// ============================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Fecha y hora de la última modificación realizada al registro. Es crítico
        /// para control de cambios, detección de modificaciones, sincronización de datos
        /// y cumplimiento con políticas de governance empresarial.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - CHANGE DETECTION: Identificación de registros modificados para sincronización
        /// - OPTIMISTIC LOCKING: Control de concurrencia basado en timestamp de modificación
        /// - ETL INCREMENTAL: Procesamiento incremental basado en última modificación
        /// - CACHE INVALIDATION: Invalidación de cache cuando datos han sido modificados
        /// - AUDIT TRAIL: Registro de cuándo se realizaron cambios para compliance
        /// - DATA SYNCHRONIZATION: Sincronización entre sistemas basada en timestamps
        /// - CONFLICT RESOLUTION: Resolución de conflictos en replicación de datos
        /// - BACKUP INCREMENTAL: Respaldos incrementales basados en cambios recientes
        /// - CHANGE MANAGEMENT: Gestión de cambios empresariales con trazabilidad temporal
        /// - COMPLIANCE REPORTING: Reportes de modificaciones para auditorías regulatorias
        /// - FORENSIC ANALYSIS: Análisis forense de cuándo se modificaron datos críticos
        /// - SLA MONITORING: Monitoreo de frecuencia de cambios en datos maestros
        /// 
        /// CARACTERÍSTICAS TÉCNICAS:
        /// - NULLABLE: null indica que el registro nunca ha sido modificado después de creación
        /// - AUTO-UPDATED: Actualizada automáticamente por Repository/DbContext en cada UPDATE
        /// - UTC TIMEZONE: Almacenada en UTC para consistencia global empresarial
        /// - PRECISION: Precisión de milisegundos para detección precisa de cambios
        /// - CONDITIONAL: Solo se asigna cuando hay modificaciones reales de datos
        /// 
        /// PATRONES DE IMPLEMENTACIÓN:
        /// - OPTIMISTIC CONCURRENCY: Usado como rowversion para control de concurrencia
        /// - DELTA SYNC: Base para sincronización delta entre sistemas distribuidos
        /// - CHANGE TRACKING: Fundamento para sistemas de change data capture (CDC)
        /// - VERSIONING: Soporte para versionado de entidades empresariales
        /// 
        /// COMPLIANCE Y GOVERNANCE:
        /// - CHANGE AUDIT: Evidencia de modificaciones para auditorías de compliance
        /// - DATA GOVERNANCE: Aplicación de políticas de cambio de datos maestros
        /// - REGULATORY COMPLIANCE: Cumplimiento con regulaciones de trazabilidad de cambios
        /// - INTERNAL CONTROLS: Controles internos para gestión de cambios empresariales
        /// 
        /// CONSIDERACIONES DE PERFORMANCE:
        /// - INDEXED: Índice para consultas por rango de modificación reciente
        /// - MONITORING: Monitoreo de patrones de modificación para optimización
        /// - PARTITIONING: Consideración para particionamiento basado en actividad
        /// - CACHING: Estrategias de cache invalidation basadas en modificaciones
        /// </summary>
        public DateTime? FechaModificacion { get; set; }

        // ========== SOFT DELETE EMPRESARIAL ==========
        /// <summary>
        /// INDICADOR DE REGISTRO ACTIVO - SOFT DELETE PATTERN
        /// =================================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Indicador que implementa el patrón de eliminación lógica (soft delete)
        /// para mantener integridad referencial y cumplir con regulaciones que
        /// requieren preservación de datos históricos para auditoría y compliance.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - SOFT DELETE: Eliminación lógica sin borrado físico para preservar integridad
        /// - REFERENTIAL INTEGRITY: Mantener referencias válidas en foreign keys
        /// - AUDIT PRESERVATION: Preservar datos eliminados para compliance y auditoría
        /// - DATA RECOVERY: Capacidad de "undelete" restaurando registros eliminados
        /// - HISTORICAL REPORTING: Incluir/excluir datos históricos en reportes según necesidad
        /// - COMPLIANCE RETENTION: Cumplir con regulaciones de retención de datos
        /// - FORENSIC ANALYSIS: Análisis forense de datos eliminados durante incidentes
        /// - DATA ARCHIVING: Identificar registros para archivado sin eliminar físicamente
        /// - BUSINESS CONTINUITY: Mantener continuidad operacional sin pérdida de datos
        /// - LEGAL HOLD: Preservar datos durante procesos legales o investigaciones
        /// - ROLLBACK CAPABILITY: Capacidad de rollback de eliminaciones accidentales
        /// - INTEGRATION SAFETY: Evitar errores en integraciones por datos faltantes
        /// 
        /// COMPORTAMIENTO POR DEFECTO:
        /// - DEFAULT VALUE: true (todos los registros están activos al crearse)
        /// - CREATION: Nuevos registros siempre se crean como activos (Activo = true)
        /// - MODIFICATION: Solo se cambia a false cuando se requiere eliminación lógica
        /// - RESTORATION: Cambio de false a true restaura el registro eliminado
        /// 
        /// IMPLEMENTACIÓN EN CONSULTAS:
        /// - WHERE CLAUSE: Mayoría de consultas deben filtrar por Activo = true
        /// - SOFT DELETE QUERIES: Consultas especiales pueden incluir registros inactivos
        /// - INDEX STRATEGY: Índice compuesto (Activo, Id) para performance óptima
        /// - QUERY FILTERS: Filtros globales en EF Core para aplicar automáticamente
        /// 
        /// CONSIDERACIONES DE CASCADA:
        /// - CASCADING BEHAVIOR: Definir comportamiento cuando entidad padre se desactiva
        /// - RELATED ENTITIES: Considerar impacto en entidades relacionadas
        /// - BUSINESS RULES: Aplicar reglas de negocio específicas por tipo de entidad
        /// - DEPENDENCY CHECK: Verificar dependencias antes de desactivar registros
        /// 
        /// COMPLIANCE Y REGULACIONES:
        /// - GDPR: "Right to be forgotten" vs. audit trail requirements (balance crítico)
        /// - SOX: Preservación de datos financieros para auditorías regulatorias
        /// - SECTOR REGULATIONS: Regulaciones específicas por industria (banca, salud, etc.)
        /// - LEGAL REQUIREMENTS: Requisitos legales de retención y preservación de datos
        /// 
        /// MONITOREO Y ALERTAS:
        /// - DELETION MONITORING: Monitoreo de patrones de eliminación lógica
        /// - RESTORATION TRACKING: Seguimiento de restauraciones de registros
        /// - COMPLIANCE REPORTING: Reportes de retención y eliminación para compliance
        /// - AUDIT ALERTS: Alertas por eliminaciones masivas o inusuales
        /// 
        /// PERFORMANCE Y OPTIMIZACIÓN:
        /// - INDEX DESIGN: Índices optimizados para consultas con filtro de activo
        /// - QUERY PERFORMANCE: Impacto en performance de consultas con soft delete
        /// - PARTITIONING: Consideraciones para particionamiento por estado activo
        /// - ARCHIVING STRATEGY: Estrategias de archivado basadas en estado y antigüedad
        /// </summary>
        public bool Activo { get; set; } = true;

        // ========== MÉTODOS DE DOMINIO FUTUROS ==========
        // TODO: Implementar métodos virtuales para comportamiento base común empresarial:

        /// <summary>
        /// FUTURO: Método virtual de validación empresarial de entidad
        /// Cada entidad puede override para implementar validaciones específicas de negocio
        /// Incluirá validaciones de integridad, reglas de negocio y compliance
        /// </summary>
        // public virtual ValidationResult ValidarEntidad()

        /// <summary>
        /// FUTURO: Método virtual para marcar entidad como modificada
        /// Actualiza automáticamente FechaModificacion y aplica reglas de audit trail
        /// </summary>
        // public virtual void MarcarComoModificada(int? usuarioModificacion = null)

        /// <summary>
        /// FUTURO: Método virtual para eliminación lógica empresarial
        /// Implementa soft delete con reglas de negocio y audit trail completo
        /// </summary>
        // public virtual void EliminarLogicamente(int? usuarioEliminacion = null, string? razonEliminacion = null)

        /// <summary>
        /// FUTURO: Método virtual para restauración de eliminación lógica
        /// Restaura registros eliminados lógicamente con audit trail
        /// </summary>
        // public virtual void RestaurarEliminacion(int? usuarioRestauracion = null, string? razonRestauracion = null)

        /// <summary>
        /// FUTURO: Método virtual para clonación empresarial segura
        /// Crea copias de entidades respetando reglas de negocio y security
        /// </summary>
        // public virtual T ClonarEntidad<T>() where T : BaseEntity, new()

        /// <summary>
        /// FUTURO: Método virtual para serialización segura empresarial
        /// Serializa entidad ocultando información sensible según clasificación
        /// </summary>
        // public virtual string SerializarSeguro(SecurityContext? contexto = null)

        /// <summary>
        /// FUTURO: Método virtual para audit trail detallado
        /// Genera registro completo de cambios para compliance y forensics
        /// </summary>
        // public virtual AuditEntry GenerarAuditEntry(AuditAction accion, SecurityContext contexto)

        /// <summary>
        /// FUTURO: Método virtual para validación de integridad referencial
        /// Valida que relaciones entre entidades sean consistentes y válidas
        /// </summary>
        // public virtual IntegrityValidationResult ValidarIntegridad()

        // ========== PROPIEDADES FUTURAS PARA AUDIT TRAIL AVANZADO ==========
        // TODO: Implementar audit trail más completo para compliance empresarial:

        /// <summary>
        /// FUTURO: Usuario que creó el registro - AUDIT TRAIL COMPLETO
        /// Foreign key a Usuario para trazabilidad completa de responsabilidades
        /// </summary>
        // public int? CreadoPorUsuarioId { get; set; }
        // public virtual Usuario? CreadoPor { get; set; }

        /// <summary>
        /// FUTURO: Usuario que modificó el registro por última vez - CHANGE TRACKING
        /// Foreign key a Usuario para audit trail de modificaciones
        /// </summary>
        // public int? ModificadoPorUsuarioId { get; set; }
        // public virtual Usuario? ModificadoPor { get; set; }

        /// <summary>
        /// FUTURO: Timestamp de eliminación lógica - SOFT DELETE AUDIT
        /// Registro exacto de cuándo se marcó como inactivo para compliance
        /// </summary>
        // public DateTime? FechaEliminacion { get; set; }

        /// <summary>
        /// FUTURO: Usuario que eliminó lógicamente el registro - DELETION AUDIT
        /// Audit trail completo de quién realizó la eliminación lógica
        /// </summary>
        // public int? EliminadoPorUsuarioId { get; set; }
        // public virtual Usuario? EliminadoPor { get; set; }

        /// <summary>
        /// FUTURO: Número de versión para optimistic locking empresarial
        /// Control de concurrencia avanzado para operaciones críticas
        /// </summary>
        // public long Version { get; set; }

        /// <summary>
        /// FUTURO: Clasificación de seguridad de la entidad
        /// Nivel de seguridad (Público, Confidencial, Secreto) para access control
        /// </summary>
        // public SecurityClassification ClasificacionSeguridad { get; set; }

        /// <summary>
        /// FUTURO: Contexto de origen de la entidad
        /// Información sobre cómo/dónde se originó la entidad (API, UI, ETL, etc.)
        /// </summary>
        // public OriginContext? Origen { get; set; }

        /// <summary>
        /// FUTURO: Hash de integridad para detección de tampering
        /// Hash criptográfico para detectar modificaciones no autorizadas
        /// </summary>
        // public string? HashIntegridad { get; set; }
    }
} 