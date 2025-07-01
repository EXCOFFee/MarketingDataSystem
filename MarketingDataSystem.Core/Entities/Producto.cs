// ==================== ENTIDAD DE DOMINIO - PRODUCTO COMERCIAL ====================
// SISTEMA: MarketingDataSystem - Sistema Empresarial de Gestión de Datos de Marketing
// COMPONENTE: Entidad Core de Catálogo de Productos - ALTA CRITICIDAD
// RESPONSABILIDAD: Gestión completa del catálogo maestro de productos comerciales
// CAPA: Core/Domain - Entidad fundamental del modelo de negocio
// DDD: Aggregate Root - Maneja su propio ciclo de vida y reglas de negocio
// RELACIONES: Centro de las relaciones Stock (inventario) y Venta (transacciones)
// VALIDACIONES: Implementa validaciones a nivel de dominio para integridad comercial
// PERSISTENCIA: Mapeada a tabla 'Productos' en base de datos con índices optimizados

using System.Collections.Generic; // Para colecciones de navegación
using System.ComponentModel.DataAnnotations; // Para validaciones de dominio

namespace MarketingDataSystem.Core.Entities
{
    /// <summary>
    /// ENTIDAD CORE CRÍTICA: Producto del Catálogo Comercial - ALTA CRITICIDAD
    /// ====================================================================
    /// 
    /// DOMINIO EMPRESARIAL:
    /// Esta entidad representa un producto comercial en el catálogo maestro del sistema.
    /// Es el núcleo del negocio ya que define qué se vende, a qué precio, quién lo provee
    /// y cómo se categoriza. Impacta directamente en ventas, inventario y estrategia comercial.
    /// 
    /// CASOS DE USO EMPRESARIALES CRÍTICOS:
    /// 1. CATÁLOGO COMERCIAL: Gestión del catálogo maestro de productos para venta
    /// 2. PRICING MANAGEMENT: Control de precios y estrategias de pricing dinámico
    /// 3. INVENTORY CONTROL: Base para gestión de inventario y control de stock
    /// 4. SALES ANALYTICS: Análisis de performance de ventas por producto
    /// 5. SUPPLIER MANAGEMENT: Gestión de proveedores y cadena de suministro
    /// 6. CATEGORY MANAGEMENT: Optimización de categorías y mix de productos
    /// 7. MARKETING CAMPAIGNS: Segmentación y targeting por tipo de producto
    /// 8. FINANCIAL REPORTING: Base para cálculo de ingresos y márgenes de ganancia
    /// 9. CUSTOMER EXPERIENCE: Información de producto para atención al cliente
    /// 10. PROCUREMENT: Planificación de compras y gestión de proveedores
    /// 11. COMPLIANCE: Cumplimiento con regulaciones de productos y etiquetado
    /// 12. LIFECYCLE MANAGEMENT: Gestión del ciclo de vida desde lanzamiento hasta descontinuación
    /// 
    /// ARQUITECTURA:
    /// - CAPA: Core/Domain en Clean Architecture - completamente independiente de infraestructura
    /// - PATRÓN: DDD Aggregate Root - maneja consistencia y reglas de negocio del producto
    /// - RESPONSABILIDAD: Gestión integral del catálogo con validaciones de negocio
    /// 
    /// RELACIONES CRÍTICAS:
    /// - Stocks (1:N) - Un producto puede tener múltiples registros de stock por ubicación
    /// - Ventas (N:M) - Un producto puede participar en múltiples transacciones de venta
    /// - Proveedores (Conceptual) - Relación con gestión de proveedores y cadena de suministro
    /// - Categorías (Conceptual) - Agrupación lógica para análisis y gestión comercial
    /// 
    /// INTEGRIDAD COMERCIAL:
    /// - Consistencia de precios: Precios coherentes con estrategia comercial
    /// - Validación de proveedores: Proveedores activos y confiables
    /// - Categorización correcta: Productos correctamente categorizados
    /// - Información completa: Datos completos para soporte comercial
    /// 
    /// IMPACTO FINANCIERO:
    /// - Revenue Impact: Cambios en productos afectan directamente los ingresos
    /// - Margin Analysis: Base para análisis de márgenes y rentabilidad
    /// - Cost Management: Gestión de costos de productos y proveedores
    /// - Pricing Strategy: Implementación de estrategias de precios dinámicos
    /// 
    /// COMPLIANCE Y REGULACIONES:
    /// - Product Labeling: Cumplimiento con regulaciones de etiquetado
    /// - Safety Standards: Estándares de seguridad y calidad de productos
    /// - Tax Compliance: Correcta categorización para efectos fiscales
    /// - Import/Export: Cumplimiento con regulaciones de comercio internacional
    /// 
    /// PERFORMANCE Y ESCALABILIDAD:
    /// - Catálogo masivo: Soporte para miles de productos con performance óptimo
    /// - Búsqueda optimizada: Índices para búsquedas rápidas por nombre, categoría, proveedor
    /// - Caching inteligente: Productos populares en cache para acceso rápido
    /// - Particionamiento: Estrategias de particionamiento por categoría o proveedor
    /// 
    /// SEGURIDAD:
    /// - Control de acceso: Solo usuarios autorizados pueden modificar el catálogo
    /// - Audit trail: Registro completo de cambios en productos críticos
    /// - Validaciones estrictas: Prevención de datos incorrectos en el catálogo
    /// - Backup automático: Respaldo frecuente del catálogo maestro
    /// </summary>
    public class Producto : BaseEntity // Hereda propiedades comunes (Id, timestamps, etc.)
    {
        // ========== IDENTIFICADOR ÚNICO COMERCIAL ==========
        /// <summary>
        /// IDENTIFICADOR ÚNICO DEL PRODUCTO
        /// ===============================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Identificador único e inmutable que representa este producto específico en todo
        /// el ecosistema comercial. Es la clave principal para referencias en ventas,
        /// inventario, reportes financieros y análisis de negocio.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - TRANSACCIONES: Referencia única en todas las ventas y movimientos de stock
        /// - REPORTES FINANCIEROS: Clave primaria para análisis de ingresos por producto
        /// - INTEGRACIÓN: Referencia para sistemas externos (ERP, e-commerce, POS)
        /// - CUSTOMER SERVICE: Identificador para consultas y soporte al cliente
        /// - ANALYTICS: Clave para análisis de performance y rotación de productos
        /// - PROCUREMENT: Identificación única para órdenes de compra y reabastecimiento
        /// 
        /// CONSIDERACIONES TÉCNICAS:
        /// - AUTO-INCREMENT: Generado automáticamente por la base de datos
        /// - INMUTABLE: Una vez asignado, permanece constante durante todo el ciclo de vida
        /// - ÚNICO: Garantiza unicidad absoluta en todo el sistema comercial
        /// - INDEXADO: Índice clustered para máximo performance en consultas
        /// 
        /// INTEGRIDAD REFERENCIAL:
        /// - Referenciado por Stock para gestión de inventario distribuido
        /// - Usado en Ventas para trazabilidad completa de transacciones
        /// - Clave para logs de auditoría y cambios de catálogo
        /// - Base para numeración de SKUs y códigos de producto
        /// </summary>
        [Key] // Anotación para EF Core - define como clave primaria
        public int IdProducto { get; set; }

        // ========== INFORMACIÓN COMERCIAL BÁSICA ==========
        /// <summary>
        /// NOMBRE COMERCIAL DEL PRODUCTO
        /// =============================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Nombre comercial visible para clientes, empleados y sistemas externos.
        /// Es la identificación principal del producto en interfaces de usuario,
        /// catálogos comerciales, facturas y material de marketing.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - CUSTOMER FACING: Nombre visible en catálogos, e-commerce y puntos de venta
        /// - FACTURAS: Descripción del producto en documentos fiscales oficiales
        /// - REPORTES GERENCIALES: Identificación de productos en análisis de ventas
        /// - BÚSQUEDA: Campo principal para búsquedas de productos por usuarios
        /// - MARKETING: Nombre usado en campañas publicitarias y material promocional
        /// - INVENTORY: Identificación de productos en sistemas de gestión de almacén
        /// - CUSTOMER SERVICE: Referencia para atención al cliente y soporte técnico
        /// 
        /// VALIDACIONES CRÍTICAS DE NEGOCIO:
        /// - OBLIGATORIO: No puede estar vacío - todo producto debe tener nombre
        /// - UNICIDAD: Considerar unicidad para evitar confusión de productos
        /// - LONGITUD: Validar longitud apropiada para sistemas downstream
        /// - CARACTERES: Permitir caracteres especiales según regulaciones locales
        /// - IDIOMA: Considerar soporte multi-idioma para mercados internacionales
        /// 
        /// CONSIDERACIONES COMERCIALES:
        /// - BRANDING: Coherencia con estrategia de marca y naming conventions
        /// - SEO: Optimización para búsquedas online si aplica e-commerce
        /// - LEGAL: Cumplimiento con regulaciones de nombres de productos
        /// - MARKETING: Nombres atractivos que impulsen ventas y recordación
        /// </summary>
        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [StringLength(200, ErrorMessage = "El nombre del producto no puede exceder 200 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// CATEGORÍA COMERCIAL DEL PRODUCTO
        /// ================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Clasificación comercial del producto que permite agrupación lógica para análisis,
        /// reportes, estrategias de marketing y optimización del mix de productos.
        /// Es fundamental para organización del catálogo y análisis de performance.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - CATEGORY MANAGEMENT: Gestión estratégica de categorías de productos
        /// - SALES ANALYTICS: Análisis de ventas y performance por categoría de producto
        /// - MARKETING SEGMENTATION: Segmentación para campañas dirigidas por categoría
        /// - INVENTORY PLANNING: Planificación de inventario por categoría comercial
        /// - PRICING STRATEGY: Estrategias de precios diferenciadas por categoría
        /// - CUSTOMER NAVIGATION: Filtros de navegación en catálogos y e-commerce
        /// - FINANCIAL REPORTING: Reportes de ingresos y márgenes por categoría
        /// - SUPPLIER NEGOTIATION: Negociaciones con proveedores por categoría de producto
        /// - PROMOTIONAL PLANNING: Planificación de promociones por categoría
        /// - COMPETITIVE ANALYSIS: Análisis competitivo por categoría de mercado
        /// 
        /// VALORES EMPRESARIALES TÍPICOS:
        /// - "Electrónicos", "Ropa y Accesorios", "Hogar y Jardín", "Deportes"
        /// - "Alimentación", "Salud y Belleza", "Automotriz", "Construcción"
        /// - "Libros y Medios", "Juguetes", "Mascotas", "Industrial"
        /// 
        /// VALIDACIONES DE NEGOCIO:
        /// - CATÁLOGO: Validar contra lista oficial de categorías activas del negocio
        /// - COHERENCIA: Verificar que la categoría sea apropiada para el tipo de producto
        /// - JERARQUÍA: Considerar estructura jerárquica de categorías (principal/subcategoría)
        /// - BUSINESS RULES: Algunas categorías pueden tener reglas especiales de negocio
        /// 
        /// CONSIDERACIONES FUTURAS:
        /// - NORMALIZACIÓN: Migrar a tabla Categorías para integridad referencial
        /// - JERARQUÍA: Implementar estructura jerárquica de categorías y subcategorías
        /// - MULTI-CATEGORÍA: Permitir que productos pertenezcan a múltiples categorías
        /// - TAXONOMY: Implementar taxonomía estándar de la industria para categorización
        /// </summary>
        public string Categoria { get; set; } = string.Empty;

        /// <summary>
        /// PROVEEDOR DEL PRODUCTO
        /// =====================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Identifica el proveedor responsable del suministro de este producto.
        /// Es crítico para gestión de la cadena de suministro, negociaciones comerciales,
        /// control de calidad y análisis de performance de proveedores.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - SUPPLIER MANAGEMENT: Gestión integral de proveedores y relaciones comerciales
        /// - PROCUREMENT: Planificación de compras y órdenes de reabastecimiento
        /// - QUALITY CONTROL: Trazabilidad para control de calidad y responsabilidades
        /// - COST ANALYSIS: Análisis de costos y negociaciones por proveedor
        /// - SUPPLIER PERFORMANCE: Evaluación de performance y confiabilidad de proveedores
        /// - RISK MANAGEMENT: Diversificación de proveedores para mitigar riesgos
        /// - PAYMENT TERMS: Gestión de términos de pago y condiciones comerciales
        /// - COMPLIANCE: Cumplimiento con regulaciones de proveedores y auditorías
        /// - SUPPLIER DIVERSITY: Estrategias de diversificación de base de proveedores
        /// - CONTRACT MANAGEMENT: Gestión de contratos y acuerdos comerciales
        /// 
        /// INFORMACIÓN CRÍTICA PARA NEGOCIO:
        /// - LEAD TIMES: Tiempos de entrega que afectan planificación de inventario
        /// - QUALITY STANDARDS: Estándares de calidad y especificaciones técnicas
        /// - PAYMENT TERMS: Condiciones de pago que afectan flujo de caja
        /// - MINIMUM ORDERS: Cantidades mínimas de pedido para optimización de costos
        /// - GEOGRAPHIC LOCATION: Ubicación geográfica para logística y costos de envío
        /// 
        /// VALIDACIONES DE NEGOCIO:
        /// - PROVEEDOR ACTIVO: Verificar que el proveedor esté activo y autorizado
        /// - COMPLIANCE: Cumplimiento con regulaciones y certificaciones requeridas
        /// - CREDIT STANDING: Verificación de situación crediticia y financiera
        /// - QUALITY CERTIFICATION: Certificaciones de calidad y estándares industriales
        /// 
        /// CONSIDERACIONES FUTURAS:
        /// - NORMALIZACIÓN: Migrar a tabla Proveedores para gestión integral
        /// - MULTI-SUPPLIER: Permitir múltiples proveedores por producto para redundancia
        /// - SUPPLIER RATING: Sistema de calificación y evaluación de proveedores
        /// - CONTRACT INTEGRATION: Integración con sistemas de gestión de contratos
        /// </summary>
        public string Proveedor { get; set; } = string.Empty;

        /// <summary>
        /// PRECIO DE VENTA DEL PRODUCTO
        /// ============================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Precio de venta al público del producto en la moneda base del sistema.
        /// Es uno de los datos más críticos del negocio ya que impacta directamente
        /// en ingresos, márgenes, competitividad y estrategia comercial.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - REVENUE CALCULATION: Base para cálculo de ingresos y facturación
        /// - MARGIN ANALYSIS: Análisis de márgenes de ganancia por producto
        /// - PRICING STRATEGY: Implementación de estrategias de precios dinámicos
        /// - COMPETITIVE POSITIONING: Posicionamiento competitivo en el mercado
        /// - PROMOTIONAL PRICING: Base para cálculo de descuentos y promociones
        /// - FINANCIAL FORECASTING: Proyecciones financieras y presupuestos
        /// - TAX CALCULATION: Base para cálculo de impuestos sobre ventas
        /// - CUSTOMER QUOTATION: Generación de cotizaciones y propuestas comerciales
        /// - PROFITABILITY ANALYSIS: Análisis de rentabilidad por producto
        /// - INVENTORY VALUATION: Valorización de inventario para reportes financieros
        /// 
        /// VALIDACIONES CRÍTICAS FINANCIERAS:
        /// - RANGO VÁLIDO: Debe ser >= 0 (considerando productos promocionales gratuitos)
        /// - COHERENCIA COMERCIAL: Precios coherentes con posicionamiento de marca
        /// - MARGIN PROTECTION: Validar que el precio cubra costos mínimos + margen
        /// - COMPETITIVE RANGE: Precios dentro de rangos competitivos del mercado
        /// - AUTHORIZATION: Precios especiales requieren autorización gerencial
        /// 
        /// CONSIDERACIONES FINANCIERAS CRÍTICAS:
        /// - ⚠️ PRECISIÓN: CRÍTICO - Migrar de float a decimal para precisión financiera
        /// - CURRENCY: Definir claramente la moneda base del sistema
        /// - ROUNDING: Implementar reglas de redondeo consistentes
        /// - HISTORICAL PRICING: Mantener histórico de precios para análisis de tendencias
        /// - MULTI-CURRENCY: Considerar soporte para múltiples monedas en el futuro
        /// 
        /// IMPACTO EN COMPLIANCE:
        /// - TAX COMPLIANCE: Precios base para cálculo correcto de impuestos
        /// - FINANCIAL REPORTING: Base para reportes financieros y auditorías
        /// - REGULATORY: Cumplimiento con regulaciones de precios mínimos/máximos
        /// - CONSUMER PROTECTION: Cumplimiento con leyes de protección al consumidor
        /// 
        /// ESTRATEGIAS DE PRICING:
        /// - DYNAMIC PRICING: Implementación de precios dinámicos basados en demanda
        /// - PSYCHOLOGICAL PRICING: Precios psicológicos para maximizar ventas
        /// - BUNDLING: Precios especiales para venta de productos agrupados
        /// - LOYALTY PRICING: Precios diferenciados para clientes leales
        /// </summary>
        [Range(0, float.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a cero")]
        public float Precio { get; set; } // TODO: CRÍTICO - Migrar a decimal para precisión financiera real

        // ========== RELACIONES DE NAVEGACIÓN (FOREIGN KEYS) ==========
        /// <summary>
        /// COLECCIÓN DE REGISTROS DE STOCK POR UBICACIÓN
        /// ============================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Colección de todos los registros de inventario asociados a este producto
        /// distribuidos en diferentes ubicaciones, sucursales o almacenes.
        /// Es fundamental para gestión de inventario distribuido y disponibilidad total.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - AVAILABILITY CHECK: Verificación de disponibilidad total antes de ventas
        /// - INVENTORY DISTRIBUTION: Análisis de distribución de inventario por ubicación
        /// - STOCK TRANSFER: Planificación de transferencias entre ubicaciones
        /// - REORDER POINT: Cálculo de puntos de reorden considerando todas las ubicaciones
        /// - STOCKOUT PREVENTION: Prevención de agotamiento de stock total
        /// - LOCATION OPTIMIZATION: Optimización de ubicaciones según demanda local
        /// - FULFILLMENT: Selección de ubicación óptima para cumplir órdenes
        /// - INVENTORY VALUATION: Valorización total de inventario del producto
        /// - CYCLE COUNTING: Planificación de conteos cíclicos por ubicación
        /// - OBSOLESCENCE DETECTION: Identificación de stock obsoleto por ubicación
        /// 
        /// RELACIÓN DE DATOS:
        /// - CARDINALIDAD: 1:N - Un producto puede tener múltiples registros de stock
        /// - INTEGRIDAD: Cada registro de stock debe referenciar un producto válido
        /// - AGGREGATION: La suma de todos los stocks determina disponibilidad total
        /// - DISTRIBUTION: Permite gestión de inventario multi-ubicación
        /// 
        /// NAVEGACIÓN LAZY LOADING:
        /// - EF CORE: Carga automática cuando se accede a la colección por primera vez
        /// - PERFORMANCE: Se carga bajo demanda para optimizar consultas iniciales
        /// - BATCH LOADING: Usar .Include() para cargar explícitamente en consultas masivas
        /// - FILTERING: Aplicar filtros antes de cargar para mejor performance
        /// 
        /// OPTIMIZACIÓN DE CONSULTAS:
        /// - EXPLICIT LOADING: Usar .Include(p => p.Stocks) para cargar en consultas específicas
        /// - PROJECTION: Usar .Select() para obtener solo datos de stock necesarios
        /// - AGGREGATION: Cálculos de stock total directamente en base de datos
        /// - CACHING: Stock de productos populares en cache con expiración
        /// 
        /// INTEGRIDAD DE DATOS:
        /// - CONSISTENCY: Garantizar coherencia entre producto y sus registros de stock
        /// - CASCADE BEHAVIOR: Comportamiento definido si se elimina el producto
        /// - VALIDATION: Validación de integridad antes de operaciones críticas
        /// - TRANSACTION: Operaciones transaccionales para mantener consistencia
        /// </summary>
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();

        /// <summary>
        /// COLECCIÓN DE TRANSACCIONES DE VENTA
        /// ==================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Colección de todas las transacciones de venta en las que participa este producto.
        /// Proporciona trazabilidad completa del historial comercial y es fundamental
        /// para análisis de performance, rotación y popularidad del producto.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - SALES ANALYTICS: Análisis completo de performance de ventas del producto
        /// - PRODUCT POPULARITY: Identificación de productos más/menos vendidos
        /// - CUSTOMER BEHAVIOR: Análisis de patrones de compra por producto
        /// - REVENUE TRACKING: Seguimiento de ingresos generados por producto
        /// - SEASONALITY ANALYSIS: Identificación de patrones estacionales de demanda
        /// - CROSS-SELLING: Análisis de productos vendidos juntos para cross-selling
        /// - MARGIN ANALYSIS: Análisis de márgenes históricos por producto
        /// - FORECASTING: Predicción de demanda futura basada en histórico de ventas
        /// - INVENTORY PLANNING: Planificación de inventario basada en velocidad de venta
        /// - PRODUCT LIFECYCLE: Análisis del ciclo de vida del producto en el mercado
        /// 
        /// RELACIÓN DE DATOS:
        /// - CARDINALIDAD: N:M - Un producto puede estar en múltiples ventas
        /// - HISTORICAL DATA: Mantiene historial completo de transacciones
        /// - PERFORMANCE METRICS: Base para cálculo de KPIs de producto
        /// - CUSTOMER INSIGHTS: Datos para análisis de comportamiento de clientes
        /// 
        /// NAVEGACIÓN LAZY LOADING:
        /// - EF CORE: Maneja automáticamente la tabla intermedia Producto-Venta
        /// - PERFORMANCE: Considerar paginación para productos con alto volumen de ventas
        /// - BATCH LOADING: Usar .Include() con cuidado debido al potencial volumen de datos
        /// - SELECTIVE LOADING: Cargar solo transacciones necesarias por período/filtro
        /// 
        /// OPTIMIZACIÓN DE PERFORMANCE:
        /// - PAGINATION: Implementar paginación para productos con muchas transacciones
        /// - DATE FILTERING: Filtrar por rangos de fecha para limitar volumen de datos
        /// - AGGREGATION: Cálculos agregados directamente en base de datos
        /// - ARCHIVING: Considerar archivado de transacciones antiguas para performance
        /// 
        /// ANÁLISIS Y REPORTES:
        /// - SALES VELOCITY: Cálculo de velocidad de venta para planificación de inventario
        /// - REVENUE CONTRIBUTION: Contribución del producto a ingresos totales
        /// - SEASONAL PATTERNS: Identificación de patrones estacionales de demanda
        /// - CUSTOMER SEGMENTATION: Segmentación de clientes por productos comprados
        /// </summary>
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();

        // ========== MÉTODOS DE DOMINIO FUTUROS (BUSINESS LOGIC) ==========
        /// <summary>
        /// ROADMAP - MÉTODOS DE DOMINIO A IMPLEMENTAR:
        /// ==========================================
        /// 
        /// 1. bool EstaDisponible()
        ///    - Verifica si hay stock suficiente en cualquier ubicación para venta
        ///    - Considera stock reservado y comprometido en otras transacciones
        ///    - Implementa reglas de negocio específicas por tipo de producto
        /// 
        /// 2. int CalcularStockTotal()
        ///    - Calcula la suma total de stock disponible en todas las ubicaciones
        ///    - Excluye stock en cuarentena, dañado o no disponible para venta
        ///    - Optimizado para consultas frecuentes con caching inteligente
        /// 
        /// 3. decimal CalcularTotal(int cantidad, IDiscountService discountService)
        ///    - Calcula total de venta considerando cantidad, precio y descuentos
        ///    - Aplica reglas de descuento por volumen, promociones y loyalty programs
        ///    - Incluye cálculo de impuestos según categoría del producto
        /// 
        /// 4. bool PuedeSerVendido(int cantidad, string ubicacion)
        ///    - Valida todas las reglas de negocio antes de permitir venta
        ///    - Verifica disponibilidad de stock, estado del producto, restricciones
        ///    - Considera reglas específicas por ubicación o canal de venta
        /// 
        /// 5. ProductoPriceResult ActualizarPrecio(decimal nuevoPrecio, string usuario, string motivo)
        ///    - Actualiza precio con validaciones exhaustivas de negocio
        ///    - Requiere autorización para cambios significativos de precio
        ///    - Mantiene historial de cambios para auditoría y análisis
        /// 
        /// 6. ProductoPerformance CalcularPerformance(DateTime desde, DateTime hasta)
        ///    - Calcula métricas de performance del producto en período específico
        ///    - Incluye ventas, rotación, margen, popularidad y tendencias
        ///    - Base para análisis de ciclo de vida del producto
        /// 
        /// 7. bool RequiereReposicion(IForecastingService forecastingService)
        ///    - Determina si el producto requiere reposición de inventario
        ///    - Considera lead times, patrones de demanda y stock de seguridad
        ///    - Integra con sistemas de forecasting para predicciones precisas
        /// 
        /// 8. IEnumerable<string> ValidarIntegridadComercial()
        ///    - Valida integridad de todos los datos comerciales del producto
        ///    - Identifica inconsistencias en precios, categorías, proveedores
        ///    - Implementa reglas de negocio específicas para validación
        /// 
        /// 9. ProductoRecommendation GenerarRecomendaciones(ICustomerProfile customerProfile)
        ///    - Genera recomendaciones personalizadas basadas en perfil del cliente
        ///    - Considera historial de compras, preferencias y patrones de comportamiento
        ///    - Implementa algoritmos de machine learning para mejores recomendaciones
        /// 
        /// 10. ProductoLifecycleStatus EvaluarCicloDeVida()
        ///     - Evalúa en qué etapa del ciclo de vida se encuentra el producto
        ///     - Considera introducción, crecimiento, madurez, declive
        ///     - Base para decisiones estratégicas de marketing y pricing
        /// 
        /// 11. decimal CalcularMargenBruto(decimal costoProducto)
        ///     - Calcula margen bruto considerando precio de venta vs costo
        ///     - Incluye todos los costos directos e indirectos del producto
        ///     - Base para análisis de rentabilidad y decisiones de pricing
        /// 
        /// 12. bool EsProductoEstrategico(IBusinessRulesEngine rulesEngine)
        ///     - Determina si el producto es estratégico para el negocio
        ///     - Considera volumen de ventas, margen, diferenciación competitiva
        ///     - Base para decisiones de inversión y desarrollo de producto
        /// </summary>
        // TODO: Implementar métodos de dominio críticos para lógica de negocio empresarial
    }
} 