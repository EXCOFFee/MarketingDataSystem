// ==================== ENTIDAD DE DOMINIO CRÍTICA - FUENTE DE DATOS ETL ====================
// Esta entidad representa configuraciones de fuentes de datos para el pipeline ETL
// CAPA: Domain (Core) - Modelo de dominio central para configuración de datos
// CRITICIDAD: MÁXIMA - Sin fuentes configuradas, el sistema no puede funcionar
// PIPELINE ETL: Base fundamental para ingesta automática de datos empresariales
// CLEAN ARCHITECTURE: Núcleo del dominio - independiente de infraestructura
// CONECTIVIDAD: Gestiona conexiones a sistemas externos críticos

// Entidad que representa una fuente de datos en el sistema
using System.Collections.Generic;       // Para colecciones de entidades relacionadas
using System.ComponentModel.DataAnnotations; // Para validaciones de modelo críticas

namespace MarketingDataSystem.Core.Entities
{
    /// <summary>
    /// Entidad de dominio que representa una fuente de datos externa para el pipeline ETL
    /// RESPONSABILIDAD: Modelar configuración de conexiones a sistemas de datos externos
    /// ARQUITECTURA: Domain Layer en Clean Architecture - modelo de negocio puro
    /// CRITICIDAD: MÁXIMA - Sin fuentes configuradas, el pipeline ETL no puede ejecutarse
    /// CASOS DE USO EMPRESARIALES:
    /// - Integración con sistemas ERP/CRM legacy para datos maestros
    /// - Conexión con APIs de e-commerce para datos de ventas en tiempo real
    /// - Ingesta de archivos CSV/Excel de campañas de marketing
    /// - Sincronización con bases de datos corporativas principales
    /// - Integración con servicios cloud (AWS S3, Azure Blob, Google Cloud Storage)
    /// - Conexión con APIs de partners comerciales y proveedores
    /// TIPOS DE FUENTES SOPORTADAS:
    /// - API: Endpoints RESTful con autenticación (OAuth2, API Keys)
    /// - DATABASE: Conexiones SQL Server, MySQL, PostgreSQL, Oracle
    /// - FILE: Archivos CSV, JSON, XML en sistemas de archivos locales/remotos
    /// - FTP/SFTP: Servidores de archivos seguros para intercambio B2B
    /// - CLOUD: AWS S3, Azure Blob Storage, Google Cloud Storage
    /// - STREAM: Kafka, RabbitMQ, Azure Service Bus para datos en tiempo real
    /// PIPELINE ETL INTEGRATION:
    /// - ValidadorService: Utiliza tipo/formato para aplicar validaciones específicas
    /// - TransformadorService: Aplica transformaciones según origen y formato
    /// - EnriquecedorService: Enriquece datos según contexto de la fuente
    /// - DeduplicadorService: Identifica duplicados entre múltiples fuentes
    /// CONFIGURACIÓN CRÍTICA:
    /// - Connection strings con credenciales seguras (encriptadas)
    /// - Formatos de datos esperados para validación automática
    /// - Horarios de disponibilidad de fuentes externas
    /// - Políticas de retry y manejo de errores por fuente
    /// RELACIONES DE DOMINIO:
    /// - DatoCrudo: Datos raw ingresados desde esta fuente
    /// - IngestionLog: Historial de ejecuciones de ingesta por fuente
    /// PRINCIPIOS DOMAIN-DRIVEN DESIGN:
    /// - Aggregate Root: FuenteDeDatos como agregado de configuración
    /// - Value Objects: Tipo, Formato, Url como objetos de valor
    /// - Invariants: Nombre único, tipo válido, configuración consistente
    /// CLEAN ARCHITECTURE: Entidad pura sin dependencias de infraestructura
    /// HERENCIA: Extiende BaseEntity para audit trail y soft delete
    /// </summary>
    public class FuenteDeDatos : BaseEntity // Hereda de BaseEntity (Id, audit trail, soft delete)
    {
        /// <summary>
        /// Identificador único de la fuente de datos en el sistema
        /// PROPÓSITO: Primary Key para identificación única y referencias
        /// GENERACIÓN: Auto-increment en base de datos
        /// REFERENCIAS: Usado en logs de ingesta, configuración de ETL, auditoría
        /// RELACIONES: Foreign Key en DatoCrudo e IngestionLog
        /// </summary>
        [Key] // Clave primaria
        public int IdFuente { get; set; } // Identificador único de la fuente de datos
        
        /// <summary>
        /// Nombre descriptivo de la fuente de datos - IDENTIFICACIÓN EMPRESARIAL
        /// PROPÓSITO: Identificación humana clara en configuraciones y logs
        /// FORMATO: Descriptivo del sistema origen (ej: "ERP SAP Ventas", "API Shopify Store")
        /// VALIDACIÓN: Requerido y único para evitar confusiones
        /// CASOS DE USO: Dashboards de configuración, logs de ingesta, troubleshooting
        /// EJEMPLOS: "Sistema POS Principal", "Base Datos Clientes CRM", "API E-commerce"
        /// </summary>
        [Required]
        public string Nombre { get; set; } = string.Empty; // Nombre de la fuente de datos
        
        /// <summary>
        /// Tipo de tecnología de la fuente de datos - CLASIFICACIÓN TÉCNICA
        /// PROPÓSITO: Determinar estrategia de conexión y procesamiento ETL
        /// VALORES VÁLIDOS: "API", "DATABASE", "FILE", "FTP", "CLOUD", "STREAM"
        /// PIPELINE ETL: Usado por servicios para aplicar lógica específica por tipo
        /// CASOS DE USO:
        /// - ValidadorService: Aplicar validaciones específicas según tipo
        /// - TransformadorService: Usar parser apropiado (JSON, CSV, XML, SQL)
        /// - Conectores: Seleccionar driver/cliente de conexión correcto
        /// EJEMPLOS: "API" para REST endpoints, "DATABASE" para SQL, "FILE" para CSV/JSON
        /// </summary>
        public string Tipo { get; set; } = string.Empty; // Tipo de fuente (ej: API, archivo, etc)
        
        /// <summary>
        /// Descripción del origen o sistema fuente - CONTEXTO EMPRESARIAL
        /// PROPÓSITO: Proporcionar contexto del sistema origen para documentación
        /// CONTENIDO: Información sobre el sistema externo y propósito de datos
        /// CASOS DE USO: Documentación técnica, onboarding nuevos desarrolladores
        /// EJEMPLOS: "Sistema ERP principal con datos maestros de productos y clientes",
        ///           "API de plataforma e-commerce con transacciones en tiempo real"
        /// </summary>
        public string Origen { get; set; } = string.Empty; // Origen de los datos
        
        /// <summary>
        /// Formato de datos esperado de la fuente - CONFIGURACIÓN DE PARSEO
        /// PROPÓSITO: Definir estrategia de parseo y transformación de datos
        /// VALORES COMUNES: "JSON", "CSV", "XML", "SQL", "BINARY", "TEXT"
        /// PIPELINE ETL: Usado por TransformadorService para seleccionar parser
        /// VALIDACIÓN: ValidadorService verifica que datos recibidos coincidan
        /// CASOS DE USO: Configurar parsers automáticos, validar estructura de datos
        /// EJEMPLOS: "JSON" para APIs REST, "CSV" para exports Excel, "XML" para SOAP
        /// </summary>
        public string Formato { get; set; } = string.Empty; // Formato de los datos (ej: JSON, CSV)
        
        /// <summary>
        /// Descripción detallada de la fuente y su propósito - DOCUMENTACIÓN
        /// PROPÓSITO: Documentar propósito empresarial y características técnicas
        /// CONTENIDO: Información sobre datos proporcionados, frecuencia, criticidad
        /// CASOS DE USO: Documentación técnica, análisis de impacto, troubleshooting
        /// EJEMPLO: "Fuente principal de datos de ventas diarias. Actualizada cada hora.
        ///           Crítica para reportes ejecutivos y dashboards en tiempo real."
        /// </summary>
        public string Descripcion { get; set; } = string.Empty; // Descripción de la fuente
        
        /// <summary>
        /// URL o string de conexión a la fuente de datos - CONFIGURACIÓN DE ACCESO
        /// PROPÓSITO: Información técnica para establecer conexión con fuente externa
        /// TIPOS POR FUENTE:
        /// - API: https://api.ejemplo.com/v1/datos
        /// - DATABASE: Server=servidor;Database=db;User=user;Password=***
        /// - FILE: C:\datos\archivo.csv o \\servidor\compartido\datos.json
        /// - FTP: ftp://servidor.com/directorio/ con credenciales separadas
        /// SEGURIDAD: Passwords y API keys deben estar encriptados en base de datos
        /// CASOS DE USO: Establece conexión durante ejecución del pipeline ETL
        /// </summary>
        public string Url { get; set; } = string.Empty; // URL de la fuente (si aplica)
        
        /// <summary>
        /// Colección de datos crudos ingresados desde esta fuente - TRAZABILIDAD
        /// RELACIÓN: Uno a muchos (FuenteDeDatos → DatoCrudo)
        /// PROPÓSITO: Rastrear todos los datos raw ingresados desde esta fuente
        /// CASOS DE USO:
        /// - Auditoría: Determinar origen de cada dato en el sistema
        /// - Troubleshooting: Identificar problemas por fuente específica
        /// - Analytics: Métricas de volumen y calidad por fuente
        /// - Rollback: Revertir datos de fuente problemática
        /// PERFORMANCE: Puede ser voluminosa, considerar paginación en consultas
        /// </summary>
        // Relación con datos crudos
        public ICollection<DatoCrudo> DatosCrudos { get; set; } = new List<DatoCrudo>(); // Datos crudos asociados a la fuente
        
        /// <summary>
        /// Colección de logs de ingesta de esta fuente - MONITOREO Y AUDITORÍA
        /// RELACIÓN: Uno a muchos (FuenteDeDatos → IngestionLog)
        /// PROPÓSITO: Historial completo de ejecuciones de ingesta por fuente
        /// INFORMACIÓN TRACKING:
        /// - Fechas y horarios de cada ejecución
        /// - Éxito/fallo de cada intento de ingesta
        /// - Número de registros procesados por ejecutión
        /// - Mensajes de error específicos por fuente
        /// - Duración de procesamiento por fuente
        /// CASOS DE USO:
        /// - Monitoreo: Detectar fuentes problemáticas o intermitentes
        /// - Performance: Analizar tiempos de procesamiento por fuente
        /// - SLA: Verificar cumplimiento de acuerdos de nivel de servicio
        /// - Troubleshooting: Diagnosticar problemas específicos por fuente
        /// </summary>
        public ICollection<IngestionLog> Logs { get; set; } = new List<IngestionLog>();

        // ========== PROPIEDADES FUTURAS PARA FUNCIONALIDAD EMPRESARIAL ==========
        // TODO: Implementar propiedades adicionales para configuración avanzada:

        /// <summary>
        /// Credenciales de acceso encriptadas para la fuente
        /// FUTURO: Almacenamiento seguro de API keys, usuarios, passwords
        /// </summary>
        // public string? CredencialesEncriptadas { get; set; }

        /// <summary>
        /// Configuración de horarios de disponibilidad de la fuente
        /// FUTURO: Definir ventanas de tiempo para ingesta (ej: solo horario comercial)
        /// </summary>
        // public HorarioDisponibilidad? Horarios { get; set; }

        /// <summary>
        /// Configuración de políticas de retry para esta fuente
        /// FUTURO: Número de reintentos, intervalos, backoff exponencial
        /// </summary>
        // public PoliticasRetry? ConfiguracionRetry { get; set; }

        /// <summary>
        /// Configuración de validaciones específicas para esta fuente
        /// FUTURO: Reglas de validación customizadas por fuente
        /// </summary>
        // public ICollection<ReglaValidacion> ReglasValidacion { get; set; } = new List<ReglaValidacion>();

        /// <summary>
        /// Métricas de performance histórica de la fuente
        /// FUTURO: Estadísticas de throughput, latencia, disponibilidad
        /// </summary>
        // public MetricasFuente? Metricas { get; set; }

        /// <summary>
        /// Estado actual de la fuente (Activa, Inactiva, En Mantenimiento)
        /// FUTURO: Control de estado para management operacional
        /// </summary>
        // public EstadoFuente Estado { get; set; } = EstadoFuente.Activa;

        /// <summary>
        /// Configuración de transformaciones específicas para esta fuente
        /// FUTURO: Mapeo de campos, transformaciones custom por fuente
        /// </summary>
        // public ICollection<ConfiguracionTransformacion> ConfiguracionesTransformacion { get; set; } = new List<ConfiguracionTransformacion>();

        // ========== MÉTODOS DE DOMINIO PARA LÓGICA DE NEGOCIO ==========
        // TODO: Implementar métodos de dominio para encapsular lógica:

        /// <summary>
        /// Valida si la configuración de la fuente es correcta y completa
        /// </summary>
        // public bool EsConfiguracionValida()
        // {
        //     // Lógica para validar que todos los campos requeridos estén presentes
        //     // y sean consistentes según el tipo de fuente
        // }

        /// <summary>
        /// Determina si la fuente está actualmente disponible para ingesta
        /// </summary>
        // public bool EstaDisponible()
        // {
        //     // Lógica basada en horarios, estado, último acceso exitoso
        // }

        /// <summary>
        /// Obtiene la configuración de conexión específica para el tipo de fuente
        /// </summary>
        // public ConfiguracionConexion ObtenerConfiguracionConexion()
        // {
        //     // Lógica para construir configuración según tipo (API, DB, File, etc.)
        // }

        /// <summary>
        /// Registra un evento de ingesta exitosa
        /// </summary>
        // public void RegistrarIngestaExitosa(int registrosProcesados)
        // {
        //     // Actualiza métricas, último acceso, estadísticas
        // }

        /// <summary>
        /// Registra un evento de falla en ingesta
        /// </summary>
        // public void RegistrarFallaIngesta(string mensajeError)
        // {
        //     // Registra error, actualiza métricas de disponibilidad
        // }

        // ========== INVARIANTES DE DOMINIO PARA INTEGRIDAD ==========
        // DOMAIN-DRIVEN DESIGN: Invariantes que la entidad debe mantener:
        // - Nombre debe ser único en el sistema
        // - Tipo debe ser uno de los valores válidos del enum
        // - Url no puede estar vacía si el tipo requiere conexión
        // - Formato debe ser consistente con el tipo de fuente
        // - Activo (heredado de BaseEntity) determina si se usa en ETL

        // ========== EVENTOS DE DOMINIO PARA ARQUITECTURA EVENT-DRIVEN ==========
        // FUTURO: Eventos de dominio para comunicación entre bounded contexts:
        // - FuenteConfigurada: Notificar que nueva fuente está lista para ETL
        // - FuenterModificada: Actualizar configuraciones de servicios ETL
        // - FuenteDeshabilitada: Limpiar caches, suspender jobs programados
        // - FuenteConError: Disparar alertas, escalamiento a soporte
        // - FuenteRecuperada: Notificar que fuente problemática está operativa
    }
} 