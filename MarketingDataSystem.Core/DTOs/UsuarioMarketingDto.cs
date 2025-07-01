// ==================== DATA TRANSFER OBJECT - USUARIO MARKETING ====================
// Este DTO define el contrato de comunicación para usuarios del módulo de marketing
// PROPÓSITO: Transferencia segura de datos de usuarios especializados en marketing
// DIFERENCIA CON ENTITY: Sin relaciones navegacionales, sin anotaciones EF
// ARQUITECTURA: Capa Core - DTOs compartidos entre todas las capas
// RESPONSABILIDAD: Gestión de usuarios con roles específicos de marketing
// AUTORIZACIÓN: Integrado con sistema de roles empresariales
// AUDITORÍA: Incluido en logs de acceso y modificaciones de usuarios

using System;

namespace MarketingDataSystem.Core.DTOs
{
    /// <summary>
    /// Data Transfer Object para usuarios del módulo de marketing empresarial
    /// RESPONSABILIDAD: Definir estructura de datos para usuarios especializados
    /// SEGURIDAD: Expone solo información necesaria para operaciones de marketing
    /// AUTORIZACIÓN: Soporte para roles específicos (Marketing, Admin, Supervisor)
    /// SERIALIZACIÓN: Se convierte automáticamente a/desde JSON en APIs REST
    /// VALIDACIÓN: FluentValidation aplica reglas de negocio específicas
    /// MAPPING: AutoMapper convierte entre UsuarioMarketingDto ↔ UsuarioMarketing Entity
    /// COMPATIBILIDAD: Propiedades en inglés y español para legacy support
    /// CASOS DE USO:
    /// - Gestión de usuarios del equipo de marketing
    /// - Autenticación y autorización de personal marketing
    /// - Reportes de actividad de usuarios especializados
    /// - Integración con sistemas CRM/ERP empresariales
    /// </summary>
    public class UsuarioMarketingDto
    {
        // ========== IDENTIFICADOR ÚNICO EMPRESARIAL ==========
        /// <summary>
        /// Identificador único del usuario en el sistema empresarial
        /// AUTO-GENERADO: Asignado por base de datos en creación
        /// USO: URLs REST (GET /api/usuario-marketing/123), referencias en auditoría
        /// INMUTABLE: No debe modificarse una vez asignado
        /// TRAZABILIDAD: Usado en todos los logs de actividad del usuario
        /// </summary>
        public int Id { get; set; } // Identificador único del usuario

        // ========== INFORMACIÓN PERSONAL Y PROFESIONAL ==========
        /// <summary>
        /// Nombre completo del usuario de marketing
        /// VALIDACIÓN: FluentValidation verifica longitud, caracteres permitidos
        /// REQUERIDO: Campo obligatorio según reglas de negocio empresarial
        /// DISPLAY: Mostrado en interfaces de usuario y reportes ejecutivos
        /// AUDITORÍA: Incluido en logs de acceso para trazabilidad de responsabilidades
        /// </summary>
        public string Nombre { get; set; } = string.Empty; // Nombre completo del usuario

        /// <summary>
        /// Dirección de correo electrónico corporativo del usuario
        /// VALIDACIÓN: FluentValidation verifica formato RFC 5321 y dominio empresarial
        /// ÚNICO: No puede haber dos usuarios con el mismo email corporativo
        /// COMUNICACIÓN: Canal principal para notificaciones críticas de marketing
        /// AUTENTICACIÓN: Usado como identificador principal en sistema JWT
        /// COMPLIANCE: Debe cumplir con políticas de email corporativo
        /// </summary>
        public string Email { get; set; } = string.Empty; // Correo electrónico del usuario

        // ========== AUTORIZACIÓN Y ROLES EMPRESARIALES ==========
        /// <summary>
        /// Rol empresarial del usuario en el sistema
        /// ROLES MARKETING: Marketing, MarketingManager, MarketingAnalyst
        /// ROLES ADMINISTRATIVOS: Admin, SuperAdmin, SystemAdmin
        /// ROLES GENERALES: User, Viewer, Guest
        /// JERÁRQUÍA: Define nivel de acceso a funcionalidades críticas
        /// AUDITORÍA: Cambios de rol registrados en audit trail empresarial
        /// AUTORIZACIÓN: Usado por JWT middleware para control de acceso
        /// </summary>
        public string Role { get; set; } = string.Empty; // Rol del usuario (Admin, User, etc)

        /// <summary>
        /// Alias en español para Role - Compatibilidad con tests legacy y UI localizada
        /// PROPÓSITO: Mantener compatibilidad con sistema anterior en español
        /// SINCRONIZACIÓN: Debe mantenerse en sync con propiedad Role
        /// LOCALIZACIÓN: Soporte para interfaces de usuario en español
        /// TESTS: Requerido para compatibilidad con suite de tests existente
        /// FUTURO: Considerar deprecación una vez migrados todos los tests
        /// </summary>
        public string Rol { get; set; } = string.Empty; // Alias en español para Role (para compatibilidad con tests)

        // ========== NOTAS DE DISEÑO ARQUITECTURAL ==========
        // DELIBERADAMENTE OMITIDO: Información sensible (passwords, tokens, PII)
        // RAZÓN: DTOs deben ser objetos planos para serialización segura
        // EXTENSIBILIDAD: Preparado para campos adicionales (department, cost_center)
        // PERFORMANCE: Optimizado para serialización JSON rápida
        // FUTURO: Considerar campos adicionales para integración CRM/ERP:
        // - string Departamento (Marketing, Growth, Product Marketing)
        // - string CentroCoste (para reportes financieros)
        // - DateTime UltimoAcceso (para auditoría de seguridad)
        // - bool Activo (para gestión de usuarios temporales)
        // - List<string> Permisos (para autorización granular)
    }
} 