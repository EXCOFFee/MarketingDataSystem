// ==================== DATA TRANSFER OBJECT CRÍTICO - RESPUESTA DE AUTENTICACIÓN JWT ====================
// Este DTO contiene datos de seguridad críticos para acceso al sistema empresarial
// CAPA: Application/Core - Contrato de autenticación segura
// CRITICIDAD: MÁXIMA - Controla acceso completo al sistema y datos empresariales
// RESPONSABILIDAD: Transferir credenciales JWT de forma segura después de login exitoso
// CLEAN ARCHITECTURE: DTO puro de seguridad sin dependencias externas
// SEGURIDAD: Contiene token JWT sensible que debe protegerse estrictamente

using System;

// DTO que representa la respuesta de autenticación (login)
namespace MarketingDataSystem.Core.DTOs
{
    /// <summary>
    /// Data Transfer Object para respuesta de autenticación JWT - SEGURIDAD CRÍTICA DEL SISTEMA
    /// RESPONSABILIDAD: Transferir credenciales y datos de usuario después de login exitoso
    /// ARQUITECTURA: Application Layer DTO en Clean Architecture
    /// CRITICIDAD: MÁXIMA - Contiene token de acceso que autoriza todas las operaciones
    /// CASOS DE USO EMPRESARIALES:
    /// - Autenticación API: Login exitoso devuelve este DTO con token válido
    /// - Single Sign-On: Token compartido entre aplicaciones del ecosistema
    /// - Mobile Apps: Credenciales para aplicaciones móviles empresariales
    /// - Integración B2B: Tokens para APIs de partners y sistemas externos
    /// - Dashboards Ejecutivos: Acceso seguro a reportes sensibles
    /// - Administración: Tokens con permisos elevados para configuración
    /// SEGURIDAD IMPLEMENTADA:
    /// - JWT Token: Firmado digitalmente para verificar autenticidad
    /// - Expiración: Tokens con TTL limitado para minimizar riesgo
    /// - Claims: Información de usuario y permisos embedida en token
    /// - HTTPS Only: Transmisión exclusivamente sobre conexiones seguras
    /// - No Persistencia: No almacenar tokens en logs o almacenamiento inseguro
    /// COMPLIANCE Y AUDITORÍA:
    /// - Tracking completo de emisión y uso de tokens
    /// - Rotación automática según políticas de seguridad
    /// - Revocación inmediata en caso de compromiso
    /// - Logs de auditoría para compliance SOX/regulatorio
    /// JSON SERIALIZATION:
    /// - Automática para endpoints REST de autenticación
    /// - Estructura consistente para clientes frontend/mobile
    /// LIFECYCLE:
    /// - Generado: Después de validación exitosa de credenciales
    /// - Transmitido: Una sola vez en respuesta de login
    /// - Utilizado: En header Authorization de requests subsecuentes
    /// - Expirado: Automáticamente después de TTL configurado
    /// </summary>
    public class AuthResponseDto
    {
        /// <summary>
        /// Token JWT firmado digitalmente - CREDENCIAL DE ACCESO CRÍTICA
        /// PROPÓSITO: Credencial portátil que autoriza acceso a todas las APIs del sistema
        /// CONTENIDO: Header.Payload.Signature con información de usuario y permisos
        /// CASOS DE USO:
        /// - Authorization Header: "Bearer {token}" en todas las requests HTTP
        /// - Claims Verification: Extracción de usuario, roles, permisos del payload
        /// - Signature Validation: Verificación de integridad y autenticidad
        /// - Expiration Check: Validación automática de vigencia temporal
        /// - API Gateway: Autorización distribuida en microservicios
        /// - Audit Trail: Identificación de usuario en logs de operaciones
        /// ESTRUCTURA JWT TÍPICA:
        /// - Header: {"alg":"HS256","typ":"JWT"}
        /// - Payload: {"sub":"userId","name":"userName","role":"userRole","exp":timestamp}
        /// - Signature: HMACSHA256(base64UrlEncode(header)+"."+base64UrlEncode(payload),secret)
        /// SEGURIDAD CRÍTICA:
        /// - Secret Key: Clave secreta compartida para firma/verificación
        /// - Expiration: TTL configurado (típicamente 1-8 horas)
        /// - Issuer: Identificación del servidor que emitió el token
        /// - Audience: Aplicación/servicio para el cual es válido el token
        /// CONFIGURACIÓN:
        /// - Algoritmo: HS256 (HMAC SHA256) o RS256 (RSA SHA256)
        /// - TTL: Configurable según políticas de seguridad (1-8 horas)
        /// - Refresh: Posibilidad de renovación sin re-autenticación
        /// VALIDACIONES AUTOMÁTICAS:
        /// - Signature: Verificación de integridad en cada request
        /// - Expiration: Rechazo automático de tokens vencidos
        /// - Format: Validación de estructura JWT estándar
        /// - Claims: Verificación de claims requeridos (sub, exp, iat)
        /// CONSIDERACIONES DE SEGURIDAD:
        /// - Storage: Solo en memoria/sessionStorage, nunca localStorage
        /// - Transmission: Exclusivamente HTTPS, nunca HTTP plano
        /// - Logging: NUNCA incluir token completo en logs de aplicación
        /// - Revocation: Lista de tokens revocados para invalidación inmediata
        /// </summary>
        public string Token { get; set; } = string.Empty;
        
        /// <summary>
        /// Información del usuario autenticado - DATOS DE CONTEXTO SEGURO
        /// PROPÓSITO: Proporcionar datos básicos del usuario para personalización frontend
        /// CONTENIDO: UserDto con información no sensible del usuario autenticado
        /// CASOS DE USO:
        /// - UI Personalization: Mostrar nombre, email en interfaz de usuario
        /// - Role-based UI: Mostrar/ocultar elementos según permisos del usuario
        /// - Avatar/Profile: Información para componentes de perfil de usuario
        /// - Dashboard Setup: Configuración inicial de dashboard por rol
        /// - Audit Context: Información adicional para logging de operaciones
        /// - Multi-tenant: Información de tenant/organización del usuario
        /// PROPIEDADES INCLUIDAS:
        /// - Id: Identificador único del usuario para referencias
        /// - Username: Nombre de usuario para visualización
        /// - Email: Correo electrónico para comunicaciones
        /// - Role: Rol del usuario para autorización frontend
        /// PROPIEDADES EXCLUIDAS (SEGURIDAD):
        /// - PasswordHash: NUNCA incluir hash de contraseña
        /// - Tokens históricos: Sin información de sesiones previas
        /// - Datos sensibles: Sin información PII innecesaria
        /// SINCRONIZACIÓN:
        /// - Consistencia: Debe coincidir con claims del JWT token
        /// - Actualización: Se actualiza junto con renovación de token
        /// - Cache Frontend: Puede cachearse localmente para UI
        /// VALIDACIÓN:
        /// - Coherencia: Id y Username deben coincidir con JWT claims
        /// - Autorización: Role debe estar autorizado para operaciones solicitadas
        /// - Sesión: Información válida durante vigencia del token
        /// PERFORMANCE:
        /// - Lightweight: Solo propiedades esenciales para UI/UX
        /// - Serializable: Optimizado para JSON serialization
        /// - Cacheable: Apto para cache de sesión frontend
        /// </summary>
        public UserDto User { get; set; } = new();

        // ========== PROPIEDADES FUTURAS PARA SEGURIDAD AVANZADA ==========
        // TODO: Implementar propiedades adicionales para enterprise security:

        /// <summary>
        /// Token de renovación para obtener nuevos tokens sin re-autenticación
        /// FUTURO: Refresh token de larga duración para UX mejorada
        /// </summary>
        // public string? RefreshToken { get; set; }

        /// <summary>
        /// Timestamp de expiración del token principal
        /// FUTURO: Información explícita de expiración para manejo frontend
        /// </summary>
        // public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Tipo de token emitido (Bearer, Basic, etc.)
        /// FUTURO: Support para múltiples tipos de autenticación
        /// </summary>
        // public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Alcance/permisos específicos del token
        /// FUTURO: Granularidad fina de permisos por token
        /// </summary>
        // public string[] Scopes { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Información de la sesión de autenticación
        /// FUTURO: Metadatos de sesión para auditoría y control
        /// </summary>
        // public SessionInfo? Session { get; set; }

        /// <summary>
        /// Configuraciones de seguridad específicas del usuario
        /// FUTURO: Políticas de seguridad personalizadas
        /// </summary>
        // public SecurityConfig? SecurityConfig { get; set; }

        /// <summary>
        /// Multi-factor authentication status
        /// FUTURO: Estado MFA y requerimientos adicionales
        /// </summary>
        // public MfaStatus? MfaStatus { get; set; }

        /// <summary>
        /// Información de tenant/organización para multi-tenancy
        /// FUTURO: Support para arquitecturas multi-tenant
        /// </summary>
        // public TenantInfo? Tenant { get; set; }

        // ========== MÉTODOS FUTUROS PARA VALIDACIÓN DE SEGURIDAD ==========
        // TODO: Implementar métodos para validación y manipulación segura:

        /// <summary>
        /// Valida la integridad y consistencia de la respuesta de autenticación
        /// </summary>
        // public bool EsValida()
        // {
        //     // Validar que token no esté vacío y tenga formato JWT válido
        //     // Verificar que User no sea null y tenga datos mínimos
        //     // Confirmar coherencia entre token claims y User data
        // }

        /// <summary>
        /// Obtiene claims del token JWT sin validar signature
        /// NOTA: Solo para display, siempre validar signature en operaciones críticas
        /// </summary>
        // public Dictionary<string, object> ObtenerClaims()
        // {
        //     // Parse del JWT payload para extraer claims
        //     // Útil para debugging y display de información
        // }

        /// <summary>
        /// Verifica si el token está cerca de expirar
        /// </summary>
        // public bool RequiereRenovacion(TimeSpan ventanaRenovacion)
        // {
        //     // Determinar si token necesita renovación proactiva
        //     // Para UX fluida sin interrupciones por expiración
        // }

        /// <summary>
        /// Sanitiza la respuesta removiendo información sensible para logging
        /// </summary>
        // public AuthResponseDto SanitizarParaLogging()
        // {
        //     // Retornar copia con token parcialmente ofuscado
        //     // Preservar datos necesarios para auditoría sin comprometer seguridad
        // }

        // ========== CONSIDERACIONES PARA SEGURIDAD EMPRESARIAL ==========
        // FUTURE ENHANCEMENTS para enterprise-grade security:
        // - Token Binding: Vincular tokens a características específicas del dispositivo
        // - Geographic Restrictions: Validar ubicación geográfica del usuario
        // - Device Fingerprinting: Tracking de dispositivos autorizados
        // - Behavioral Analytics: Detección de patrones anómalos de uso
        // - Zero Trust: Verificación continua en lugar de autenticación única
        // - Quantum-Safe: Preparación para algoritmos post-quantum
        // - FIDO2/WebAuthn: Support para autenticación sin contraseñas
        // - Risk-based Auth: Autenticación adaptativa basada en riesgo
        // - Session Management: Control avanzado de sesiones concurrentes
        // - Audit Logging: Logging detallado para compliance y forensics

        // ========== PATRONES DE USO RECOMENDADOS ==========
        // FRONTEND (React/Angular/Vue):
        // 1. Recibir AuthResponseDto del endpoint /auth/login
        // 2. Almacenar token en memoria/sessionStorage (NUNCA localStorage)
        // 3. Usar User data para personalización inmediata de UI
        // 4. Incluir token en header Authorization de todas las requests
        // 5. Manejar expiración automática y redirección a login
        // 
        // MOBILE (iOS/Android):
        // 1. Almacenar token en Keychain (iOS) o Keystore (Android)
        // 2. Implementar renovación automática de tokens
        // 3. Manejar estados de red y sincronización offline
        // 
        // APIs/BACKENDS:
        // 1. Validar signature en cada request que use el token
        // 2. Verificar claims de autorización antes de operaciones
        // 3. Registrar uso del token para auditoría
        // 4. Implementar revocación inmediata si es necesario
    }
} 