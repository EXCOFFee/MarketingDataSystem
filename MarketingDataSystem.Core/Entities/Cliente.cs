// ==================== ENTIDAD DE DOMINIO - CLIENTE CRM ====================
// SISTEMA: MarketingDataSystem - Sistema Empresarial de Gestión de Datos de Marketing
// COMPONENTE: Entidad Core de Customer Relationship Management - ALTA CRITICIDAD
// RESPONSABILIDAD: Gestión integral de clientes con compliance GDPR y estrategias CRM
// CAPA: Core/Domain - Entidad central del negocio con reglas y relaciones críticas
// PATRÓN: Domain Entity - representa conceptos de negocio con identidad única
// ORM: Entity Framework mapeará esta clase a tabla en base de datos con encriptación
// RELACIONES: Un cliente puede tener múltiples ventas (1:N) con historial completo
// PRIVACY: Contiene PII que requiere protección especial según GDPR/LGPD/CCPA

using System.Collections.Generic; // Para colecciones de entidades relacionadas
using System.ComponentModel.DataAnnotations; // Para anotaciones de validación y mapeo

namespace MarketingDataSystem.Core.Entities
{
    /// <summary>
    /// ENTIDAD CORE CRÍTICA: Cliente CRM - MÁXIMA CRITICIDAD
    /// ===================================================
    /// 
    /// DOMINIO EMPRESARIAL:
    /// Esta entidad representa un cliente en el sistema CRM, que es el núcleo de la
    /// estrategia comercial y de marketing. Contiene información personal identificable
    /// crítica que debe manejarse con máximos estándares de seguridad y compliance.
    /// 
    /// CASOS DE USO EMPRESARIALES CRÍTICOS:
    /// 1. CUSTOMER RELATIONSHIP MANAGEMENT: Gestión integral de relaciones con clientes
    /// 2. SALES TRACKING: Seguimiento completo del historial de compras y transacciones
    /// 3. MARKETING CAMPAIGNS: Segmentación y targeting para campañas personalizadas
    /// 4. CUSTOMER ANALYTICS: Análisis de comportamiento y patrones de compra
    /// 5. LOYALTY PROGRAMS: Gestión de programas de fidelización y retención
    /// 6. CUSTOMER SERVICE: Soporte al cliente con historial completo de interacciones
    /// 7. CHURN PREDICTION: Identificación de clientes en riesgo de abandono
    /// 8. LIFETIME VALUE: Cálculo del valor de vida del cliente para estrategias
    /// 9. CROSS-SELLING: Identificación de oportunidades de venta cruzada
    /// 10. UP-SELLING: Estrategias de incremento de valor por cliente
    /// 11. CUSTOMER SEGMENTATION: Segmentación por valor, comportamiento, demografía
    /// 12. PERSONALIZATION: Personalización de experiencias y ofertas
    /// 
    /// ARQUITECTURA:
    /// - CAPA: Core/Domain en Clean Architecture - completamente independiente de infraestructura
    /// - PATRÓN: DDD Aggregate Root - maneja consistencia y reglas de negocio del cliente
    /// - RESPONSABILIDAD: Gestión integral de datos del cliente con validaciones de negocio
    /// 
    /// RELACIONES CRÍTICAS:
    /// - Ventas (1:N) - Un cliente puede realizar múltiples transacciones de venta
    /// - Interacciones (1:N) - Historial de contactos y comunicaciones con el cliente
    /// - Segmentos (N:M) - Un cliente puede pertenecer a múltiples segmentos de marketing
    /// - Programas de Lealtad (N:M) - Participación en programas de fidelización
    /// 
    /// PRIVACY Y COMPLIANCE GDPR:
    /// - Consentimiento: Gestión de consentimientos para uso de datos personales
    /// - Derecho al olvido: Capacidad de anonimizar/eliminar datos del cliente
    /// - Portabilidad de datos: Exportación de datos del cliente en formato estándar
    /// - Rectificación: Capacidad de corregir datos incorrectos del cliente
    /// - Minimización de datos: Solo recopilar datos necesarios para el negocio
    /// - Encriptación: Datos sensibles encriptados en reposo y en tránsito
    /// - Audit trail: Registro completo de acceso y modificaciones a datos del cliente
    /// 
    /// SEGURIDAD EMPRESARIAL:
    /// - PII Protection: Protección especial para información personal identificable
    /// - Access Control: Control estricto de acceso a datos sensibles del cliente
    /// - Data Masking: Enmascaramiento de datos en entornos no productivos
    /// - Backup Security: Respaldos encriptados con retención controlada
    /// - Breach Detection: Monitoreo de accesos anómalos a datos de clientes
    /// 
    /// CUSTOMER ANALYTICS:
    /// - RFM Analysis: Recency, Frequency, Monetary para scoring de clientes
    /// - CLV Calculation: Customer Lifetime Value para estrategias de inversión
    /// - Churn Scoring: Puntuación de riesgo de abandono del cliente
    /// - Propensity Modeling: Modelos de propensión de compra por categoría
    /// - Behavioral Segmentation: Segmentación basada en comportamiento de compra
    /// 
    /// PERFORMANCE Y ESCALABILIDAD:
    /// - Búsqueda optimizada: Índices para búsquedas rápidas por nombre, email
    /// - Customer 360: Vista integral del cliente con datos agregados
    /// - Real-time updates: Actualización en tiempo real de datos del cliente
    /// - Particionamiento: Estrategias de particionamiento para grandes volúmenes
    /// - Caching: Datos de clientes frecuentemente consultados en cache
    /// 
    /// COMPLIANCE REGULATORIO:
    /// - GDPR: General Data Protection Regulation (Europa)
    /// - LGPD: Lei Geral de Proteção de Dados (Brasil)
    /// - CCPA: California Consumer Privacy Act (California, USA)
    /// - SOX: Sarbanes-Oxley para empresas públicas con datos financieros
    /// - PCI DSS: Si se almacenan datos de pago
    /// </summary>
    public class Cliente : BaseEntity // Hereda propiedades comunes (timestamps, soft delete, etc.)
    {
        // ========== IDENTIFICADOR ÚNICO DEL CLIENTE ==========
        /// <summary>
        /// IDENTIFICADOR ÚNICO DEL CLIENTE
        /// ==============================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Identificador único e inmutable que representa este cliente específico en todo
        /// el ecosistema CRM y comercial. Es la clave principal para todas las relaciones
        /// comerciales, análisis de comportamiento y estrategias de marketing.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - CRM INTEGRATION: Referencia única en todos los sistemas CRM y comerciales
        /// - SALES TRACKING: Identificación del cliente en todas las transacciones
        /// - CUSTOMER SERVICE: Número de cliente para soporte y atención al cliente
        /// - ANALYTICS: Clave para análisis de comportamiento y lifetime value
        /// - MARKETING: Identificación para campañas personalizadas y segmentación
        /// - LOYALTY PROGRAMS: Número de membresía en programas de fidelización
        /// - COMPLIANCE: Identificador para gestión de consentimientos y derechos GDPR
        /// 
        /// CONSIDERACIONES TÉCNICAS:
        /// - AUTO-INCREMENT: Generado automáticamente por la base de datos
        /// - INMUTABLE: Una vez asignado, permanece constante durante todo el ciclo de vida
        /// - ÚNICO: Garantiza unicidad absoluta en todo el sistema CRM
        /// - INDEXADO: Índice clustered para máximo performance en consultas
        /// 
        /// PRIVACY Y SEGURIDAD:
        /// - NO-PII: Este identificador no contiene información personal identificable
        /// - AUDIT TRAIL: Usado para rastrear accesos y modificaciones a datos del cliente
        /// - ANONYMIZATION: Se preserva en procesos de anonimización para análisis
        /// - INTEGRATION: Referencia segura para integraciones con sistemas externos
        /// </summary>
        [Key] // Anotación EF: Define como Primary Key en base de datos
        public int IdCliente { get; set; } // Identificador único auto-incremental

        // ========== INFORMACIÓN PERSONAL IDENTIFICABLE (PII) ==========
        /// <summary>
        /// NOMBRE COMPLETO DEL CLIENTE
        /// ==========================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Nombre completo del cliente para identificación personal, comunicación
        /// y personalización de la experiencia. Es información crítica para
        /// atención al cliente y marketing personalizado.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - CUSTOMER SERVICE: Identificación personal en atención al cliente
        /// - PERSONALIZATION: Personalización de comunicaciones y ofertas
        /// - MARKETING: Saludo personalizado en emails y campañas
        /// - FACTURAS: Nombre del cliente en documentos fiscales oficiales
        /// - CUSTOMER EXPERIENCE: Experiencia personalizada en puntos de venta
        /// - LOYALTY PROGRAMS: Identificación personal en programas de fidelización
        /// 
        /// PRIVACY Y COMPLIANCE:
        /// - PII CRÍTICO: Información personal identificable que requiere protección especial
        /// - GDPR: Sujeto a derechos de rectificación, portabilidad y olvido
        /// - ENCRIPTACIÓN: Debe encriptarse en reposo y en tránsito
        /// - CONSENTIMIENTO: Requiere consentimiento explícito para uso en marketing
        /// - MINIMIZACIÓN: Solo recopilar si es necesario para el propósito comercial
        /// 
        /// VALIDACIONES CRÍTICAS:
        /// - OBLIGATORIO: Requerido para identificación del cliente
        /// - FORMATO: Validar formato apropiado según regulaciones locales
        /// - LONGITUD: Considerar nombres largos en diferentes culturas
        /// - CARACTERES: Permitir caracteres especiales según idiomas locales
        /// - NORMALIZACIÓN: Estandarizar formato para búsquedas consistentes
        /// 
        /// CONSIDERACIONES TÉCNICAS:
        /// - INDEXING: Índice para búsquedas frecuentes por nombre
        /// - FUZZY SEARCH: Búsqueda aproximada para variaciones del nombre
        /// - NORMALIZATION: Normalización para búsquedas case-insensitive
        /// - AUDITING: Log de accesos y modificaciones para compliance
        /// </summary>
        [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty; // Nombre completo del cliente

        /// <summary>
        /// EMAIL DEL CLIENTE
        /// ================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Dirección de correo electrónico del cliente para comunicación oficial,
        /// marketing digital, notificaciones de transacciones y soporte al cliente.
        /// Es el canal principal de comunicación digital con el cliente.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - COMMUNICATION: Canal principal para comunicación con el cliente
        /// - EMAIL MARKETING: Envío de campañas de marketing y promociones
        /// - TRANSACTIONAL: Confirmaciones de compra, facturas, recibos
        /// - CUSTOMER SERVICE: Soporte al cliente y seguimiento de tickets
        /// - PASSWORD RECOVERY: Recuperación de contraseñas y autenticación
        /// - LOYALTY PROGRAMS: Comunicación de beneficios y recompensas
        /// - NOTIFICATIONS: Alertas y notificaciones importantes del sistema
        /// - SURVEYS: Encuestas de satisfacción y feedback del cliente
        /// 
        /// PRIVACY Y COMPLIANCE:
        /// - PII CRÍTICO: Información personal identificable protegida por GDPR
        /// - OPT-IN/OPT-OUT: Gestión de consentimientos para marketing por email
        /// - PORTABILIDAD: Incluido en exportaciones de datos del cliente
        /// - RECTIFICACIÓN: Cliente puede actualizar su dirección de email
        /// - SPAM COMPLIANCE: Cumplimiento con leyes anti-spam (CAN-SPAM, GDPR)
        /// 
        /// VALIDACIONES CRÍTICAS:
        /// - OBLIGATORIO: Requerido para comunicación con el cliente
        /// - FORMATO: Validación de formato de email estándar (RFC 5322)
        /// - UNICIDAD: Debe ser único en el sistema para evitar duplicados
        /// - VERIFICACIÓN: Verificación de email válido antes de activar cuenta
        /// - BLACKLIST: Validación contra listas de emails no válidos
        /// 
        /// CONSIDERACIONES TÉCNICAS:
        /// - INDEXING: Índice único para búsquedas rápidas y validación de unicidad
        /// - NORMALIZATION: Conversión a lowercase para consistencia
        /// - VALIDATION: Validación en tiempo real de formato y existencia
        /// - DELIVERABILITY: Integración con servicios de validación de email
        /// - BOUNCE HANDLING: Manejo de emails rebotados y actualización de estado
        /// </summary>
        [Required(ErrorMessage = "El email del cliente es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(255, ErrorMessage = "El email no puede exceder 255 caracteres")]
        public string Email { get; set; } = string.Empty; // Correo electrónico para comunicación

        // ========== PROPIEDADES NAVEGACIONALES (RELACIONES) ==========
        /// <summary>
        /// HISTORIAL DE TRANSACCIONES DEL CLIENTE
        /// ======================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Colección completa de todas las transacciones de venta realizadas por este cliente.
        /// Es fundamental para análisis de comportamiento, cálculo de lifetime value,
        /// programas de fidelización y estrategias de retención.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - CUSTOMER ANALYTICS: Análisis completo del comportamiento de compra
        /// - LIFETIME VALUE: Cálculo del valor total del cliente para el negocio
        /// - PURCHASE HISTORY: Historial completo de compras para customer service
        /// - LOYALTY CALCULATION: Base para cálculo de puntos y beneficios de fidelización
        /// - CHURN PREDICTION: Análisis de patrones para predecir abandono del cliente
        /// - CROSS-SELLING: Identificación de oportunidades de venta cruzada
        /// - SEASONAL ANALYSIS: Análisis de patrones estacionales de compra
        /// - CUSTOMER SEGMENTATION: Segmentación basada en historial de compras
        /// - RFM ANALYSIS: Recency, Frequency, Monetary para scoring del cliente
        /// - PERSONALIZATION: Personalización de ofertas basada en compras previas
        /// 
        /// RELACIÓN DE DATOS:
        /// - CARDINALIDAD: 1:N - Un cliente puede realizar múltiples ventas
        /// - INTEGRIDAD: Cada venta debe estar asociada a un cliente válido
        /// - HISTORICAL DATA: Preserva todo el historial comercial del cliente
        /// - PERFORMANCE METRICS: Base para cálculo de KPIs de cliente
        /// 
        /// NAVEGACIÓN LAZY LOADING:
        /// - EF CORE: Carga automática cuando se accede a la colección por primera vez
        /// - PERFORMANCE: Considerar paginación para clientes con alto volumen de compras
        /// - BATCH LOADING: Usar .Include() para cargar explícitamente cuando necesario
        /// - SELECTIVE LOADING: Cargar solo transacciones de períodos específicos
        /// 
        /// OPTIMIZACIÓN DE PERFORMANCE:
        /// - PAGINATION: Implementar paginación para clientes con muchas transacciones
        /// - DATE FILTERING: Filtros por fecha para limitar volumen de datos cargados
        /// - AGGREGATION: Cálculos agregados directamente en base de datos
        /// - CACHING: Métricas de cliente frecuentemente consultadas en cache
        /// - ARCHIVING: Considerar archivado de transacciones muy antiguas
        /// 
        /// PRIVACY CONSIDERATIONS:
        /// - GDPR: Incluido en proceso de portabilidad de datos del cliente
        /// - RIGHT TO ERASURE: Consideraciones especiales para derecho al olvido
        /// - ANONYMIZATION: Posible anonimización manteniendo valor analítico
        /// - AUDIT TRAIL: Registro de accesos a historial de compras del cliente
        /// </summary>
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>(); // Inicialización para evitar null

        // ========== MÉTODOS DE DOMINIO FUTUROS (BUSINESS LOGIC) ==========
        /// <summary>
        /// ROADMAP - MÉTODOS DE DOMINIO A IMPLEMENTAR:
        /// ==========================================
        /// 
        /// 1. decimal CalcularLifetimeValue()
        ///    - Calcula el valor total del cliente basado en todas sus transacciones
        ///    - Considera frequency, monetary value y proyecciones futuras
        ///    - Base para estrategias de inversión en retención del cliente
        /// 
        /// 2. CustomerSegment DeterminarSegmento(ISegmentationService segmentationService)
        ///    - Determina el segmento del cliente basado en comportamiento y valor
        ///    - Considera RFM analysis, demografía y patrones de compra
        ///    - Base para personalización y targeting de marketing
        /// 
        /// 3. ChurnRisk CalcularRiesgoAbandonamiento()
        ///    - Calcula probabilidad de que el cliente abandone el negocio
        ///    - Considera recency de compras, frecuencia y cambios en patrones
        ///    - Base para estrategias proactivas de retención
        /// 
        /// 4. IEnumerable<Producto> GenerarRecomendaciones(IRecommendationEngine engine)
        ///    - Genera recomendaciones personalizadas de productos para el cliente
        ///    - Considera historial de compras, preferencias y tendencias
        ///    - Implementa algoritmos de collaborative filtering y content-based
        /// 
        /// 5. bool EsClientePremium(IPremiumCriteria criteria)
        ///    - Determina si el cliente califica como premium según criterios del negocio
        ///    - Considera lifetime value, frequency, recency y tipo de productos
        ///    - Base para beneficios especiales y atención preferencial
        /// 
        /// 6. CustomerHealthScore CalcularHealthScore()
        ///    - Calcula puntuación de salud de la relación con el cliente
        ///    - Considera engagement, satisfaction, transaction patterns
        ///    - Base para estrategias de customer success y retention
        /// 
        /// 7. NextPurchasePrediction PredecirProximaCompra(IForecastingService forecasting)
        ///    - Predice cuándo el cliente realizará su próxima compra
        ///    - Considera patrones históricos, estacionalidad y lifecycle
        ///    - Base para timing óptimo de campañas de marketing
        /// 
        /// 8. CustomerConsentStatus ValidarConsentimientos()
        ///    - Valida estado actual de todos los consentimientos GDPR del cliente
        ///    - Considera marketing, analytics, sharing con terceros
        ///    - Esencial para compliance y marketing legal
        /// 
        /// 9. CustomerProfile GenerarPerfil360()
        ///    - Genera vista completa 360 grados del cliente
        ///    - Incluye demographics, behavior, preferences, interactions
        ///    - Base para customer service y experience personalizada
        /// 
        /// 10. CrossSellOpportunity IdentificarOportunidadesCruzadas()
        ///     - Identifica oportunidades de cross-selling basadas en patrones
        ///     - Considera productos complementarios y comportamiento similar
        ///     - Base para estrategias de incremento de revenue per customer
        /// </summary>
        // TODO: Implementar métodos de dominio críticos para Customer Relationship Management
    }
} 