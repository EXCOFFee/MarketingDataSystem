// ==================== INTERFAZ CRÍTICA - SERVICIO DE AUTENTICACIÓN Y SEGURIDAD ====================
// Esta interfaz define el contrato para operaciones críticas de seguridad del sistema
// CAPA: Application Layer - Contratos de servicios de seguridad
// CRITICIDAD: MÁXIMA - Controla acceso completo al sistema empresarial
// RESPONSABILIDAD: Definir operaciones de autenticación, autorización y gestión de usuarios
// CLEAN ARCHITECTURE: Abstracción de seguridad que permite inversión de dependencias
// PATRÓN: Service Layer Pattern + Security Gateway Pattern

using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Interfaz para servicios de autenticación y gestión de usuarios - SEGURIDAD CRÍTICA DEL SISTEMA
    /// RESPONSABILIDAD: Definir contrato para operaciones de seguridad y autenticación
    /// ARQUITECTURA: Application Layer Security Service Interface en Clean Architecture
    /// CRITICIDAD: MÁXIMA - Controla acceso completo al sistema y datos empresariales
    /// PRINCIPIOS DE SEGURIDAD:
    /// - Zero Trust: Nunca confiar, siempre verificar
    /// - Least Privilege: Permisos mínimos necesarios
    /// - Defense in Depth: Múltiples capas de seguridad
    /// - Fail Secure: Fallar de forma segura cuando hay errores
    /// CASOS DE USO EMPRESARIALES:
    /// - Autenticación Empresarial: Login seguro de usuarios del sistema
    /// - Gestión de Usuarios: Registro y administración de cuentas
    /// - Recuperación de Acceso: Procesos seguros de reset de contraseñas
    /// - Compliance: Cumplimiento de regulaciones de seguridad
    /// - Auditoría: Trazabilidad completa de accesos y cambios
    /// - Single Sign-On: Integración con sistemas de identidad empresariales
    /// - Multi-Factor Authentication: Autenticación de múltiples factores
    /// IMPLEMENTACIÓN:
    /// - AuthService: Implementación concreta con lógica de seguridad
    /// - Mock/Stub: Para testing de seguridad sin dependencias externas
    /// - Proxy: Para logging de seguridad y decoradores de autorización
    /// INYECCIÓN DE DEPENDENCIAS:
    /// - Registrada en DI container para inversión de dependencias
    /// - Permite testabilidad y flexibilidad en políticas de seguridad
    /// - Facilita intercambio de proveedores de autenticación
    /// JWT SECURITY:
    /// - Tokens firmados digitalmente para verificar integridad
    /// - Claims embebidos para autorización granular
    /// - Expiración automática para minimizar riesgo de compromiso
    /// - Refresh tokens para UX fluida sin comprometer seguridad
    /// COMPLIANCE Y AUDITORÍA:
    /// - Logging exhaustivo de todos los eventos de seguridad
    /// - Trazabilidad completa para auditorías regulatorias
    /// - Cumplimiento con GDPR, SOX, y otras regulaciones
    /// - Detección automática de patrones de ataque
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Autentica un usuario y genera token JWT - OPERACIÓN DE SEGURIDAD CRÍTICA
        /// PROPÓSITO: Verificar credenciales y generar token de acceso seguro
        /// CASOS DE USO EMPRESARIALES:
        /// - Login Web: Autenticación en aplicaciones web empresariales
        /// - API Access: Generación de tokens para acceso a APIs REST
        /// - Mobile Apps: Autenticación en aplicaciones móviles corporativas
        /// - Integración B2B: Tokens para sistemas de partners empresariales
        /// - Service-to-Service: Autenticación entre microservicios
        /// - Admin Panel: Acceso seguro a paneles de administración
        /// - Dashboards: Autenticación para dashboards ejecutivos
        /// VALIDACIONES CRÍTICAS DE SEGURIDAD:
        /// - Credenciales Válidas: Verificar username/email y password
        /// - Cuenta Activa: Confirmar que la cuenta no está suspendida
        /// - Rate Limiting: Prevenir ataques de fuerza bruta
        /// - Geo-blocking: Verificar ubicación geográfica si es necesario
        /// - Device Fingerprinting: Detectar dispositivos sospechosos
        /// - Password Policy: Verificar cumplimiento de políticas de contraseñas
        /// - Account Lockout: Bloquear cuentas después de intentos fallidos
        /// OPERACIONES ATÓMICAS DE SEGURIDAD:
        /// - Verificación de Contraseña: Hash comparison con timing attack protection
        /// - Generación de JWT: Token firmado con claims de usuario y permisos
        /// - Logging de Acceso: Registro detallado del evento de login
        /// - Actualización de Last Login: Timestamp de último acceso
        /// - Invalidación de Tokens Previos: Opcional para mayor seguridad
        /// - Notificación de Login: Alertas de acceso para detección de anomalías
        /// JWT TOKEN STRUCTURE:
        /// - Header: Algoritmo de firma y tipo de token
        /// - Payload: Claims de usuario (sub, name, role, exp, iat, iss)
        /// - Signature: Firma digital para verificar integridad
        /// SEGURIDAD AVANZADA:
        /// - HTTPS Only: Transmisión exclusivamente sobre conexiones seguras
        /// - Secure Cookies: HttpOnly, Secure, SameSite flags
        /// - CSRF Protection: Tokens CSRF para prevenir ataques
        /// - XSS Protection: Sanitización de entrada y salida
        /// MANEJO DE ERRORES SEGUROS:
        /// - Generic Error Messages: No revelar información específica de errores
        /// - Timing Attack Protection: Respuestas con tiempo constante
        /// - Logging Detallado: Registrar intentos fallidos para análisis
        /// </summary>
        /// <param name="loginDto">Credenciales de login con username/email y password</param>
        /// <returns>AuthResponseDto con token JWT y datos básicos del usuario</returns>
        /// <exception cref="InvalidCredentialsException">Cuando las credenciales son inválidas</exception>
        /// <exception cref="AccountLockedException">Cuando la cuenta está bloqueada</exception>
        /// <exception cref="AccountInactiveException">Cuando la cuenta está inactiva</exception>
        /// <exception cref="SecurityException">Cuando se detecta actividad sospechosa</exception>
        /// <exception cref="RateLimitExceededException">Cuando se excede el límite de intentos</exception>
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        
        /// <summary>
        /// Registra un nuevo usuario en el sistema - OPERACIÓN DE CREACIÓN DE IDENTIDAD CRÍTICA
        /// PROPÓSITO: Crear nueva cuenta de usuario con validaciones de seguridad estrictas
        /// CASOS DE USO EMPRESARIALES:
        /// - Self-Registration: Registro autónomo de usuarios autorizados
        /// - Admin Creation: Creación de cuentas por administradores
        /// - Bulk Import: Importación masiva de usuarios desde HR systems
        /// - Partner Onboarding: Registro de usuarios de organizaciones partner
        /// - Contractor Access: Cuentas temporales para contratistas
        /// - Service Accounts: Cuentas técnicas para integraciones
        /// - Testing: Creación de cuentas para testing y desarrollo
        /// VALIDACIONES CRÍTICAS DE SEGURIDAD:
        /// - Email Único: Verificar que no exista otro usuario con el mismo email
        /// - Username Único: Confirmar unicidad del nombre de usuario
        /// - Password Strength: Verificar cumplimiento de políticas de contraseñas
        /// - Email Verification: Validar formato y deliverability del email
        /// - Domain Whitelist: Verificar que el dominio del email esté autorizado
        /// - Role Validation: Confirmar que el rol asignado es válido y autorizado
        /// - Registration Limits: Verificar límites de registro por IP/dominio
        /// OPERACIONES ATÓMICAS DE SEGURIDAD:
        /// - Password Hashing: Generar hash seguro con salt único
        /// - Account Creation: Crear registro de usuario en base de datos
        /// - Email Verification: Enviar email de verificación si es requerido
        /// - Role Assignment: Asignar rol por defecto o especificado
        /// - Audit Logging: Registrar evento de creación de cuenta
        /// - Welcome Process: Iniciar proceso de onboarding si es necesario
        /// PASSWORD SECURITY:
        /// - Hashing Algorithm: bcrypt, scrypt, o Argon2 para hash seguro
        /// - Salt Generation: Salt único por usuario para prevenir rainbow tables
        /// - Complexity Requirements: Políticas de complejidad configurables
        /// - History Tracking: Prevenir reutilización de contraseñas previas
        /// VERIFICACIÓN DE IDENTIDAD:
        /// - Email Verification: Proceso de verificación por email
        /// - Phone Verification: Opcional para verificación adicional
        /// - Manual Approval: Proceso de aprobación manual si es requerido
        /// - Document Verification: Verificación de documentos para casos especiales
        /// COMPLIANCE:
        /// - GDPR Consent: Obtener consentimiento para procesamiento de datos
        /// - Data Minimization: Recopilar solo datos necesarios
        /// - Privacy Policy: Aceptación de políticas de privacidad
        /// - Terms of Service: Aceptación de términos de uso
        /// </summary>
        /// <param name="createUserDto">Datos del nuevo usuario a registrar</param>
        /// <returns>UserDto con información básica del usuario creado</returns>
        /// <exception cref="UserAlreadyExistsException">Cuando el usuario ya existe</exception>
        /// <exception cref="InvalidEmailException">Cuando el email no es válido</exception>
        /// <exception cref="WeakPasswordException">Cuando la contraseña no cumple políticas</exception>
        /// <exception cref="UnauthorizedDomainException">Cuando el dominio no está autorizado</exception>
        /// <exception cref="RegistrationLimitException">Cuando se exceden límites de registro</exception>
        /// <exception cref="ValidationException">Cuando los datos no son válidos</exception>
        Task<UserDto> RegisterAsync(CreateUserDto createUserDto);
        
        /// <summary>
        /// Cambia la contraseña de un usuario existente - OPERACIÓN DE SEGURIDAD CRÍTICA
        /// PROPÓSITO: Permitir cambio seguro de contraseñas con validaciones estrictas
        /// CASOS DE USO EMPRESARIALES:
        /// - Password Rotation: Cambio periódico de contraseñas por políticas
        /// - Compromised Account: Cambio de contraseña después de detección de compromiso
        /// - User-Initiated: Cambio voluntario de contraseña por el usuario
        /// - Admin Reset: Cambio forzado por administradores
        /// - Compliance: Cambio requerido por regulaciones de seguridad
        /// - Onboarding: Cambio de contraseña temporal en primer login
        /// - Recovery Process: Cambio después de proceso de recuperación
        /// VALIDACIONES CRÍTICAS DE SEGURIDAD:
        /// - Current Password: Verificar contraseña actual antes del cambio
        /// - User Authorization: Confirmar que el usuario puede cambiar su contraseña
        /// - Password Strength: Verificar que la nueva contraseña cumple políticas
        /// - Password History: Prevenir reutilización de contraseñas previas
        /// - Rate Limiting: Prevenir abuso de cambios de contraseña
        /// - Session Validation: Verificar sesión válida del usuario
        /// - Multi-Factor: Requerir MFA para cambios de contraseña si está habilitado
        /// OPERACIONES ATÓMICAS DE SEGURIDAD:
        /// - Password Verification: Verificar contraseña actual
        /// - New Password Hashing: Generar hash seguro de nueva contraseña
        /// - Password Update: Actualizar hash en base de datos
        /// - Session Invalidation: Invalidar todas las sesiones existentes
        /// - Audit Logging: Registrar evento de cambio de contraseña
        /// - Notification: Notificar al usuario sobre el cambio
        /// SECURITY MEASURES:
        /// - Atomic Operation: Operación completamente atómica
        /// - Rollback Protection: Prevenir rollback parcial en caso de error
        /// - Timing Attack Protection: Respuestas con tiempo constante
        /// - Generic Error Messages: No revelar información específica
        /// PASSWORD POLICY ENFORCEMENT:
        /// - Minimum Length: Longitud mínima configurable
        /// - Character Complexity: Requerimientos de caracteres especiales
        /// - Dictionary Check: Verificación contra diccionarios de contraseñas comunes
        /// - Personal Info Check: Verificar que no contenga información personal
        /// - Expiration Tracking: Tracking de expiración de contraseñas
        /// COMPLIANCE:
        /// - Audit Trail: Registro detallado para auditorías
        /// - Data Protection: Protección de datos sensibles durante proceso
        /// - Regulatory Compliance: Cumplimiento con regulaciones aplicables
        /// </summary>
        /// <param name="userId">ID del usuario que cambia la contraseña</param>
        /// <param name="currentPassword">Contraseña actual para verificación</param>
        /// <param name="newPassword">Nueva contraseña que cumple políticas de seguridad</param>
        /// <returns>true si el cambio fue exitoso, false en caso contrario</returns>
        /// <exception cref="UserNotFoundException">Cuando el usuario no existe</exception>
        /// <exception cref="InvalidPasswordException">Cuando la contraseña actual es incorrecta</exception>
        /// <exception cref="WeakPasswordException">Cuando la nueva contraseña no cumple políticas</exception>
        /// <exception cref="PasswordHistoryException">Cuando la contraseña ya fue usada</exception>
        /// <exception cref="UnauthorizedException">Cuando el usuario no está autorizado</exception>
        /// <exception cref="RateLimitExceededException">Cuando se excede límite de cambios</exception>
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        
        /// <summary>
        /// Inicia proceso de recuperación de contraseña - OPERACIÓN DE RECUPERACIÓN SEGURA
        /// PROPÓSITO: Permitir recuperación segura de acceso a cuentas comprometidas
        /// CASOS DE USO EMPRESARIALES:
        /// - Forgotten Password: Usuario olvidó su contraseña
        /// - Account Lockout: Recuperación después de bloqueo de cuenta
        /// - Compromised Account: Recuperación después de detección de compromiso
        /// - Employee Departure: Recuperación de cuentas de empleados que dejaron la empresa
        /// - Emergency Access: Acceso de emergencia a cuentas críticas
        /// - Bulk Recovery: Proceso masivo de recuperación después de incidentes
        /// - Testing: Recuperación de cuentas de testing
        /// VALIDACIONES CRÍTICAS DE SEGURIDAD:
        /// - Email Verification: Verificar que el email existe en el sistema
        /// - Account Status: Confirmar que la cuenta puede ser recuperada
        /// - Rate Limiting: Prevenir abuso del proceso de recuperación
        /// - Geo-blocking: Verificar ubicación geográfica si es necesario
        /// - Suspicious Activity: Detectar patrones sospechosos de recuperación
        /// - Email Deliverability: Verificar que el email puede recibir mensajes
        /// OPERACIONES ATÓMICAS DE SEGURIDAD:
        /// - Token Generation: Generar token seguro de recuperación
        /// - Token Storage: Almacenar token con expiración en base de datos
        /// - Email Dispatch: Enviar email con instrucciones de recuperación
        /// - Audit Logging: Registrar evento de inicio de recuperación
        /// - Rate Limit Tracking: Actualizar contadores de rate limiting
        /// - Security Monitoring: Monitorear para detectar abuso
        /// RECOVERY TOKEN SECURITY:
        /// - Cryptographic Randomness: Tokens generados con alta entropía
        /// - Time-Limited: Expiración automática después de tiempo configurado
        /// - Single-Use: Tokens válidos solo para un uso
        /// - Secure Transmission: Envío solo por canales seguros
        /// - Hashed Storage: Almacenamiento de hash del token, no texto plano
        /// EMAIL SECURITY:
        /// - Secure Templates: Plantillas de email seguras sin información sensible
        /// - HTTPS Links: Enlaces exclusivamente HTTPS para proceso de recuperación
        /// - Anti-Phishing: Medidas para prevenir phishing de recovery emails
        /// - Delivery Tracking: Tracking de entrega de emails críticos
        /// COMPLIANCE:
        /// - Privacy Protection: Protección de datos personales en proceso
        /// - Audit Logging: Registro detallado para compliance
        /// - Data Retention: Políticas de retención de datos de recuperación
        /// - Regulatory Compliance: Cumplimiento con regulaciones aplicables
        /// </summary>
        /// <param name="email">Email del usuario para envío de instrucciones de recuperación</param>
        /// <returns>true si el proceso se inició exitosamente, false en caso contrario</returns>
        /// <exception cref="UserNotFoundException">Cuando no existe usuario con ese email</exception>
        /// <exception cref="AccountInactiveException">Cuando la cuenta no puede ser recuperada</exception>
        /// <exception cref="RateLimitExceededException">Cuando se excede límite de recuperaciones</exception>
        /// <exception cref="EmailDeliveryException">Cuando no se puede entregar el email</exception>
        /// <exception cref="SecurityException">Cuando se detecta actividad sospechosa</exception>
        Task<bool> ResetPasswordAsync(string email);

        // ========== MÉTODOS FUTUROS PARA SEGURIDAD AVANZADA ==========
        // TODO: Implementar métodos adicionales para enterprise security:

        /// <summary>
        /// Valida y renueva un token JWT existente
        /// FUTURO: Renovación automática de tokens para UX fluida
        /// </summary>
        // Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Revoca un token JWT específico para invalidación inmediata
        /// FUTURO: Revocación granular de tokens comprometidos
        /// </summary>
        // Task<bool> RevokeTokenAsync(string token);

        /// <summary>
        /// Valida un token JWT y extrae claims sin renovar
        /// FUTURO: Validación de tokens para verificación de autorización
        /// </summary>
        // Task<ClaimsPrincipal> ValidateTokenAsync(string token);

        /// <summary>
        /// Obtiene todas las sesiones activas de un usuario
        /// FUTURO: Gestión de sesiones múltiples por usuario
        /// </summary>
        // Task<IEnumerable<UserSessionDto>> GetActiveSessionsAsync(int userId);

        /// <summary>
        /// Termina todas las sesiones de un usuario específico
        /// FUTURO: Logout masivo para casos de compromiso
        /// </summary>
        // Task<bool> LogoutAllSessionsAsync(int userId);

        /// <summary>
        /// Verifica si un usuario tiene un permiso específico
        /// FUTURO: Verificación granular de permisos
        /// </summary>
        // Task<bool> HasPermissionAsync(int userId, string permission);

        /// <summary>
        /// Inicia proceso de verificación en dos pasos (MFA)
        /// FUTURO: Multi-Factor Authentication avanzado
        /// </summary>
        // Task<MfaChallengeDto> InitiateMfaAsync(int userId);

        /// <summary>
        /// Verifica código de autenticación en dos pasos
        /// FUTURO: Verificación de códigos MFA
        /// </summary>
        // Task<bool> VerifyMfaAsync(int userId, string code);

        /// <summary>
        /// Obtiene audit trail de actividad de seguridad de un usuario
        /// FUTURO: Trazabilidad completa de eventos de seguridad
        /// </summary>
        // Task<IEnumerable<SecurityAuditDto>> GetSecurityAuditAsync(int userId);

        /// <summary>
        /// Detecta y reporta actividad sospechosa en cuentas de usuario
        /// FUTURO: Detección automática de anomalías de seguridad
        /// </summary>
        // Task<SecurityThreatDto> DetectSuspiciousActivityAsync(int userId);

        // ========== CONSIDERACIONES PARA SEGURIDAD EMPRESARIAL ==========
        // FUTURE ENHANCEMENTS para enterprise-grade security:
        // - OAuth 2.0/OpenID Connect: Integración con proveedores externos
        // - SAML 2.0: Support para Single Sign-On empresarial
        // - Active Directory: Integración con sistemas de directorio corporativo
        // - Certificate-based Auth: Autenticación basada en certificados
        // - Biometric Authentication: Support para autenticación biométrica
        // - Risk-based Authentication: Autenticación adaptativa por riesgo
        // - Behavioral Analytics: Análisis de patrones de comportamiento
        // - Threat Intelligence: Integración con feeds de inteligencia de amenazas
        // - Zero Trust Architecture: Implementación de arquitectura zero trust
        // - Quantum-Safe Cryptography: Preparación para algoritmos post-quantum
    }
} 