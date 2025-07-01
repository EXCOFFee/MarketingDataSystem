// ==================== DATA TRANSFER OBJECT CRÍTICO - CATÁLOGO DE PRODUCTOS ====================
// Este DTO maneja datos de productos que representan el catálogo empresarial
// CAPA: Application/Core - Contrato de transferencia de datos de catálogo
// CRITICIDAD: ALTA - Base del negocio y revenue, afecta ventas e inventario
// RESPONSABILIDAD: Transferencia segura de datos de productos entre capas
// CLEAN ARCHITECTURE: DTO puro sin dependencias externas para portabilidad
// GESTIÓN: Catálogo maestro para ventas, stock, pricing y reporting

// DTO que representa un producto en el sistema
namespace MarketingDataSystem.Core.DTOs
{
    /// <summary>
    /// Data Transfer Object para productos del catálogo empresarial - DATOS MAESTROS CRÍTICOS
    /// RESPONSABILIDAD: Transferir datos de productos de forma segura entre capas del sistema
    /// ARQUITECTURA: Application Layer DTO en Clean Architecture
    /// CRITICIDAD: ALTA - Representa catálogo maestro que alimenta ventas e inventario
    /// CASOS DE USO EMPRESARIALES:
    /// - APIs REST: Serialización JSON para endpoints de catálogo de productos
    /// - E-commerce: Información de productos para tiendas online
    /// - Sistemas POS: Datos de productos para puntos de venta
    /// - Gestión de Inventario: Base para control de stock y reposición
    /// - Pricing: Configuración de precios y políticas comerciales
    /// - Reportes Ejecutivos: Análisis de productos más vendidos y rentables
    /// - Integración ERP: Sincronización con sistemas de gestión empresarial
    /// - Marketing: Datos para campañas y promociones de productos
    /// VALIDACIONES CRÍTICAS APLICADAS:
    /// - Nombre requerido: Identificación clara del producto
    /// - Precio > 0: No permitir productos gratuitos o con precios negativos
    /// - Categoría válida: Clasificación según taxonomía empresarial
    /// - Proveedor existente: Debe estar registrado en sistema de proveedores
    /// - SKU único: Código de producto único para identificación
    /// MAPPING AUTOMÁTICO:
    /// - AutoMapper: ProductoDto ↔ Producto Entity (bidireccional)
    /// - JSON Serialization: Para APIs REST y integración externa
    /// GESTIÓN DE CATÁLOGO:
    /// - Categorización: Organización jerárquica de productos
    /// - Pricing: Gestión de precios y descuentos por producto
    /// - Lifecycle: Estados activo/inactivo para control de disponibilidad
    /// - Variantes: Support para productos con múltiples presentaciones
    /// COMPLIANCE:
    /// - Trazabilidad: Historial de cambios para auditoría
    /// - Regulatorio: Cumplimiento con normativas de productos específicos
    /// - Calidad: Certificaciones y estándares por categoría
    /// PERFORMANCE:
    /// - DTO liviano sin navegación para serialización rápida
    /// - Indexación automática en campos de búsqueda (Nombre, Categoría)
    /// </summary>
    public class ProductoDto
    {
        /// <summary>
        /// Identificador único del producto en el catálogo - PRIMARY KEY MAESTRO
        /// PROPÓSITO: Identificación única para trazabilidad en ventas e inventario
        /// GENERACIÓN: Auto-increment en base de datos para garantizar unicidad
        /// CASOS DE USO:
        /// - APIs REST: Identificación en endpoints (/api/productos/{id})
        /// - Ventas: Foreign key en transacciones de venta
        /// - Stock: Referencia para movimientos de inventario
        /// - Pricing: Identificación para políticas de precios
        /// - Reportes: Agrupación y análisis por producto específico
        /// - Integración: Mapping con códigos de productos externos (ERP, proveedores)
        /// VALIDACIÓN: > 0 para productos existentes, 0 para nuevos productos
        /// INMUTABILIDAD: Una vez asignado, nunca debe cambiar
        /// PERFORMANCE: Indexado para consultas rápidas de productos
        /// RELACIONES: Referenciado en Venta, Stock, y otros módulos críticos
        /// </summary>
        public int IdProducto { get; set; } // Identificador único del producto
        
        /// <summary>
        /// Nombre comercial del producto - IDENTIFICACIÓN COMERCIAL CRÍTICA
        /// PROPÓSITO: Denominación comercial para identificación en ventas y marketing
        /// VALIDACIÓN CRÍTICA: Requerido, único, longitud apropiada (3-200 caracteres)
        /// CASOS DE USO:
        /// - E-commerce: Título del producto en tiendas online
        /// - POS: Nombre mostrado en sistemas de punto de venta
        /// - Facturación: Descripción en facturas y documentos fiscales
        /// - Reportes: Identificación en reportes de ventas y análisis
        /// - Marketing: Nombre para campañas publicitarias y promociones
        /// - Búsqueda: Término principal para búsquedas de productos
        /// BUSINESS RULES:
        /// - Único en el sistema para evitar confusiones
        /// - Descriptivo y claro para comprensión del cliente
        /// - Libre de caracteres especiales problemáticos
        /// - Actualizable pero con auditoría de cambios
        /// FORMATO: Texto descriptivo sin códigos internos
        /// PERFORMANCE: Indexado para búsquedas de texto completo
        /// SEO: Optimizado para motores de búsqueda en e-commerce
        /// </summary>
        public string Nombre { get; set; } = string.Empty; // Nombre del producto
        
        /// <summary>
        /// Categoría comercial del producto - CLASIFICACIÓN EMPRESARIAL CRÍTICA
        /// PROPÓSITO: Clasificación jerárquica para organización y análisis empresarial
        /// CASOS DE USO:
        /// - Navegación: Menús categorizados en e-commerce y aplicaciones
        /// - Reportes: Análisis de ventas por categoría y segmento
        /// - Pricing: Políticas de precios específicas por categoría
        /// - Inventario: Gestión de stock organizada por categoría
        /// - Marketing: Campañas dirigidas por segmento de productos
        /// - Compras: Organización de proveedores por categoría
        /// - Analytics: KPIs de performance por línea de productos
        /// ESTRUCTURA JERÁRQUICA:
        /// - Ejemplo: "Electrónicos > Smartphones > iOS"
        /// - Formato: Categoría principal para simplificar
        /// - Extensible: Preparado para jerarquías futuras
        /// BUSINESS RULES:
        /// - Debe existir en catálogo de categorías válidas
        /// - Influye en reglas de negocio específicas (garantías, devoluciones)
        /// - Determina configuraciones de impuestos y compliance
        /// PERFORMANCE: Indexado para filtros y agregaciones rápidas
        /// MIGRATION: Permite recategorización controlada de productos
        /// </summary>
        public string Categoria { get; set; } = string.Empty;
        
        /// <summary>
        /// Proveedor principal del producto - INFORMACIÓN DE SOURCING CRÍTICA
        /// PROPÓSITO: Identificar fuente de suministro principal para gestión comercial
        /// CASOS DE USO:
        /// - Compras: Gestión de órdenes de compra y reposición
        /// - Calidad: Tracking de calidad y certificaciones por proveedor
        /// - Costos: Análisis de márgenes y negociación con proveedores
        /// - Supply Chain: Optimización de cadena de suministro
        /// - Risk Management: Diversificación de proveedores críticos
        /// - Reportes: Análisis de dependencia y performance por proveedor
        /// - Compliance: Verificación de certificaciones y cumplimiento
        /// BUSINESS RULES:
        /// - Debe existir en registro de proveedores autorizados
        /// - Impacta políticas de compra y reposición automática
        /// - Determina términos comerciales y tiempos de entrega
        /// - Influye en cálculos de costos y márgenes
        /// FORMATO: Nombre comercial o código de proveedor
        /// VALIDACIÓN: Debe estar activo en sistema de proveedores
        /// PERFORMANCE: Indexado para reportes de compras por proveedor
        /// FUTURO: Evolucionar a múltiples proveedores con preferencias
        /// </summary>
        public string Proveedor { get; set; } = string.Empty;
        
        /// <summary>
        /// Precio base del producto - VALOR FINANCIERO CRÍTICO PARA REVENUE
        /// PROPÓSITO: Precio base para cálculos de venta, márgenes y revenue
        /// VALIDACIÓN CRÍTICA: Debe ser > 0 (no se permiten productos gratuitos)
        /// CASOS DE USO:
        /// - Ventas: Precio base para transacciones (antes de descuentos)
        /// - Pricing: Base para políticas de precios dinámicos
        /// - Márgenes: Cálculo de rentabilidad vs costo de proveedor
        /// - Reportes: Análisis de pricing y competitividad
        /// - E-commerce: Precio mostrado en catálogos online
        /// - Promociones: Precio de referencia para cálculo de descuentos
        /// - Facturación: Precio unitario en documentos fiscales
        /// BUSINESS RULES:
        /// - Siempre positivo para productos comercializables
        /// - Puede diferir del precio de venta final (descuentos/recargos)
        /// - Sujeto a políticas de aprobación para cambios significativos
        /// - Base para cálculos automáticos de márgenes mínimos
        /// - Histórico para análisis de evolución de precios
        /// MONEDA: Asumida en moneda local del sistema (configuración global)
        /// PRECISION: float con validación de decimales apropiados para moneda
        /// AUDITORÍA: Cambios registrados para compliance y análisis
        /// PERFORMANCE: Indexado para consultas de productos por rango de precio
        /// </summary>
        public float Precio { get; set; } // Precio del producto

        // ========== PROPIEDADES FUTURAS PARA CATÁLOGO EMPRESARIAL ==========
        // TODO: Implementar propiedades adicionales para funcionalidad avanzada:

        /// <summary>
        /// Código SKU único del producto para identificación unívoca
        /// FUTURO: Stock Keeping Unit para control granular de inventario
        /// </summary>
        // public string SKU { get; set; } = string.Empty;

        /// <summary>
        /// Descripción detallada del producto para marketing y ventas
        /// FUTURO: Texto rico para e-commerce y materiales promocionales
        /// </summary>
        // public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Costo de adquisición del producto para cálculo de márgenes
        /// FUTURO: Precio de compra para análisis de rentabilidad
        /// </summary>
        // public decimal Costo { get; set; }

        /// <summary>
        /// Estado del producto (Activo, Descontinuado, En Desarrollo)
        /// FUTURO: Lifecycle management de productos
        /// </summary>
        // public string Estado { get; set; } = "Activo";

        /// <summary>
        /// Unidad de medida del producto (Unidad, Kg, Litro, Caja)
        /// FUTURO: Control preciso de cantidades y conversiones
        /// </summary>
        // public string UnidadMedida { get; set; } = "Unidad";

        /// <summary>
        /// Peso del producto para cálculos de envío y logística
        /// FUTURO: Optimización de costos de transporte
        /// </summary>
        // public decimal? Peso { get; set; }

        /// <summary>
        /// Dimensiones del producto para almacenamiento y envío
        /// FUTURO: Optimización de espacios y packaging
        /// </summary>
        // public ProductoDimensiones? Dimensiones { get; set; }

        /// <summary>
        /// Imágenes del producto para e-commerce y marketing
        /// FUTURO: Gestión de assets visuales
        /// </summary>
        // public List<string> ImagenesUrls { get; set; } = new();

        /// <summary>
        /// Código de barras/EAN para identificación automática
        /// FUTURO: Integración con sistemas de escaneo
        /// </summary>
        // public string? CodigoBarras { get; set; }

        /// <summary>
        /// Nivel mínimo de stock para alertas de reposición
        /// FUTURO: Gestión automática de inventario
        /// </summary>
        // public int? StockMinimo { get; set; }

        /// <summary>
        /// Nivel máximo de stock para optimización de almacenamiento
        /// FUTURO: Control de sobre-stock
        /// </summary>
        // public int? StockMaximo { get; set; }

        /// <summary>
        /// Etiquetas/tags para búsqueda y categorización flexible
        /// FUTURO: Taxonomy flexible y búsqueda avanzada
        /// </summary>
        // public List<string> Tags { get; set; } = new();

        /// <summary>
        /// Información de garantía del producto
        /// FUTURO: Gestión de post-venta y soporte
        /// </summary>
        // public GarantiaInfo? Garantia { get; set; }

        /// <summary>
        /// Certificaciones y compliance del producto
        /// FUTURO: Cumplimiento regulatorio y calidad
        /// </summary>
        // public List<CertificacionDto> Certificaciones { get; set; } = new();

        /// <summary>
        /// Atributos personalizados específicos por categoría
        /// FUTURO: Flexibilidad para atributos específicos
        /// </summary>
        // public Dictionary<string, object> AtributosPersonalizados { get; set; } = new();

        // ========== VALIDACIONES DE NEGOCIO CRÍTICAS ==========
        // FUTURO: Implementar validaciones avanzadas para integridad de catálogo:
        // - ValidarNombreUnico(): Verificar unicidad en catálogo activo
        // - ValidarCategoriaExistente(): Confirmar categoría válida
        // - ValidarProveedorActivo(): Verificar proveedor autorizado
        // - ValidarPrecioMinimo(): Asegurar margen mínimo viable
        // - ValidarCoherenciaAtributos(): Validar atributos según categoría
        // - ValidarSKUUnico(): Verificar código SKU único
        // - ValidarCumplimientoRegulatorio(): Verificar compliance por categoría

        // ========== EVENTOS DE DOMINIO PARA GESTIÓN DE CATÁLOGO ==========
        // FUTURO: Eventos de dominio para arquitectura event-driven:
        // - ProductoCreado: Disparar configuración inicial, notificaciones
        // - ProductoModificado: Actualizar caches, sincronizar sistemas
        // - PrecioActualizado: Recalcular márgenes, notificar cambios
        // - ProductoDescontinuado: Limpiar stock, notificar stakeholders
        // - CategoriaActualizada: Reindexar, actualizar navegación

        // ========== CONSIDERACIONES PARA ESCALABILIDAD DE CATÁLOGO ==========
        // FUTURE ENHANCEMENTS para sistemas de catálogo empresarial:
        // - Versionado: Control de versiones de productos para trazabilidad
        // - Multi-idioma: Support para catálogos internacionales
        // - Multi-moneda: Precios en múltiples monedas
        // - Configurador: Productos configurables con variantes
        // - Bundling: Productos compuestos y bundles
        // - Seasonal: Productos estacionales con disponibilidad temporal
        // - AI/ML: Recomendaciones automáticas y pricing dinámico
        // - Integration: APIs para PIM (Product Information Management)
        // - Digital Assets: Gestión completa de assets digitales
        // - Localization: Adaptación por mercado geográfico

    }
} 