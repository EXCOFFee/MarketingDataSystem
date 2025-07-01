// ==================== ENTIDAD DE DOMINIO - REGISTRO DE INVENTARIO ====================
// SISTEMA: MarketingDataSystem - Sistema Empresarial de Gestión de Datos de Marketing
// COMPONENTE: Entidad Core de Gestión de Inventario - ALTA CRITICIDAD
// RESPONSABILIDAD: Control preciso de inventario distribuido con integridad absoluta
// CAPA: Core/Domain - Entidad fundamental para gestión de inventario y logística
// DDD: Value Object - Representa un nivel específico de stock en momento y lugar
// CRITICIDAD: ALTA - Errores en stock pueden causar sobreventa o pérdida de ventas
// INTEGRIDAD: Debe mantener consistencia con ventas y movimientos de productos
// AUDITORÍA: Cambios en stock deben ser completamente trazables para compliance

using System;
using System.ComponentModel.DataAnnotations; // Para validaciones de inventario críticas

namespace MarketingDataSystem.Core.Entities
{
    /// <summary>
    /// ENTIDAD CORE CRÍTICA: Registro de Inventario de Productos - ALTA CRITICIDAD
    /// ======================================================================
    /// 
    /// DOMINIO EMPRESARIAL:
    /// Esta entidad representa un registro específico de inventario que controla la disponibilidad
    /// de productos en ubicaciones específicas. Es fundamental para evitar sobreventa,
    /// optimizar la cadena de suministro y garantizar la satisfacción del cliente.
    /// 
    /// CASOS DE USO EMPRESARIALES CRÍTICOS:
    /// 1. CONTROL DE INVENTARIO: Gestión precisa de stock disponible por ubicación
    /// 2. PREVENCIÓN DE SOBREVENTA: Validación de disponibilidad antes de confirmar ventas
    /// 3. ALERTAS DE REABASTECIMIENTO: Notificaciones automáticas cuando el stock es bajo
    /// 4. ANÁLISIS DE ROTACIÓN: Identificación de productos de rápido/lento movimiento
    /// 5. OPTIMIZACIÓN LOGÍSTICA: Distribución eficiente de inventario entre sucursales
    /// 6. PLANIFICACIÓN DE COMPRAS: Datos para cálculo de puntos de reorden
    /// 7. AUDITORÍAS FÍSICAS: Conciliación entre inventario físico y registros del sistema
    /// 8. ANÁLISIS DE PÉRDIDAS: Identificación de mermas, robos o deterioros
    /// 9. FORECASTING: Predicción de demanda futura basada en patrones históricos
    /// 10. COMPLIANCE: Cumplimiento con regulaciones de inventario y trazabilidad
    /// 
    /// ARQUITECTURA:
    /// - CAPA: Core/Domain en Clean Architecture - independiente de infraestructura
    /// - PATRÓN: DDD Value Object - representa estado de inventario en momento específico
    /// - RESPONSABILIDAD: Gestión de disponibilidad de productos por ubicación
    /// 
    /// RELACIONES CRÍTICAS:
    /// - Producto (N:1) - Múltiples registros de stock pueden ser del mismo producto
    /// - Venta (Indirecta) - Cada venta debe validar y actualizar el stock correspondiente
    /// - Ubicación (Conceptual) - Stock distribuido en múltiples sucursales/almacenes
    /// 
    /// INTEGRIDAD DE INVENTARIO:
    /// - Consistencia transaccional: Stock debe actualizarse atomicamente con ventas
    /// - Validación de disponibilidad: Verificación antes de confirmar transacciones
    /// - Conciliación automática: Sincronización entre movimientos y saldos
    /// - Auditoría completa: Trazabilidad de todos los movimientos de inventario
    /// 
    /// ALERTAS Y MONITOREO:
    /// - Stock bajo: Alertas cuando se alcanza el punto de reorden
    /// - Stock crítico: Notificaciones urgentes cuando el inventario está agotándose
    /// - Stock excesivo: Identificación de productos con sobreinventario
    /// - Movimientos anómalos: Detección de cambios de stock inusuales
    /// - Stock negativo: Alertas críticas por posibles errores del sistema
    /// 
    /// PERFORMANCE Y ESCALABILIDAD:
    /// - Índices optimizados: Consultas rápidas por producto y ubicación
    /// - Caching inteligente: Stock frecuentemente consultado en memoria
    /// - Particionamiento: División por ubicación para mejor performance
    /// - Archivado automático: Movimiento de registros históricos a almacenamiento frío
    /// 
    /// COMPLIANCE Y AUDITORÍA:
    /// - Trazabilidad completa: Registro de todos los movimientos de inventario
    /// - Retención de datos: Conservación de históricos según regulaciones
    /// - Auditorías físicas: Soporte para conciliación con conteos físicos
    /// - Reporting regulatorio: Datos para reportes de inventario requeridos por ley
    /// 
    /// SEGURIDAD:
    /// - Control de acceso: Solo usuarios autorizados pueden modificar stock
    /// - Audit trail: Registro completo de quién modificó qué inventario
    /// - Validaciones estrictas: Prevención de manipulación indebida de stock
    /// - Backup automático: Respaldo frecuente de datos críticos de inventario
    /// </summary>
    public class Stock : BaseEntity // Hereda propiedades comunes (Id, timestamps, etc.)
    {
        // ========== IDENTIFICADOR ÚNICO DE REGISTRO ==========
        /// <summary>
        /// IDENTIFICADOR ÚNICO DE REGISTRO DE STOCK
        /// =======================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Identificador único que representa este registro específico de inventario
        /// en el sistema. Permite trazabilidad granular de movimientos de stock
        /// y es fundamental para auditorías y reportes de inventario precisos.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - TRAZABILIDAD: Seguimiento individual de cada registro de inventario
        /// - AUDITORÍA: Referencia única para logs de auditoría de movimientos
        /// - REPORTES: Drill-down específico en análisis de inventario detallado
        /// - INTEGRACIONES: Referencia para sistemas externos de gestión de almacén
        /// - CONCILIACIÓN: Identificación precisa en procesos de conciliación física
        /// 
        /// CONSIDERACIONES TÉCNICAS:
        /// - AUTO-INCREMENT: Generado automáticamente por la base de datos
        /// - INMUTABLE: Una vez asignado, permanece constante durante todo el ciclo de vida
        /// - ÚNICO: Garantiza unicidad absoluta en todo el sistema de inventario
        /// - INDEXADO: Índice principal para máximo performance en consultas
        /// 
        /// INTEGRIDAD:
        /// - Referenciado en logs de movimientos para trazabilidad completa
        /// - Usado en procesos de conciliación de inventario automática
        /// - Clave para alertas específicas de stock crítico por ubicación
        /// </summary>
        [Key] // Anotación para EF Core - define como clave primaria
        public int IdStock { get; set; }

        // ========== RELACIÓN CON PRODUCTO ==========
        /// <summary>
        /// IDENTIFICADOR DE PRODUCTO INVENTARIADO
        /// =====================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Foreign Key que establece la relación crítica con el catálogo maestro de productos.
        /// Esta relación permite gestionar múltiples ubicaciones de stock para un mismo producto
        /// y es fundamental para análisis de distribución de inventario.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - DISPONIBILIDAD TOTAL: Suma de stock de todas las ubicaciones del producto
        /// - DISTRIBUCIÓN: Análisis de cómo está distribuido el inventario por ubicación
        /// - ALERTAS ESPECÍFICAS: Notificaciones por producto cuando el stock total es bajo
        /// - REPOSICIÓN: Cálculo de necesidades de reabastecimiento por producto
        /// - ANÁLISIS DE ROTACIÓN: Evaluación del movimiento de inventario por producto
        /// - OPTIMIZACIÓN: Rebalanceo de stock entre ubicaciones según demanda
        /// 
        /// VALIDACIONES CRÍTICAS DE NEGOCIO:
        /// - EXISTENCIA: El producto debe existir activamente en el catálogo maestro
        /// - ESTADO: El producto debe estar activo y no descontinuado
        /// - CONSISTENCIA: Debe haber coherencia entre stock y disponibilidad del producto
        /// - INTEGRIDAD: Cambios en producto afectan todos sus registros de stock
        /// 
        /// INTEGRIDAD REFERENCIAL:
        /// - CASCADE BEHAVIOR: Comportamiento definido si se elimina/modifica el producto
        /// - CONSTRAINT: Foreign key constraint a nivel de base de datos
        /// - VALIDATION: Validación transaccional antes de crear/modificar stock
        /// 
        /// PERFORMANCE:
        /// - ÍNDICE COMPUESTO: Índice (IdProducto, Sucursal) para consultas frecuentes
        /// - CACHING: Información de producto en cache para validaciones rápidas
        /// - AGGREGATION: Cálculos de stock total por producto optimizados
        /// </summary>
        [Required(ErrorMessage = "El identificador del producto es obligatorio para el registro de stock")]
        public int IdProducto { get; set; }

        // ========== CANTIDAD FÍSICA DISPONIBLE ==========
        /// <summary>
        /// CANTIDAD FÍSICA DISPONIBLE
        /// =========================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Cantidad física exacta de productos disponibles para venta inmediata en esta
        /// ubicación específica. Es el valor más crítico del sistema de inventario ya que
        /// determina directamente la capacidad de cumplir con las órdenes de los clientes.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - VALIDACIÓN DE VENTAS: Verificación de disponibilidad antes de confirmar órdenes
        /// - ALERTAS DE STOCK: Base para cálculo de alertas de stock bajo/crítico
        /// - PLANIFICACIÓN DE COMPRAS: Datos para cálculo de puntos de reorden
        /// - ANÁLISIS DE ROTACIÓN: Evaluación de movimiento y velocidad de inventario
        /// - OPTIMIZACIÓN LOGÍSTICA: Datos para redistribución entre ubicaciones
        /// - FORECASTING: Base para predicción de necesidades futuras de inventario
        /// - AUDITORÍAS: Comparación con conteos físicos para identificar discrepancias
        /// 
        /// VALIDACIONES CRÍTICAS DE NEGOCIO:
        /// - RANGO: Debe ser >= 0 - stock negativo indica error crítico del sistema
        /// - CONSISTENCIA: Debe coincidir con la suma de movimientos de inventario
        /// - CONCURRENCIA: Protección contra condiciones de carrera en ventas simultáneas
        /// - LÍMITES: Validación de capacidad máxima de almacenamiento por ubicación
        /// - ATOMICIDAD: Actualización atómica con transacciones de venta
        /// 
        /// CONSIDERACIONES TÉCNICAS:
        /// - PRECISION: int adecuado para mayoría de productos (considerar decimal para fraccionables)
        /// - CONCURRENCY: Control de concurrencia optimista para evitar overselling
        /// - TRANSACTION: Operaciones transaccionales para garantizar consistencia
        /// - VALIDATION: Validaciones de negocio antes de permitir modificaciones
        /// 
        /// INTEGRIDAD DE INVENTARIO:
        /// - REAL-TIME: Debe reflejar exactamente el inventario físico disponible
        /// - AUDIT TRAIL: Registro completo de todos los cambios de cantidad
        /// - RECONCILIATION: Conciliación periódica con conteos físicos
        /// - BACKUP: Respaldo frecuente para recuperación ante fallos críticos
        /// </summary>
        [Required(ErrorMessage = "La cantidad de stock es obligatoria")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad de stock no puede ser negativa")]
        public int Cantidad { get; set; }

        // ========== INFORMACIÓN TEMPORAL ==========
        /// <summary>
        /// TIMESTAMP DE ÚLTIMO MOVIMIENTO DE INVENTARIO
        /// ===========================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Fecha y hora exacta del último movimiento, verificación o actualización de este
        /// registro de stock. Es fundamental para trazabilidad temporal, análisis de rotación
        /// de inventario y detección de stock obsoleto o sin movimiento.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - ANÁLISIS DE ROTACIÓN: Cálculo de velocidad de movimiento de inventario
        /// - DETECCIÓN DE OBSOLESCENCIA: Identificación de stock sin movimiento prolongado
        /// - AUDITORÍA TEMPORAL: Reconstrucción cronológica de movimientos de inventario
        /// - ALERTAS DE ESTANCAMIENTO: Notificaciones por productos sin actividad
        /// - REPORTES DE AGING: Análisis de antigüedad de inventario por períodos
        /// - PLANIFICACIÓN DE LIQUIDACIÓN: Identificación de productos para promociones
        /// - COMPLIANCE: Cumplimiento con regulaciones de trazabilidad temporal
        /// 
        /// VALIDACIONES DE NEGOCIO:
        /// - COHERENCIA: No puede ser posterior a la fecha actual del sistema
        /// - SECUENCIA: Debe ser posterior o igual a la fecha de creación del registro
        /// - BUSINESS HOURS: Considerar horarios comerciales para validaciones
        /// - TIMEZONE: Consistencia en zona horaria para análisis multi-ubicación
        /// 
        /// CONSIDERACIONES TÉCNICAS:
        /// - ⚠️ MEJORA FUTURA: Migrar a DateTimeOffset para manejo de zonas horarias
        /// - UTC STORAGE: Almacenar en UTC y convertir a local para reportes
        /// - PRECISION: Incluir milisegundos para ordenamiento preciso de movimientos
        /// - INDEXING: Índice para consultas frecuentes por rangos de fecha
        /// 
        /// INTEGRIDAD TEMPORAL:
        /// - AUTO-UPDATE: Actualización automática en cada modificación de stock
        /// - IMMUTABLE HISTORY: Preservar histórico para auditorías y análisis
        /// - SEQUENCE: Garantizar orden cronológico para análisis de series temporales
        /// </summary>
        public DateTime Fecha { get; set; } // TODO: Migrar a DateTimeOffset para zona horaria

        // ========== INFORMACIÓN GEOGRÁFICA Y LOGÍSTICA ==========
        /// <summary>
        /// Sucursal o punto de venta donde se encuentra este stock
        /// DOMINIO: Dimensión geográfica para inventario distribuido
        /// LOGÍSTICA: Permite gestión de stock en múltiples ubicaciones comerciales
        /// VENTAS: Determina disponibilidad por sucursal para ventas locales
        /// REPORTES: Análisis de distribución de inventario por ubicación
        /// ALERTAS: Notificaciones específicas por sucursal con stock bajo
        /// OPCIONAL: Puede estar vacía para stock centralizado o en tránsito
        /// </summary>
        public string Sucursal { get; set; } = string.Empty;

        /// <summary>
        /// Ubicación específica dentro de la sucursal o almacén
        /// DOMINIO: Coordenada física para localización precisa del inventario
        /// LOGÍSTICA: Facilita picking y ubicación física de productos
        /// WAREHOUSE: Información tipo "Pasillo A, Estante 3, Nivel 2"
        /// EFICIENCIA: Optimiza tiempo de búsqueda y recolección de productos
        /// INVENTARIOS: Facilita conteos físicos y verificaciones de stock
        /// OPCIONAL: Puede estar vacía para ubicaciones no específicas
        /// </summary>
        public string Ubicacion { get; set; } = string.Empty;

        // ========== RELACIÓN DE NAVEGACIÓN ==========
        /// <summary>
        /// Navegación hacia la entidad Producto asociada a este stock
        /// RELACIÓN: N:1 - Múltiples registros de stock pueden ser del mismo producto
        /// LAZY LOADING: EF Core carga automáticamente cuando se accede a la propiedad
        /// INFORMACIÓN: Acceso completo a detalles del producto (nombre, precio, etc.)
        /// PERFORMANCE: Se carga bajo demanda para optimizar consultas
        /// USO: Reportes de inventario que requieren información completa del producto
        /// </summary>
        public Producto Producto { get; set; } = new();

        // ========== MÉTODOS DE DOMINIO (BUSINESS LOGIC) ==========
        // TODO: Implementar métodos de dominio como:
        // - bool EsStockBajo(int stockMinimo) - determina si requiere reabastecimiento
        // - bool EsStockCritico(int stockCritico) - determina si requiere alerta inmediata
        // - void DescontarStock(int cantidad) - reduce stock por venta con validaciones
        // - void AgregarStock(int cantidad) - incrementa stock por reabastecimiento
        // - bool TieneStockSuficiente(int cantidadRequerida) - valida antes de venta
        // - TimeSpan TiempoSinMovimiento() - calcula días sin cambios para alertas
    }
} 