// ==================== DATA TRANSFER OBJECT CRÍTICO - GESTIÓN DE USUARIOS DEL SISTEMA ====================
// Este DTO maneja datos de usuarios que controlan acceso y autorización empresarial
// CAPA: Application/Core - Contrato de transferencia de datos de usuarios
// CRITICIDAD: MÁXIMA - Controla acceso, seguridad y autorización del sistema
// RESPONSABILIDAD: Transferencia segura de datos de usuarios sin información sensible
// CLEAN ARCHITECTURE: DTO puro sin dependencias externas para portabilidad
// SEGURIDAD: No incluye passwords ni datos sensibles para transferencia

using System;

// DTO que representa un usuario en el sistema (para autenticación y gestión)
namespace MarketingDataSystem.Core.DTOs
{
    /// <summary>
    /// Data Transfer Object para usuarios del sistema - CONTROL DE ACCESO CRÍTICO
    /// RESPONSABILIDAD: Transferir datos de usuarios de forma segura entre capas del sistema
    /// ARQUITECTURA: Application Layer DTO en Clean Architecture
    /// CRITICIDAD: MÁXIMA - Controla acceso completo al sistema y autorización de operaciones
    /// CASOS DE USO EMPRESARIALES:
    /// - Autenticación: Datos del usuario después de login exitoso
    /// - Autorización: Información de rol para control de acceso granular
    /// - Administración: Gestión de usuarios por administradores del sistema
    /// - Auditoría: Identificación de usuarios en logs y tracking de operaciones
    /// - Personalización: Datos para customización de interfaz de usuario
    /// - Comunicación: Email para notificaciones y comunicaciones empresariales
    /// - Multi-tenancy: Soporte para organizaciones y tenants múltiples
    /// - APIs REST: Serialización JSON para endpoints de gestión de usuarios
    /// SEGURIDAD IMPLEMENTADA:
    /// - Sin contraseñas: NUNCA incluye hashes de password ni datos sensibles
    /// - Información mínima: Solo datos necesarios para operación y UI
    /// - Sanitización: Datos seguros para logging y transferencia
    /// - Autorización: Información de rol para verificaciones de permisos
    /// - Audit Trail: Trazabilidad completa de acciones por usuario
    /// COMPLIANCE Y AUDITORÍA:
    /// - GDPR: Transferencia controlada de datos personales
    /// - SOX: Tracking de usuarios para compliance regulatorio
    /// - Logs: Identificación clara en logs de aplicación y auditoría
    /// - Data Protection: Sin exposición de información sensible
    /// JSON SERIALIZATION:
    /// - Automática para endpoints REST de gestión de usuarios
    /// - Estructura consistente para clientes frontend/mobile
    /// - Optimizada para transferencia eficiente
    /// LIFECYCLE:
    /// - Creado: En respuestas de autenticación y consultas de usuario
    /// - Utilizado: En interfaces de usuario y validaciones de autorización
    /// - Cacheado: Datos seguros para cache de sesión
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Identificador único del usuario en el sistema - PRIMARY KEY DE SEGURIDAD
        /// PROPÓSITO: Identificación única para autorización y trazabilidad empresarial
        /// GENERACIÓN: Auto-increment en base de datos para garantizar unicidad
        /// CASOS DE USO:
        /// - JWT Claims: "sub" claim en tokens de autenticación
        /// - Authorization: Verificación de permisos específicos por usuario
        /// - Audit Trail: Referencia única en logs de auditoría y compliance
        /// - Session Management: Identificación de sesiones activas
        /// - APIs REST: Identificación en endpoints (/api/users/{id})
        /// - Relacionales: Foreign key en tablas que requieren tracking de usuario
        /// - Cache: Key para cache de información de usuario
        /// VALIDACIÓN: > 0 para usuarios existentes, 0 para nuevos usuarios
        /// INMUTABILIDAD: Una vez asignado, nunca debe cambiar
        /// PERFORMANCE: Indexado para consultas rápidas de autorización
        /// SECURITY: Usado en tokens JWT y verificaciones de seguridad
        /// COMPLIANCE: Identificación única para auditorías regulatorias
        /// </summary>
        public int Id { get; set; } // Identificador único del usuario
        
        /// <summary>
        /// Nombre de usuario para login y identificación - CREDENCIAL DE ACCESO ÚNICA
        /// PROPÓSITO: Identificador único para login y visualización en interfaces
        /// VALIDACIÓN CRÍTICA: Único en sistema, formato válido, no sensible a mayúsculas
        /// CASOS DE USO:
        /// - Login: Credential principal para autenticación
        /// - JWT Claims: "name" claim en tokens para identificación
        /// - UI Display: Mostrado en interfaces para identificar usuario activo
        /// - Logs: Identificación legible en logs de aplicación
        /// - APIs: Parámetro de consulta para búsqueda de usuarios
        /// - Admin Panel: Listado y gestión de usuarios por administradores
        /// - Audit Trail: Identificación clara en auditorías de acciones
        /// BUSINESS RULES:
        /// - Único en todo el sistema (no duplicados)
        /// - Inmutable después de creación para consistencia
        /// - Solo caracteres alfanuméricos y algunos especiales permitidos
        /// - Longitud apropiada (3-50 caracteres típicamente)
        /// - Case-insensitive para login pero preserva formato original
        /// FORMATO: Alfanumérico, sin espacios, legible para identificación
        /// SECURITY: No debe revelar información personal sensible
        /// PERFORMANCE: Indexado único para login rápido
        /// COMPLIANCE: Puede ser requerido para trazabilidad regulatoria
        /// </summary>
        public string Username { get; set; } = string.Empty; // Nombre de usuario para login
        
        /// <summary>
        /// Dirección de correo electrónico del usuario - CANAL DE COMUNICACIÓN PRINCIPAL
        /// PROPÓSITO: Comunicación empresarial y alternativa de identificación
        /// VALIDACIÓN CRÍTICA: Formato RFC 5321, único en sistema, verificación activa
        /// CASOS DE USO:
        /// - Comunicaciones: Envío de reportes, alertas y notificaciones empresariales
        /// - Recovery: Recuperación de contraseña y verificación de identidad
        /// - Login alternativo: Permitir login con email como username
        /// - Verification: Proceso de verificación de cuenta de usuario
        /// - Marketing: Comunicaciones de marketing (con consent)
        /// - Compliance: Notificaciones regulatorias y legales
        /// - Multi-factor: Canal para códigos de autenticación MFA
        /// - Admin: Comunicación directa con usuarios para soporte técnico
        /// BUSINESS RULES:
        /// - Único en todo el sistema (un email = un usuario)
        /// - Debe ser válido y verificable (no temporales/desechables)
        /// - Requerido para recuperación de cuenta
        /// - Sujeto a políticas de privacidad y GDPR
        /// - Puede requerir verificación antes de activación completa
        /// FORMATO: RFC 5321 compliant (validación estricta)
        /// SECURITY: Usado en procesos de recuperación de contraseña
        /// PRIVACY: Datos personales sujetos a protección de datos
        /// PERFORMANCE: Indexado para consultas de usuarios por email
        /// COMPLIANCE: Manejo según regulaciones de protección de datos
        /// </summary>
        public string Email { get; set; } = string.Empty; // Correo electrónico del usuario
        
        /// <summary>
        /// Rol del usuario en el sistema - CONTROL DE AUTORIZACIÓN CRÍTICO
        /// PROPÓSITO: Definir nivel de acceso y permisos dentro del sistema empresarial
        /// VALIDACIÓN CRÍTICA: Debe existir en catálogo de roles válidos del sistema
        /// CASOS DE USO:
        /// - Authorization: Control granular de acceso a funcionalidades
        /// - JWT Claims: "role" claim en tokens para verificación de permisos
        /// - UI Control: Mostrar/ocultar elementos según permisos del rol
        /// - API Security: Verificación de permisos antes de operaciones críticas
        /// - Admin Functions: Acceso a funciones administrativas según rol
        /// - Reporting: Filtros de datos según nivel de autorización
        /// - Audit: Tracking de acciones según contexto de rol
        /// - Workflow: Aprobaciones y procesos según jerarquía de roles
        /// ROLES TÍPICOS DEL SISTEMA:
        /// - Admin: Acceso completo al sistema y configuración
        /// - Manager: Gestión de equipos y reportes avanzados
        /// - User: Operaciones estándar de negocio
        /// - Viewer: Solo lectura y consulta de información
        /// - Guest: Acceso muy limitado para demostración
        /// BUSINESS RULES:
        /// - Debe existir en catálogo de roles predefinidos
        /// - Un usuario puede tener solo un rol activo
        /// - Cambios de rol requieren autorización de administrador
        /// - Roles determinan funcionalidades disponibles completamente
        /// - Auditoría obligatoria de todos los cambios de rol
        /// FORMAT: String que coincide exactamente con roles definidos
        /// SECURITY: Base para toda la autorización del sistema
        /// PERFORMANCE: Usado en verificaciones frecuentes de permisos
        /// EXTENSIBILITY: Preparado para sistema de roles más complejo
        /// </summary>
        public string Role { get; set; } = string.Empty; // Rol del usuario (Admin, User, etc)

        // ========== PROPIEDADES FUTURAS PARA GESTIÓN AVANZADA DE USUARIOS ==========
        // TODO: Implementar propiedades adicionales para enterprise user management:

        /// <summary>
        /// Nombre completo del usuario para personalización y comunicación
        /// FUTURO: Display name completo para interfaces de usuario
        /// </summary>
        // public string? FullName { get; set; }

        /// <summary>
        /// Nombre de la organización o tenant al que pertenece el usuario
        /// FUTURO: Support para arquitecturas multi-tenant
        /// </summary>
        // public string? Organization { get; set; }

        /// <summary>
        /// Departamento o área funcional del usuario
        /// FUTURO: Organización interna y permisos por departamento
        /// </summary>
        // public string? Department { get; set; }

        /// <summary>
        /// Estado del usuario (Active, Inactive, Suspended, PendingVerification)
        /// FUTURO: Gestión del lifecycle de usuarios
        /// </summary>
        // public string Status { get; set; } = "Active";

        /// <summary>
        /// Fecha de último acceso para auditoría y gestión de sesiones
        /// FUTURO: Tracking de actividad y políticas de inactividad
        /// </summary>
        // public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// Fecha de creación de la cuenta para auditoría
        /// FUTURO: Tracking completo del lifecycle del usuario
        /// </summary>
        // public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Configuraciones personales del usuario (tema, idioma, timezone)
        /// FUTURO: Personalización avanzada de la experiencia de usuario
        /// </summary>
        // public UserPreferences? Preferences { get; set; }

        /// <summary>
        /// Lista de permisos específicos adicionales al rol base
        /// FUTURO: Granularidad fina de permisos por usuario
        /// </summary>
        // public List<string> AdditionalPermissions { get; set; } = new();

        /// <summary>
        /// Avatar o foto de perfil del usuario
        /// FUTURO: Personalización visual de perfiles
        /// </summary>
        // public string? AvatarUrl { get; set; }

        /// <summary>
        /// Número de teléfono para comunicaciones y MFA
        /// FUTURO: Canal adicional de comunicación y autenticación MFA
        /// </summary>
        // public string? PhoneNumber { get; set; }

        /// <summary>
        /// Timezone del usuario para correcta visualización de fechas
        /// FUTURO: Localización temporal precisa
        /// </summary>
        // public string? TimeZone { get; set; }

        /// <summary>
        /// Idioma preferido del usuario para interfaces localizadas
        /// FUTURO: Soporte multi-idioma completo
        /// </summary>
        // public string? Language { get; set; } = "es";

        /// <summary>
        /// Información de Multi-Factor Authentication del usuario
        /// FUTURO: Seguridad avanzada con MFA
        /// </summary>
        // public MfaInfo? MfaInfo { get; set; }

        // ========== MÉTODOS FUTUROS PARA GESTIÓN DE USUARIOS ==========
        // TODO: Implementar métodos para operaciones avanzadas de usuario:

        /// <summary>
        /// Verifica si el usuario tiene un permiso específico
        /// </summary>
        // public bool TienePermiso(string permiso)
        // {
        //     // Verificar permiso basado en rol y permisos adicionales
        //     // Integrar con sistema de autorización granular
        // }

        /// <summary>
        /// Obtiene todos los permisos efectivos del usuario (rol + adicionales)
        /// </summary>
        // public List<string> ObtenerPermisosEfectivos()
        // {
        //     // Combinar permisos del rol con permisos adicionales específicos
        //     // Resolver conflictos y precedencias
        // }

        /// <summary>
        /// Verifica si el usuario pertenece a una organización específica
        /// </summary>
        // public bool PerteneceAOrganizacion(string organizacion)
        // {
        //     // Verificación para arquitecturas multi-tenant
        // }

        /// <summary>
        /// Sanitiza la información del usuario removiendo datos sensibles para logging
        /// </summary>
        // public UserDto SanitizarParaLogging()
        // {
        //     // Retornar copia con email parcialmente ofuscado
        //     // Preservar datos necesarios para auditoría sin comprometer privacidad
        // }

        /// <summary>
        /// Valida la integridad y consistencia de los datos del usuario
        /// </summary>
        // public bool EsValido()
        // {
        //     // Validar formato de email, existencia de rol, consistencia de datos
        // }

        // ========== CONSIDERACIONES PARA GESTIÓN EMPRESARIAL DE USUARIOS ==========
        // FUTURE ENHANCEMENTS para enterprise user management:
        // - Single Sign-On: Integración con proveedores de identidad empresariales
        // - Active Directory: Sincronización con AD/LDAP corporativo
        // - RBAC Avanzado: Sistema de roles y permisos granular y jerárquico
        // - Provisioning: Creación automática de usuarios desde sistemas HR
        // - Lifecycle Management: Automatización completa del ciclo de vida
        // - Compliance: Cumplimiento automático con regulaciones de privacidad
        // - Analytics: Dashboards de uso y comportamiento de usuarios
        // - Security: Detección de anomalías y patrones de riesgo
        // - Integration: APIs para integración con sistemas externos
        // - Audit: Logging exhaustivo para compliance y forensics

        // ========== PATRONES DE USO RECOMENDADOS ==========
        // FRONTEND (React/Angular/Vue):
        // 1. Recibir UserDto en respuesta de autenticación
        // 2. Usar Role para control de acceso en componentes
        // 3. Mostrar Username/Email en header de aplicación
        // 4. Cache local durante sesión activa
        // 5. Limpiar cache al logout
        // 
        // BACKEND APIs:
        // 1. Incluir en respuestas de autenticación
        // 2. Usar para identificación en logs de aplicación
        // 3. Verificar Role antes de operaciones críticas
        // 4. Nunca incluir en respuestas información sensible
        // 
        // MOBILE (iOS/Android):
        // 1. Almacenar datos básicos para personalización
        // 2. Usar Role para funcionalidades disponibles
        // 3. Sincronizar con actualizaciones de perfil
        // 4. Limpiar datos al logout
    }
} 