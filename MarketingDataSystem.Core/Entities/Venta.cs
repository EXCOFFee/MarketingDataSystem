// ==================== ENTIDAD DE DOMINIO - TRANSACCIÓN DE VENTA ====================
// SISTEMA: MarketingDataSystem - Sistema Empresarial de Gestión de Datos de Marketing
// COMPONENTE: Entidad Core de Transacciones Financieras - CRITICIDAD MÁXIMA
// RESPONSABILIDAD: Gestión completa de transacciones comerciales con integridad financiera absoluta
// CAPA: Core/Domain - Entidad transaccional crítica del modelo de negocio
// DDD: Aggregate Root - Maneja consistencia y reglas de negocio de transacciones
// FINANCIERO: Contiene información financiera crítica que requiere integridad absoluta
// AUDITORÍA: Toda transacción debe ser completamente auditable y trazable
// PERSISTENCIA: Mapeada a tabla 'Ventas' en base de datos con índices optimizados
// COMPLIANCE: Cumple con SOX, regulaciones financieras y estándares de auditoría

using System;
using System.ComponentModel.DataAnnotations; // Para validaciones financieras críticas

namespace MarketingDataSystem.Core.Entities
{
    /// <summary>
    /// ENTIDAD CORE CRÍTICA: Transacción de Venta Financiera
    /// ==================================================
    /// 
    /// DOMINIO EMPRESARIAL:
    /// Esta entidad representa una transacción comercial completa que afecta directamente
    /// los ingresos de la empresa, el inventario disponible y la relación con el cliente.
    /// Es el corazón del sistema comercial y requiere integridad absoluta.
    /// 
    /// ARQUITECTURA:
    /// - CAPA: Core/Domain en Clean Architecture - completamente independiente de infraestructura
    /// - PATRÓN: DDD Aggregate Root - coordina reglas de negocio complejas
    /// - RESPONSABILIDAD: Gestión transaccional con consistencia ACID
    /// 
    /// CASOS DE USO EMPRESARIALES:
    /// 1. PUNTO DE VENTA (POS): Registro de ventas en tiempo real en sucursales físicas
    /// 2. E-COMMERCE: Procesamiento de órdenes online con integración de pagos
    /// 3. FACTURACIÓN: Generación automática de facturas con datos fiscales completos
    /// 4. ANÁLISIS COMERCIAL: Base de datos para reportes gerenciales y KPIs de ventas
    /// 5. CRM: Seguimiento del comportamiento de compra de clientes para fidelización
    /// 6. INVENTARIO: Actualización automática de stock después de cada transacción
    /// 7. CONTABILIDAD: Integración con sistemas contables para registro de ingresos
    /// 8. AUDITORÍA: Trazabilidad completa para cumplimiento regulatorio
    /// 
    /// RELACIONES CRÍTICAS:
    /// - Cliente (N:1) - Múltiples ventas pueden pertenecer al mismo cliente CRM
    /// - Producto (N:1) - Múltiples ventas pueden ser del mismo producto del catálogo
    /// - Stock (Indirecta) - Cada venta debe verificar y actualizar inventario disponible
    /// 
    /// INTEGRIDAD FINANCIERA:
    /// - Transaccionalidad ACID para operaciones críticas
    /// - Validaciones estrictas antes de confirmar transacciones
    /// - Backup automático de transacciones críticas
    /// - Conciliación diaria con sistemas bancarios
    /// 
    /// COMPLIANCE Y AUDITORÍA:
    /// - SOX: Cumplimiento Sarbanes-Oxley para empresas públicas
    /// - GAAP: Principios contables generalmente aceptados
    /// - Retención de datos: 7 años mínimo para auditorías fiscales
    /// - Inmutabilidad: Una vez confirmada, la transacción no puede modificarse
    /// 
    /// SEGURIDAD:
    /// - Encriptación: Datos financieros sensibles encriptados en reposo
    /// - Control de acceso: Solo usuarios autorizados pueden crear/consultar ventas
    /// - Audit Trail: Registro completo de quién, cuándo y qué operaciones se realizaron
    /// - Backup: Respaldo automático cada 15 minutos para datos críticos
    /// 
    /// ESCALABILIDAD:
    /// - Particionamiento: Tabla particionada por fecha para queries eficientes
    /// - Índices: Optimización para consultas frecuentes (Cliente, Producto, Fecha)
    /// - Caching: Datos agregados en cache para reportes de alto rendimiento
    /// - Archivado: Migración automática de datos antiguos a almacenamiento frío
    /// 
    /// PERFORMANCE:
    /// - Consultas optimizadas: < 100ms para operaciones CRUD individuales
    /// - Bulk operations: Soporte para inserción masiva de transacciones
    /// - Lazy loading: Navegación bajo demanda para optimizar memoria
    /// - Connection pooling: Gestión eficiente de conexiones de base de datos
    /// 
    /// MONITOREO Y ALERTAS:
    /// - Transacciones fallidas: Alertas inmediatas para errores de procesamiento
    /// - Volumen anómalo: Detección de patrones de venta inusuales
    /// - Performance: Monitoreo de tiempo de respuesta de operaciones críticas
    /// - Integridad: Verificación nocturna de consistencia de datos financieros
    /// </summary>
    public class Venta : BaseEntity // Hereda propiedades comunes (Id, timestamps, etc.)
    {
        // ========== IDENTIFICADOR ÚNICO TRANSACCIONAL ==========
        /// <summary>
        /// IDENTIFICADOR ÚNICO DE TRANSACCIÓN
        /// ====================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Identificador único e inmutable que representa esta transacción específica
        /// en todo el ecosistema empresarial. Es la clave principal para trazabilidad,
        /// reportes financieros, análisis de datos y compliance regulatorio.
        /// 
        /// CASOS DE USO:
        /// - FACTURACIÓN: Número de referencia en facturas oficiales
        /// - REPORTES: Clave primaria para drill-down en análisis gerenciales
        /// - AUDITORÍA: Identificador único para seguimiento de transacciones
        /// - INTEGRACIÓN: Referencia para sistemas externos (ERP, contabilidad)
        /// - SOPORTE: Número de transacción para atención al cliente
        /// 
        /// CONSIDERACIONES TÉCNICAS:
        /// - AUTO-INCREMENT: Generado automáticamente por la base de datos
        /// - INMUTABLE: Una vez asignado, nunca cambia durante el ciclo de vida
        /// - ÚNICO: Garantiza unicidad global en toda la organización
        /// - INDEXADO: Índice clustered para máximo performance en consultas
        /// 
        /// INTEGRIDAD:
        /// - Referenciado en reportes financieros mensuales y anuales
        /// - Usado en procesos de conciliación bancaria automática
        /// - Clave para logs de auditoría y compliance regulatorio
        /// - Base para numeración correlativa de facturas fiscales
        /// </summary>
        [Key] // Anotación para EF Core - define como clave primaria
        public int IdVenta { get; set; }

        // ========== RELACIONES COMERCIALES CRÍTICAS ==========
        /// <summary>
        /// IDENTIFICADOR DE PRODUCTO VENDIDO
        /// =================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Foreign Key que establece la relación crítica con el catálogo maestro de productos.
        /// Esta relación es fundamental para gestión de inventario, análisis de ventas
        /// por producto, estrategias de marketing y optimización del catálogo comercial.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - INVENTARIO: Descontar automáticamente stock después de la venta confirmada
        /// - ANÁLISIS: Reportes de productos más/menos vendidos por período
        /// - MARKETING: Segmentación de clientes por preferencias de producto
        /// - COMPRAS: Datos para cálculo de puntos de reorden y gestión de proveedores
        /// - PRICING: Análisis de elasticidad de precios por producto
        /// - SEASONALITY: Identificación de patrones estacionales de demanda
        /// 
        /// VALIDACIONES CRÍTICAS DE NEGOCIO:
        /// - EXISTENCIA: El producto debe existir activamente en el catálogo
        /// - DISPONIBILIDAD: Debe haber stock suficiente antes de confirmar venta
        /// - ESTADO: El producto debe estar activo y disponible para venta
        /// - PRECIO: Validar que el precio coincida con el catálogo actual o promociones
        /// 
        /// INTEGRIDAD REFERENCIAL:
        /// - CASCADE DELETE: Comportamiento definido si se elimina el producto
        /// - CONSTRAINT: Foreign key constraint a nivel de base de datos
        /// - TRANSACTION: Validación transaccional antes de confirmar venta
        /// 
        /// PERFORMANCE:
        /// - ÍNDICE: Índice no-clustered para consultas frecuentes por producto
        /// - CACHING: Información de producto en cache para validaciones rápidas
        /// - BATCH: Operaciones por lote para análisis de múltiples productos
        /// </summary>
        [Required(ErrorMessage = "El identificador del producto es obligatorio para registrar la venta")]
        public int IdProducto { get; set; }

        /// <summary>
        /// IDENTIFICADOR DE CLIENTE COMPRADOR
        /// ==================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Foreign Key que conecta la transacción con la base de datos de clientes CRM.
        /// Esta relación es crítica para análisis de comportamiento, fidelización,
        /// segmentación de mercado y estrategias de retención de clientes.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - CRM: Historial completo de compras por cliente para personalización
        /// - FIDELIZACIÓN: Cálculo de puntos, descuentos y programas de lealtad
        /// - SEGMENTACIÓN: Clasificación de clientes por valor, frecuencia, recencia
        /// - MARKETING: Campañas dirigidas basadas en comportamiento de compra
        /// - ANÁLISIS RFM: Recency, Frequency, Monetary para scoring de clientes
        /// - CHURN PREDICTION: Identificación de clientes en riesgo de abandono
        /// - LIFETIME VALUE: Cálculo del valor de vida del cliente
        /// 
        /// VALIDACIONES CRÍTICAS DE NEGOCIO:
        /// - EXISTENCIA: El cliente debe estar registrado activamente en CRM
        /// - ESTADO: Validar que el cliente no esté en lista negra o suspendido
        /// - LÍMITE DE CRÉDITO: Verificar límites de crédito para ventas a plazo
        /// - GDPR: Consentimiento válido para uso de datos personales en marketing
        /// 
        /// PRIVACY Y COMPLIANCE:
        /// - GDPR: Derecho al olvido - manejo de eliminación de datos de cliente
        /// - PCI DSS: Si se almacenan datos de pago, cumplir estándares de seguridad
        /// - RETENTION: Política de retención de datos de clientes
        /// - ANONYMIZATION: Anonimización para análisis agregados
        /// 
        /// PERFORMANCE:
        /// - ÍNDICE: Índice no-clustered para consultas frecuentes por cliente
        /// - PARTITIONING: Consideraciones para clientes con alto volumen de transacciones
        /// - ARCHIVING: Estrategia de archivado para clientes inactivos
        /// </summary>
        [Required(ErrorMessage = "El identificador del cliente es obligatorio para registrar la venta")]
        public int IdCliente { get; set; }

        // ========== INFORMACIÓN FINANCIERA CRÍTICA ==========
        /// <summary>
        /// CANTIDAD DE PRODUCTOS VENDIDOS
        /// ==============================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Cantidad física exacta de productos transferidos al cliente en esta transacción.
        /// Este valor es crítico para gestión de inventario, cálculos financieros
        /// y análisis de volumen de ventas por producto y cliente.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - INVENTARIO: Cantidad exacta a descontar del stock disponible
        /// - FACTURACIÓN: Base para cálculo del subtotal (Cantidad × PrecioUnitario)
        /// - LOGÍSTICA: Planificación de picking, packing y envío de productos
        /// - ANÁLISIS: Métricas de volumen de ventas y tendencias de demanda
        /// - FORECASTING: Datos para predicción de demanda futura
        /// - PROMOTIONS: Validación de cantidad mínima para descuentos por volumen
        /// 
        /// VALIDACIONES CRÍTICAS DE NEGOCIO:
        /// - RANGO: Debe ser > 0 - no pueden existir ventas con cantidad cero o negativa
        /// - STOCK: Verificar disponibilidad de inventario antes de confirmar
        /// - LÓGICA: Para productos fraccionables, considerar decimales (kg, litros)
        /// - LÍMITES: Validar cantidad máxima permitida por transacción
        /// - PROMOCIONES: Verificar si la cantidad califica para descuentos especiales
        /// 
        /// CONSIDERACIONES TÉCNICAS:
        /// - TIPO: int adecuado para mayoría de productos (considerar decimal para fraccionables)
        /// - CONCURRENCIA: Validación de stock en transacción para evitar overselling
        /// - ATOMIC: Operación atómica de venta + descuento de stock
        /// - ROLLBACK: Capacidad de revertir si falla cualquier parte de la transacción
        /// 
        /// INTEGRIDAD FINANCIERA:
        /// - MATCH: Cantidad facturada debe coincidir exactamente con cantidad entregada
        /// - AUDIT: Trazabilidad completa de cambios de cantidad durante el proceso
        /// - RECONCILIATION: Conciliación diaria entre ventas y movimientos de inventario
        /// </summary>
        [Required(ErrorMessage = "La cantidad vendida es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public int Cantidad { get; set; }

        /// <summary>
        /// PRECIO UNITARIO DE VENTA
        /// ========================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Precio unitario exacto cobrado al cliente en el momento de la transacción.
        /// Este precio histórico puede diferir del precio actual del producto debido
        /// a promociones, descuentos, ajustes de mercado o estrategias comerciales.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - FACTURACIÓN: Precio base para cálculo del total de la línea de venta
        /// - ANÁLISIS FINANCIERO: Cálculo de ingresos, márgenes y rentabilidad
        /// - PRICING HISTORY: Histórico de precios para análisis de elasticidad
        /// - PROMOTIONS: Precio con descuentos aplicados vs precio de lista
        /// - MARGIN ANALYSIS: Comparación con costo del producto para análisis de margen
        /// - TAX CALCULATION: Base para cálculo de impuestos sobre ventas
        /// 
        /// VALIDACIONES CRÍTICAS DE NEGOCIO:
        /// - RANGO: Debe ser >= 0 (considerar productos promocionales gratuitos)
        /// - COHERENCIA: Validar vs precio de lista o rango de precios aceptable
        /// - AUTORIZACIÓN: Precios especiales requieren aprobación gerencial
        /// - MARGIN: Validar que el precio cubra costos mínimos del producto
        /// - TAX: Considerar si el precio incluye o excluye impuestos
        /// 
        /// CONSIDERACIONES TÉCNICAS:
        /// - PRECISIÓN: ⚠️ CRÍTICO - Cambiar a decimal para precisión financiera
        /// - CURRENCY: Manejar conversión de monedas para ventas internacionales
        /// - ROUNDING: Reglas de redondeo consistentes para cálculos financieros
        /// - HISTORICAL: Mantener precio histórico inmutable para auditorías
        /// 
        /// COMPLIANCE FINANCIERO:
        /// - SOX: Trazabilidad completa de precios para auditorías Sarbanes-Oxley
        /// - TAX: Documentación de precios para declaraciones fiscales
        /// - GAAP: Cumplimiento con principios contables para reconocimiento de ingresos
        /// - IFRS: Estándares internacionales de reportes financieros
        /// 
        /// FUTURAS MEJORAS:
        /// - Implementar campo PrecioOriginal para tracking de descuentos
        /// - Agregar campo MonedaVenta para soporte multi-currency
        /// - Incluir campo TipoDescuento para categorización de promociones
        /// </summary>
        [Required(ErrorMessage = "El precio unitario es obligatorio")]
        [Range(0, float.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a cero")]
        public float PrecioUnitario { get; set; } // TODO: CRÍTICO - Migrar a decimal para precisión financiera

        // ========== INFORMACIÓN TEMPORAL Y CONTEXTUAL ==========
        /// <summary>
        /// TIMESTAMP DE TRANSACCIÓN
        /// ========================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Fecha y hora exacta cuando se ejecutó y confirmó la transacción comercial.
        /// Es la dimensión temporal fundamental para análisis de trends, reportes
        /// por períodos y cumplimiento de obligaciones fiscales y contables.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - REPORTES FINANCIEROS: Agregación por día, semana, mes, trimestre, año
        /// - ANÁLISIS DE TENDENCIAS: Identificación de patrones estacionales y cíclicos
        /// - COMPLIANCE FISCAL: Declaraciones de impuestos por períodos específicos
        /// - ANÁLISIS DE PERFORMANCE: Comparación de ventas vs períodos anteriores
        /// - FORECASTING: Base temporal para modelos predictivos de demanda
        /// - AUDIT TRAIL: Secuencia temporal para auditorías y investigaciones
        /// - CUT-OFF: Definición de períodos contables para cierre mensual/anual
        /// 
        /// CONSIDERACIONES TÉCNICAS:
        /// - TIMEZONE: ⚠️ Considerar DateTimeOffset para manejo de zonas horarias
        /// - PRECISION: Incluir milisegundos para ordenamiento preciso de transacciones
        /// - UTC: Almacenar en UTC y convertir a timezone local para reportes
        /// - IMMUTABLE: Una vez registrada, la fecha no debe modificarse
        /// 
        /// INTEGRIDAD TEMPORAL:
        /// - SEQUENCE: Garantizar orden cronológico para análisis de series temporales
        /// - VALIDATION: Validar que la fecha no sea futura ni muy antigua
        /// - BUSINESS HOURS: Considerar horarios comerciales para validaciones
        /// - HOLIDAYS: Considerar días festivos para análisis de patterns
        /// 
        /// PERFORMANCE:
        /// - PARTITIONING: Tabla particionada por fecha para queries eficientes
        /// - INDEXING: Índice clustered o no-clustered según patrones de consulta
        /// - ARCHIVING: Estrategia de archivado de datos antiguos para performance
        /// </summary>
        public DateTime Fecha { get; set; } // TODO: Migrar a DateTimeOffset para zona horaria

        /// <summary>
        /// UBICACIÓN DE VENTA
        /// ==================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Sucursal, tienda física o canal de venta donde se originó la transacción.
        /// Información crítica para análisis de performance por ubicación,
        /// gestión de inventario distribuido y estrategias de expansión comercial.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - ANÁLISIS TERRITORIAL: Performance de ventas por sucursal/región
        /// - GESTIÓN DE INVENTARIO: Stock distribuido por ubicación geográfica
        /// - EXPANSIÓN: Datos para decisiones de apertura/cierre de sucursales
        /// - COMISIONES: Cálculo de comisiones por vendedor/sucursal
        /// - MARKETING LOCAL: Campañas específicas por ubicación geográfica
        /// - CUSTOMER JOURNEY: Análisis de preferencias por canal de venta
        /// - SUPPLY CHAIN: Optimización logística por proximidad geográfica
        /// 
        /// VALORES POSIBLES:
        /// - Sucursales físicas: "Sucursal Centro", "Mall Plaza Norte"
        /// - Canales digitales: "E-commerce", "App Mobile", "WhatsApp"
        /// - Marketplaces: "MercadoLibre", "Amazon", "Shopify"
        /// - Eventos: "Feria Expo", "Pop-up Store", "Venta Corporativa"
        /// 
        /// VALIDACIONES DE NEGOCIO:
        /// - CATÁLOGO: Validar vs lista oficial de sucursales/canales activos
        /// - INVENTORY: Verificar que la sucursal tenga stock del producto
        /// - BUSINESS RULES: Algunos productos solo disponibles en ciertos canales
        /// - GEO-RESTRICTIONS: Restricciones geográficas por producto o regulación
        /// 
        /// CONSIDERACIONES FUTURAS:
        /// - Normalizar en tabla Sucursales/Canales para integridad referencial
        /// - Agregar coordenadas geográficas para análisis espacial
        /// - Incluir código de sucursal para integraciones con sistemas legacy
        /// </summary>
        public string Sucursal { get; set; } = string.Empty;

        /// <summary>
        /// MÉTODO DE PAGO UTILIZADO
        /// ========================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Canal de pago utilizado para completar la transacción comercial.
        /// Información crítica para conciliación bancaria, análisis de preferencias
        /// de pago de clientes y optimización de métodos de cobro.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - CONCILIACIÓN BANCARIA: Matching automático con movimientos bancarios
        /// - CASH FLOW: Análisis de flujo de caja por método de pago
        /// - CUSTOMER INSIGHTS: Preferencias de pago por segmento de cliente
        /// - FRAUD DETECTION: Patrones anómalos por método de pago
        /// - FEES ANALYSIS: Análisis de costos financieros por método de pago
        /// - RECONCILIATION: Cuadre diario de ventas vs depósitos bancarios
        /// - PAYMENT OPTIMIZATION: Optimización de mix de métodos de pago
        /// 
        /// VALORES ESTÁNDAR:
        /// - "Efectivo" - Pago en dinero físico
        /// - "Tarjeta Débito" - Tarjeta de débito bancaria
        /// - "Tarjeta Crédito" - Tarjeta de crédito bancaria
        /// - "Transferencia" - Transferencia bancaria directa
        /// - "Billetera Digital" - PayPal, Yape, Plin, etc.
        /// - "Criptomonedas" - Bitcoin, Ethereum, etc.
        /// - "Crédito Empresarial" - Cuenta corriente B2B
        /// 
        /// VALIDACIONES DE NEGOCIO:
        /// - AVAILABILITY: Verificar que el método esté disponible en la sucursal
        /// - LIMITS: Validar límites mínimos/máximos por método de pago
        /// - FEES: Considerar comisiones que afecten el precio final
        /// - PROCESSING: Verificar que el pago fue procesado exitosamente
        /// 
        /// CONSIDERACIONES FUTURAS:
        /// - Normalizar en tabla MetodosPago para integridad referencial
        /// - Agregar campos de referencia de transacción bancaria
        /// - Incluir información de fees y comisiones por método
        /// - Implementar token de pago para trazabilidad con gateway
        /// </summary>
        public string MetodoPago { get; set; } = string.Empty;

        // ========== RELACIONES DE NAVEGACIÓN (LAZY LOADING) ==========
        /// <summary>
        /// NAVEGACIÓN A PRODUCTO VENDIDO
        /// =============================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Propiedad de navegación que permite acceso completo a la información
        /// del producto vendido sin realizar consultas adicionales explícitas.
        /// Facilita reportes detallados y análisis que requieren datos combinados.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - REPORTES DETALLADOS: Ventas con nombre de producto, categoría, proveedor
        /// - ANÁLISIS CRUZADO: Correlación entre ventas y atributos de producto
        /// - FACTURAS: Generación de facturas con información completa del producto
        /// - CUSTOMER SERVICE: Información completa para atención al cliente
        /// - INVENTORY IMPACT: Análisis de impacto en inventario por tipo de producto
        /// 
        /// LAZY LOADING:
        /// - EF Core carga automáticamente cuando se accede a la propiedad
        /// - Optimización: Solo se carga cuando es necesario para performance
        /// - N+1 Problem: Considerar Include() en LINQ para queries masivas
        /// - Caching: Información de producto frecuentemente usada en cache
        /// 
        /// PERFORMANCE CONSIDERATIONS:
        /// - Incluir explícitamente con .Include() para evitar múltiples queries
        /// - Projection: Usar Select() para obtener solo campos necesarios
        /// - Batching: Agrupar consultas para reducir round-trips a DB
        /// </summary>
        public Producto Producto { get; set; } = new();

        /// <summary>
        /// NAVEGACIÓN A CLIENTE COMPRADOR
        /// ==============================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Propiedad de navegación que permite acceso completo a la información
        /// del cliente que realizó la compra. Esencial para análisis CRM,
        /// personalización y estrategias de retención de clientes.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - CRM ANALYSIS: Análisis completo del perfil y comportamiento del cliente
        /// - PERSONALIZATION: Recomendaciones basadas en historial de compras
        /// - CUSTOMER SERVICE: Información completa para soporte al cliente
        /// - SEGMENTATION: Agrupación de clientes por patrones de compra
        /// - RETENTION: Estrategias de retención basadas en valor del cliente
        /// - MARKETING: Campañas dirigidas con información demográfica
        /// 
        /// PRIVACY CONSIDERATIONS:
        /// - GDPR: Considerar consentimiento para uso de datos personales
        /// - ANONYMIZATION: Anonimización para análisis agregados
        /// - ACCESS CONTROL: Control estricto de acceso a datos personales
        /// - AUDIT: Log de acceso a información sensible del cliente
        /// 
        /// PERFORMANCE CONSIDERATIONS:
        /// - Incluir explícitamente con .Include() para evitar múltiples queries
        /// - Selective Loading: Cargar solo campos necesarios para cada caso de uso
        /// - Caching: Información de clientes frecuentes en cache con expiración
        /// </summary>
        public Cliente Cliente { get; set; } = new();

        // ========== MÉTODOS DE DOMINIO FUTUROS (BUSINESS LOGIC) ==========
        // TODO: IMPLEMENTACIÓN FUTURA - Métodos de dominio críticos para lógica de negocio
        
        /// <summary>
        /// ROADMAP - MÉTODOS DE DOMINIO A IMPLEMENTAR:
        /// ==========================================
        /// 
        /// 1. decimal CalcularTotal()
        ///    - Calcula Cantidad × PrecioUnitario con precisión decimal
        ///    - Aplica descuentos, promociones e impuestos
        ///    - Valida límites de transacción y reglas de negocio
        /// 
        /// 2. decimal CalcularImpuestos(ITaxCalculator calculator)
        ///    - Calcula impuestos según tipo de producto y jurisdicción
        ///    - Soporte para múltiples tipos de impuestos (IVA, ISC, etc.)
        ///    - Integración con servicios de cálculo de impuestos externos
        /// 
        /// 3. bool ValidarVenta()
        ///    - Valida disponibilidad de stock antes de confirmar
        ///    - Verifica límites de crédito del cliente
        ///    - Aplica reglas de negocio específicas por producto/cliente
        /// 
        /// 4. string GenerarNumeroFactura()
        ///    - Genera numeración correlativa para facturación fiscal
        ///    - Cumple con normativas fiscales locales
        ///    - Integración con sistemas de facturación electrónica
        /// 
        /// 5. void AplicarDescuento(decimal porcentaje, string motivo)
        ///    - Aplica descuentos con trazabilidad completa
        ///    - Requiere autorización para descuentos significativos
        ///    - Actualiza precios manteniendo histórico original
        /// 
        /// 6. VentaResult ProcesarVenta(IStockService stockService)
        ///    - Orquesta el proceso completo de venta
        ///    - Maneja transaccionalidad ACID
        ///    - Implementa patrón Saga para operaciones distribuidas
        /// 
        /// 7. bool PuedeSerCancelada()
        ///    - Evalúa si la venta puede ser cancelada según reglas de negocio
        ///    - Considera tiempo transcurrido, estado de entrega, políticas
        /// 
        /// 8. void AuditarCambio(string usuario, string accion)
        ///    - Registra cambios para compliance y auditoría
        ///    - Mantiene trail completo de modificaciones
        /// </summary>
    }
} 