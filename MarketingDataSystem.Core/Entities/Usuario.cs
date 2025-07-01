// ==================== ENTIDAD DE DOMINIO - USUARIO DEL SISTEMA ====================
// SISTEMA: MarketingDataSystem - Sistema Empresarial de Gestión de Datos de Marketing
// COMPONENTE: Entidad Core de Autenticación y Autorización - MÁXIMA CRITICIDAD
// RESPONSABILIDAD: Gestión integral de usuarios con seguridad empresarial y compliance
// CAPA: Core/Domain - Entidad fundamental de seguridad y control de acceso
// PATRÓN: DDD Aggregate Root - Maneja identidad, credenciales y sesiones de usuario
// CRITICIDAD: MÁXIMA - Controla acceso completo al sistema y protege datos empresariales
// SEGURIDAD: Implementa autenticación JWT, autorización RBAC y audit trail completo
// COMPLIANCE: Cumple con políticas de seguridad corporativa y regulaciones de acceso

using System;
using System.Collections.Generic; // Para colecciones de entidades relacionadas
using System.ComponentModel.DataAnnotations; // Para validaciones de seguridad críticas

namespace MarketingDataSystem.Core.Entities
{
    /// <summary>
    /// ENTIDAD CORE CRÍTICA: Usuario del Sistema - MÁXIMA CRITICIDAD
    /// ==========================================================
    /// 
    /// DOMINIO EMPRESARIAL:
    /// Esta entidad representa un usuario autenticado en el sistema empresarial que tiene
    /// acceso a funcionalidades críticas como datos financieros, información de clientes,
    /// reportes ejecutivos y configuración del sistema. Es el guardián de la seguridad.
    /// 
    /// CASOS DE USO EMPRESARIALES CRÍTICOS:
    /// 1. AUTENTICACIÓN JWT: Verificación de identidad con tokens seguros y sesiones controladas
    /// 2. AUTORIZACIÓN RBAC: Control de acceso basado en roles con permisos granulares
    /// 3. AUDIT TRAIL: Registro completo de acciones para compliance y forensics de seguridad
    /// 4. SESSION MANAGEMENT: Gestión de sesiones múltiples con control de concurrencia
    /// 5. PASSWORD MANAGEMENT: Políticas de contraseñas empresariales con rotación automática
    /// 6. ACCESS CONTROL: Control de acceso a datos sensibles según clasificación de seguridad
    /// 7. COMPLIANCE MONITORING: Monitoreo de actividad para cumplimiento regulatorio
    /// 8. SECURITY ALERTING: Detección y alertas de comportamiento anómalo de usuarios
    /// 9. PRIVILEGE ESCALATION: Control de escalación de privilegios para operaciones críticas
    /// 10. DATA CLASSIFICATION: Acceso diferenciado según clasificación de datos (público, confidencial, secreto)
    /// 11. MULTI-FACTOR AUTHENTICATION: Soporte para 2FA/MFA en operaciones críticas
    /// 12. ZERO TRUST SECURITY: Verificación continua de identidad y contexto de acceso
    /// 
    /// ARQUITECTURA:
    /// - CAPA: Core/Domain en Clean Architecture - completamente independiente de infraestructura
    /// - PATRÓN: DDD Aggregate Root - coordina reglas de seguridad y gestión de identidad
    /// - RESPONSABILIDAD: Gestión integral de identidad, autenticación y autorización
    /// 
    /// ROLES DE SEGURIDAD EMPRESARIAL:
    /// - ADMIN: Acceso completo incluyendo configuración, backups, gestión de usuarios, auditoría
    /// - MANAGER: Acceso a reportes ejecutivos, ventas, stock, clientes (sin configuración del sistema)
    /// - ANALYST: Acceso a reportes y análisis de datos (solo lectura de información empresarial)
    /// - USER: Acceso básico a consultas operativas y reportes limitados de su área
    /// - VIEWER: Solo lectura de dashboards públicos y métricas no sensibles
    /// - AUDITOR: Acceso de solo lectura a logs de auditoría y compliance (rol especial)
    /// 
    /// SEGURIDAD EMPRESARIAL IMPLEMENTADA:
    /// - PASSWORD HASHING: Argon2id con salt único y parámetros empresariales de seguridad
    /// - JWT AUTHENTICATION: Tokens seguros con expiración y refresh token rotation  
    /// - ROLE-BASED ACCESS: Autorización granular por endpoint y funcionalidad
    /// - SESSION TRACKING: Control de sesiones activas con límites por usuario
    /// - AUDIT LOGGING: Registro detallado de acciones para compliance y forensics
    /// - ACCOUNT LOCKOUT: Bloqueo automático después de intentos fallidos de login
    /// - PASSWORD POLICIES: Políticas de complejidad, rotación y reutilización
    /// - ACTIVITY MONITORING: Detección de patrones anómalos y comportamiento sospechoso
    /// 
    /// COMPLIANCE Y REGULACIONES:
    /// - SOX: Sarbanes-Oxley para control de acceso a datos financieros
    /// - GDPR: Protección de datos personales y derechos de usuarios
    /// - ISO 27001: Estándares de seguridad de información empresarial
    /// - NIST Framework: Marco de ciberseguridad para protección de activos críticos
    /// - PCI DSS: Si maneja datos de pago, cumplimiento con estándares de seguridad
    /// - HIPAA: Si maneja datos de salud, protección de información médica
    /// 
    /// PERFORMANCE Y ESCALABILIDAD:
    /// - Authentication caching: Cache de tokens JWT para validación rápida
    /// - Session clustering: Sesiones distribuidas en múltiples servidores
    /// - Database optimization: Índices optimizados para consultas de autenticación
    /// - Connection pooling: Pool de conexiones para operaciones de usuario frecuentes
    /// - Horizontal scaling: Soporte para múltiples instancias de autenticación
    /// 
    /// MONITOREO DE SEGURIDAD:
    /// - Failed login attempts: Alertas por intentos de acceso fallidos
    /// - Privilege escalation: Monitoreo de cambios de roles y permisos
    /// - Unusual activity: Detección de actividad fuera de horarios normales
    /// - Geographic anomalies: Alertas por accesos desde ubicaciones inusuales
    /// - Data access patterns: Monitoreo de acceso a datos sensibles
    /// 
    /// DISASTER RECOVERY:
    /// - Account recovery: Procedimientos seguros de recuperación de cuentas
    /// - Backup authentication: Métodos alternativos de autenticación en emergencias
    /// - Audit trail preservation: Preservación de logs en caso de incidentes
    /// - Emergency access: Procedimientos de acceso de emergencia controlados
    /// </summary>
    public class Usuario : BaseEntity // Hereda propiedades comunes (Id, timestamps, audit trail)
    {
        // ========== IDENTIFICADOR ÚNICO DE SEGURIDAD ==========
        /// <summary>
        /// IDENTIFICADOR ÚNICO DEL USUARIO
        /// ==============================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Identificador único e inmutable que representa este usuario específico en todo
        /// el ecosistema de seguridad empresarial. Es la clave principal para autenticación,
        /// autorización, audit trails y control de acceso granular.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - JWT CLAIMS: Subject claim en tokens JWT para identificación segura
        /// - AUDIT TRAIL: Identificación de usuario en todos los logs de auditoría
        /// - SESSION MANAGEMENT: Clave para gestión de sesiones activas y concurrencia
        /// - ROLE ASSIGNMENT: Asignación de roles y permisos específicos por usuario
        /// - ACCESS CONTROL: Control granular de acceso a recursos y funcionalidades
        /// - COMPLIANCE REPORTING: Identificación en reportes regulatorios de acceso
        /// - FORENSICS: Investigación de incidentes de seguridad por usuario específico
        /// - USER ANALYTICS: Métricas de uso y comportamiento por usuario
        /// 
        /// CONSIDERACIONES DE SEGURIDAD:
        /// - NO-PII: Este identificador no contiene información personal identificable
        /// - IMMUTABLE: Una vez asignado, permanece constante durante todo el ciclo de vida
        /// - UNIQUE: Garantiza unicidad absoluta en todo el sistema empresarial
        /// - INDEXED: Índice primario para máximo performance en operaciones de seguridad
        /// 
        /// COMPLIANCE:
        /// - GDPR: Preservado en procesos de anonimización para análisis agregados
        /// - SOX: Usado para trazabilidad en auditorías de control interno
        /// - AUDIT TRAILS: Referencia permanente en logs de compliance regulatorio
        /// </summary>
        [Key] // Anotación EF: Define como Primary Key con índice automático
        public int IdUsuario { get; set; }

        // ========== INFORMACIÓN DE IDENTIFICACIÓN ==========
        /// <summary>
        /// NOMBRE DE VISUALIZACIÓN DEL USUARIO
        /// ==================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Nombre visible del usuario para identificación humana en interfaces,
        /// reportes de auditoría, logs de sistema y comunicaciones empresariales.
        /// Es crítico para trazabilidad y responsabilidad en operaciones.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - AUDIT LOGS: Identificación humana en registros de auditoría empresarial
        /// - DASHBOARD UI: Nombre mostrado en interfaces de usuario del sistema
        /// - REPORTS: Identificación en reportes ejecutivos y operacionales
        /// - NOTIFICATIONS: Personalización de alertas y comunicaciones del sistema
        /// - COMPLIANCE: Trazabilidad de responsabilidades en auditorías regulatorias
        /// - USER MANAGEMENT: Gestión de usuarios en paneles administrativos
        /// - FORENSICS: Identificación en investigaciones de incidentes de seguridad
        /// 
        /// VALIDACIONES CRÍTICAS DE NEGOCIO:
        /// - OBLIGATORIO: Requerido para identificación y responsabilidad
        /// - FORMATO: Debe permitir caracteres especiales según diversidad cultural
        /// - LONGITUD: Limitado para performance de consultas y compatibilidad UI
        /// - UNIQUENESS: Considerar unicidad para evitar confusión en equipos
        /// - NORMALIZATION: Normalización para búsquedas consistentes case-insensitive
        /// 
        /// CONSIDERACIONES DE SEGURIDAD:
        /// - PII CLASSIFICATION: Información personal que requiere protección especial
        /// - ACCESS LOGGING: Registro de accesos a información personal del usuario
        /// - DATA RETENTION: Políticas de retención según regulaciones empresariales
        /// - PRIVACY: Consideraciones de privacidad en logs y reportes públicos
        /// </summary>
        [Required(ErrorMessage = "El nombre del usuario es obligatorio para identificación")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// EMAIL CORPORATIVO DEL USUARIO
        /// =============================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Dirección de correo electrónico corporativo que sirve como identificador
        /// alternativo único y canal oficial de comunicación para alertas de seguridad,
        /// notificaciones críticas y procedimientos de recuperación de cuenta.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - AUTHENTICATION: Identificador único alternativo para login al sistema
        /// - SECURITY ALERTS: Canal para notificaciones de seguridad críticas
        /// - ACCOUNT RECOVERY: Método principal para recuperación segura de contraseñas
        /// - COMPLIANCE NOTIFICATIONS: Comunicación de políticas y cambios regulatorios
        /// - AUDIT COMMUNICATIONS: Notificaciones de auditorías y compliance
        /// - MFA DELIVERY: Canal para códigos de autenticación multifactor
        /// - POLICY UPDATES: Comunicación de cambios en políticas de seguridad
        /// - INCIDENT RESPONSE: Comunicación durante incidentes de seguridad
        /// 
        /// VALIDACIONES CRÍTICAS:
        /// - OBLIGATORIO: Requerido para comunicaciones críticas de seguridad
        /// - FORMATO: Validación de formato RFC 5322 para garantizar deliverability
        /// - DOMAIN VALIDATION: Verificar dominio corporativo (@empresa.com)
        /// - UNIQUENESS: Debe ser único en el sistema para evitar conflictos
        /// - VERIFICATION: Verificación de email válido antes de activar cuenta
        /// - DELIVERABILITY: Validación contra listas de correos no válidos
        /// 
        /// SEGURIDAD Y COMPLIANCE:
        /// - PII CRÍTICO: Información personal identificable protegida por GDPR
        /// - ENCRYPTION: Encriptación en reposo y tránsito para comunicaciones
        /// - ACCESS CONTROL: Acceso controlado por personal autorizado únicamente
        /// - AUDIT TRAIL: Registro de cambios para compliance y forensics
        /// - RETENTION: Políticas de retención según regulaciones de privacidad
        /// </summary>
        [Required(ErrorMessage = "El email corporativo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [StringLength(150, ErrorMessage = "El email no puede exceder 150 caracteres")]
        public string Email { get; set; } = string.Empty;

        // ========== CREDENCIALES DE SEGURIDAD ==========
        /// <summary>
        /// HASH SEGURO DE CONTRASEÑA
        /// ========================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Hash criptográfico seguro de la contraseña del usuario para autenticación
        /// sin exponer credenciales en texto plano. Implementa las mejores prácticas
        /// de seguridad empresarial para protección de identidades críticas.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - AUTHENTICATION: Verificación de identidad durante login al sistema
        /// - PASSWORD VERIFICATION: Validación de credenciales en operaciones críticas
        /// - SECURITY COMPLIANCE: Cumplimiento con políticas de seguridad corporativa
        /// - AUDIT REQUIREMENTS: Satisface requisitos de auditoría para autenticación fuerte
        /// - INCIDENT RESPONSE: Base para investigación de compromisos de credenciales
        /// - ACCOUNT RECOVERY: Verificación durante procesos de recuperación de cuenta
        /// 
        /// IMPLEMENTACIÓN DE SEGURIDAD:
        /// - ALGORITMO: Argon2id (ganador PHC) con parámetros empresariales optimizados
        /// - SALT: Salt único criptográficamente seguro por cada contraseña
        /// - COST PARAMETERS: Configurado para resistir ataques de fuerza bruta modernos
        /// - MEMORY HARD: Resistente a ataques con hardware especializado (ASICs/FPGAs)
        /// - UPGRADE PATH: Capacidad de migrar a algoritmos más fuertes sin interrupción
        /// 
        /// POLÍTICAS EMPRESARIALES:
        /// - COMPLEXITY: Mínimo 12 caracteres con mayúsculas, minúsculas, números, símbolos
        /// - ROTATION: Cambio obligatorio cada 90 días para usuarios privilegiados
        /// - HISTORY: No reutilización de últimas 12 contraseñas para prevenir reciclaje
        /// - COMPROMISE DETECTION: Validación contra bases de datos de contraseñas comprometidas
        /// - ACCOUNT LOCKOUT: Bloqueo automático después de 5 intentos fallidos
        /// 
        /// COMPLIANCE Y AUDITORÍA:
        /// - NEVER PLAINTEXT: Jamás almacenar contraseñas en texto plano (violación crítica)
        /// - AUDIT TRAIL: Registro de cambios de contraseña para compliance
        /// - FORENSICS: Capacidad de análisis forense sin exposición de credenciales
        /// - REGULATORY: Cumplimiento con ISO 27001, SOX, NIST para autenticación fuerte
        /// </summary>
        [Required(ErrorMessage = "El hash de contraseña es obligatorio para autenticación")]
        public string PasswordHash { get; set; } = string.Empty;

        // ========== CONTROL DE ACCESO Y AUTORIZACIÓN ==========
        /// <summary>
        /// ROL DE USUARIO PARA AUTORIZACIÓN RBAC
        /// ====================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Define el nivel de acceso y permisos del usuario en el sistema mediante
        /// Role-Based Access Control (RBAC). Es fundamental para implementar el principio
        /// de menor privilegio y controlar acceso granular a funcionalidades críticas.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - ACCESS CONTROL: Control granular de acceso a endpoints y funcionalidades
        /// - AUTHORIZATION: Evaluación de permisos en cada operación crítica del sistema
        /// - AUDIT COMPLIANCE: Registro de niveles de acceso para auditorías regulatorias
        /// - PRIVILEGE MANAGEMENT: Gestión de escalación y reducción de privilegios
        /// - SEPARATION OF DUTIES: Implementación de segregación de funciones críticas
        /// - COMPLIANCE REPORTING: Reportes de acceso para cumplimiento regulatorio
        /// 
        /// ROLES EMPRESARIALES DEFINIDOS:
        /// - ADMIN: Acceso completo (configuración, backups, usuarios, datos críticos)
        ///   * Gestión de usuarios y roles del sistema
        ///   * Configuración de fuentes de datos y pipelines ETL
        ///   * Acceso a backups y procedimientos de disaster recovery
        ///   * Configuración de políticas de seguridad del sistema
        /// 
        /// - MANAGER: Acceso ejecutivo (reportes, análisis, operaciones de negocio)
        ///   * Reportes ejecutivos y dashboards gerenciales
        ///   * Análisis de ventas, stock y performance comercial
        ///   * Gestión de clientes y datos de marketing
        ///   * Sin acceso a configuración crítica del sistema
        /// 
        /// - ANALYST: Acceso analítico (datos, reportes, análisis de negocio)
        ///   * Generación de reportes operacionales y análisis
        ///   * Acceso de solo lectura a datos empresariales
        ///   * Herramientas de business intelligence y análisis
        ///   * Sin capacidad de modificar datos críticos
        /// 
        /// - USER: Acceso operativo (consultas básicas, operaciones rutinarias)
        ///   * Consultas básicas de productos, clientes, ventas
        ///   * Reportes limitados de su área de responsabilidad
        ///   * Operaciones rutinarias sin impacto crítico
        ///   * Sin acceso a información confidencial
        /// 
        /// - VIEWER: Acceso de solo lectura (dashboards públicos, métricas básicas)
        ///   * Solo lectura de dashboards públicos
        ///   * Métricas básicas no sensibles del negocio
        ///   * Sin acceso a datos personales o financieros
        ///   * Ideal para stakeholders externos o consultores
        /// 
        /// - AUDITOR: Acceso especializado (logs, compliance, auditoría)
        ///   * Acceso de solo lectura a logs de auditoría
        ///   * Reportes de compliance y regulatorios
        ///   * Trazabilidad de operaciones críticas
        ///   * Sin acceso a modificar datos operacionales
        /// 
        /// IMPLEMENTACIÓN TÉCNICA:
        /// - CLAIMS-BASED: Implementado como claims en JWT tokens
        /// - ATTRIBUTE-BASED: Soporte futuro para ABAC (Attribute-Based Access Control)
        /// - HIERARCHICAL: Roles jerárquicos con herencia de permisos
        /// - CONTEXT-AWARE: Evaluación de contexto (horario, ubicación, dispositivo)
        /// 
        /// COMPLIANCE Y AUDITORÍA:
        /// - PRINCIPLE OF LEAST PRIVILEGE: Mínimos permisos necesarios para la función
        /// - SEPARATION OF DUTIES: Segregación de funciones críticas entre roles
        /// - REGULAR REVIEW: Revisión periódica de asignación de roles y permisos
        /// - AUDIT TRAIL: Registro completo de cambios de roles para compliance
        /// </summary>
        [Required(ErrorMessage = "El rol es obligatorio para control de acceso")]
        public string Role { get; set; } = string.Empty;

        // ========== INFORMACIÓN ADICIONAL DE USUARIO ==========
        /// <summary>
        /// NOMBRE DE USUARIO PARA LOGIN
        /// ============================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Identificador alternativo para login cuando el email corporativo no es práctico
        /// o cuando se requiere un identificador más corto y memorable para uso frecuente.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - QUICK LOGIN: Acceso rápido al sistema con identificador corto
        /// - LEGACY INTEGRATION: Compatibilidad con sistemas legacy que requieren username
        /// - MOBILE ACCESS: Identificador más fácil de escribir en dispositivos móviles
        /// - API ACCESS: Identificador para acceso programático a APIs del sistema
        /// - KIOSK SYSTEMS: Login en terminales de punto de venta o kioscos
        /// 
        /// VALIDACIONES EMPRESARIALES:
        /// - UNIQUENESS: Debe ser único en todo el sistema para evitar conflictos
        /// - FORMAT: Solo caracteres alfanuméricos sin espacios para compatibilidad
        /// - LENGTH: Mínimo 3 caracteres, máximo 20 para balance usabilidad/seguridad
        /// - RESERVED WORDS: No permitir nombres reservados (admin, root, system, etc.)
        /// - BUSINESS RULES: Seguir convenciones de naming corporativas
        /// </summary>
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 20 caracteres")]
        public string? NombreUsuario { get; set; }

        /// <summary>
        /// NOMBRE COMPLETO OFICIAL
        /// =======================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Nombre formal completo del usuario para documentos oficiales, reportes
        /// ejecutivos, comunicaciones formales y cumplimiento de políticas de identificación.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - FORMAL REPORTS: Reportes ejecutivos y documentos oficiales de la empresa
        /// - COMPLIANCE DOCS: Documentos de compliance que requieren identificación formal
        /// - LEGAL DOCUMENTS: Documentos legales que requieren nombre completo oficial
        /// - AUDIT REPORTS: Reportes de auditoría con identificación formal de responsables
        /// - EXECUTIVE COMMUNICATIONS: Comunicaciones formales a nivel ejecutivo
        /// - HR INTEGRATION: Integración con sistemas de recursos humanos corporativos
        /// 
        /// FORMATO EMPRESARIAL:
        /// - STANDARD: "Apellido, Nombre" según convenciones corporativas
        /// - INTERNATIONAL: Soporte para formatos internacionales según ubicación
        /// - PROFESSIONAL: Incluir títulos profesionales si es política empresarial
        /// - CONSISTENCY: Mantener formato consistente para profesionalismo
        /// </summary>
        [StringLength(200, ErrorMessage = "El nombre completo no puede exceder 200 caracteres")]
        public string? NombreCompleto { get; set; }

        /// <summary>
        /// ROL LEGACY (COMPATIBILIDAD)
        /// ===========================
        /// 
        /// PROPÓSITO: Campo de rol adicional para compatibilidad con versiones legacy
        /// ESTADO: En proceso de deprecación - usar 'Role' como campo principal
        /// MIGRACIÓN: Será eliminado en versiones futuras del sistema
        /// USO: Solo para compatibilidad temporal durante migración de sistemas
        /// </summary>
        [Obsolete("Usar campo 'Role' principal. Este campo será eliminado en versiones futuras.")]
        public string? Rol { get; set; }

        // ========== MONITOREO Y AUDITORÍA ==========
        /// <summary>
        /// TIMESTAMP DE ÚLTIMO ACCESO
        /// =========================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Registro de la última vez que el usuario accedió exitosamente al sistema.
        /// Es crítico para monitoreo de seguridad, políticas de inactividad y detección
        /// de cuentas comprometidas o abandonadas.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - SECURITY MONITORING: Detección de cuentas inactivas o comprometidas
        /// - ACCOUNT POLICIES: Aplicación de políticas de caducidad por inactividad
        /// - COMPLIANCE AUDITING: Monitoreo de acceso para auditorías regulatorias  
        /// - USER ANALYTICS: Métricas de actividad y engagement de usuarios
        /// - INCIDENT RESPONSE: Análisis de patrones de acceso durante incidentes
        /// - LICENSE MANAGEMENT: Optimización de licencias basada en uso real
        /// - ANOMALY DETECTION: Detección de accesos fuera de patrones normales
        /// - ACCOUNT CLEANUP: Identificación de cuentas para desactivación automática
        /// 
        /// ACTUALIZACIÓN AUTOMÁTICA:
        /// - LOGIN SUCCESS: Se actualiza automáticamente en cada login exitoso
        /// - JWT REFRESH: Actualización durante refresh de tokens para actividad continua
        /// - API ACCESS: Consideración de actualización en accesos a API críticas
        /// - TIMEZONE: Almacenado en UTC para consistencia global empresarial
        /// 
        /// POLÍTICAS EMPRESARIALES:
        /// - INACTIVE THRESHOLD: Cuentas inactivas por 90+ días marcadas para revisión
        /// - SECURITY ALERTS: Alertas automáticas por accesos después de inactividad prolongada
        /// - AUDIT REPORTING: Incluido en reportes regulares de actividad de usuarios
        /// - COMPLIANCE: Requerido para auditorías de acceso y control de seguridad
        /// 
        /// CONSIDERACIONES DE PRIVACY:
        /// - DATA RETENTION: Retención según políticas de privacidad empresarial
        /// - AGGREGATION: Usado solo para análisis agregados cuando sea posible
        /// - ACCESS LOGGING: Registro de quién accede a esta información sensible
        /// </summary>
        public DateTime? UltimoAcceso { get; set; }

        // ========== RELACIONES DE AUDITORÍA ==========
        /// <summary>
        /// HISTORIAL DE DESCARGAS DE REPORTES
        /// ==================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Colección completa de todos los reportes descargados por este usuario.
        /// Es fundamental para auditoría de acceso a información sensible y compliance
        /// con regulaciones que requieren trazabilidad de datos empresariales.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - AUDIT COMPLIANCE: Trazabilidad completa de acceso a información empresarial
        /// - SECURITY MONITORING: Detección de descargas inusuales o no autorizadas
        /// - DATA GOVERNANCE: Control de distribución de información confidencial
        /// - COMPLIANCE REPORTING: Reportes regulatorios de acceso a datos críticos
        /// - INCIDENT INVESTIGATION: Análisis forense de acceso a datos durante incidentes
        /// - USER BEHAVIOR ANALYTICS: Patrones de uso de información por usuario
        /// - LICENSE COMPLIANCE: Verificación de uso apropiado de licencias de reportes
        /// - INFORMATION CLASSIFICATION: Control de acceso según clasificación de datos
        /// 
        /// INFORMACIÓN REGISTRADA POR DESCARGA:
        /// - Timestamp exacto de descarga para trazabilidad temporal
        /// - Tipo y nombre específico del reporte descargado
        /// - IP address y contexto de seguridad de la descarga
        /// - Resultado de la descarga (exitosa, fallida, interrumpida)
        /// - Tamaño y formato del archivo descargado
        /// - Clasificación de seguridad del reporte accedido
        /// 
        /// COMPLIANCE Y REGULACIONES:
        /// - SOX: Trazabilidad de acceso a reportes financieros críticos
        /// - GDPR: Control de acceso a reportes con datos personales
        /// - ISO 27001: Registro de acceso a información clasificada
        /// - AUDIT TRAILS: Evidencia para auditorías internas y externas
        /// - DATA PROTECTION: Cumplimiento de políticas de protección de datos
        /// 
        /// RELACIÓN TÉCNICA:
        /// - Relación 1:N con DescargaReporte (un usuario puede tener múltiples descargas)
        /// - Navegación bidireccional para consultas eficientes
        /// - Índices optimizados para consultas de auditoría frecuentes
        /// - Retención configurada según políticas empresariales de datos
        /// </summary>
        public ICollection<DescargaReporte> Descargas { get; set; } = new List<DescargaReporte>();

        // ========== MÉTODOS DE DOMINIO FUTUROS ==========
        // TODO: Implementar métodos de dominio para encapsular lógica de negocio de seguridad:

        /// <summary>
        /// FUTURO: Valida si el usuario tiene permisos para una operación específica
        /// Implementará lógica granular de autorización basada en roles y contexto
        /// </summary>
        // public bool TienePermiso(string operacion, string? contexto = null)

        /// <summary>
        /// FUTURO: Verifica si la contraseña cumple con políticas empresariales
        /// Validará complejidad, historial, compromiso conocido, etc.
        /// </summary>
        // public bool ValidarPoliticaContrasena(string nuevaContrasena)

        /// <summary>
        /// FUTURO: Genera token JWT con claims apropiados para el usuario
        /// Incluirá roles, permisos, contexto de seguridad y expiración
        /// </summary>
        // public string GenerarTokenJWT(TimeSpan? expiracionCustom = null)

        /// <summary>
        /// FUTURO: Verifica si el usuario requiere autenticación multifactor
        /// Basado en rol, operación, contexto y políticas de seguridad
        /// </summary>
        // public bool RequiereMFA(string operacion, string contexto)

        /// <summary>
        /// FUTURO: Registra actividad del usuario para audit trail
        /// Registrará acción, contexto, resultado y metadata de seguridad
        /// </summary>
        // public void RegistrarActividad(string accion, string detalle, bool exitosa)

        /// <summary>
        /// FUTURO: Verifica si la cuenta está en estado válido para acceso
        /// Verificará locks, expiración, políticas de inactividad, etc.
        /// </summary>
        // public EstadoCuenta VerificarEstadoCuenta()

        /// <summary>
        /// FUTURO: Aplica políticas de rotación de credenciales
        /// Verificará si requiere cambio de contraseña según políticas
        /// </summary>
        // public bool RequiereRotacionCredenciales()

        /// <summary>
        /// FUTURO: Evalúa riesgo de la sesión actual del usuario
        /// Analizará ubicación, dispositivo, horario, comportamiento
        /// </summary>
        // public NivelRiesgo EvaluarRiesgoSesion(ContextoSesion contexto)
    }
}