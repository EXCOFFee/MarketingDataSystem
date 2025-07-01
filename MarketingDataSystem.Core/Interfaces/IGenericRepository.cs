// ==================== INTERFAZ CORE CRÍTICA - GENERIC REPOSITORY ====================
// SISTEMA: MarketingDataSystem - Sistema Empresarial de Gestión de Datos de Marketing
// COMPONENTE: Interfaz Base de Acceso a Datos - ALTA CRITICIDAD
// RESPONSABILIDAD: Abstracción de operaciones CRUD con patrones empresariales
// PATRÓN: Repository Pattern - Abstrae acceso a datos de la lógica de negocio
// PRINCIPIO: Dependency Inversion (D de SOLID) - Independiente de implementación
// GENERICIDAD: Tipo genérico para reutilización en todas las entidades del sistema
// TESTING: Fundamental para testing unitario y mocking de acceso a datos

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MarketingDataSystem.Core.Entities;

namespace MarketingDataSystem.Core.Interfaces
{
    /// <summary>
    /// INTERFAZ CORE CRÍTICA: Repositorio Genérico de Acceso a Datos - ALTA CRITICIDAD
    /// ==============================================================================
    /// 
    /// PROPÓSITO EMPRESARIAL:
    /// Esta interfaz define el contrato base para todos los repositorios del sistema,
    /// implementando el patrón Repository para abstraer el acceso a datos de la lógica
    /// de negocio. Es fundamental para mantenibilidad, testing y separación de responsabilidades.
    /// 
    /// CASOS DE USO EMPRESARIALES CRÍTICOS:
    /// 1. CRUD OPERATIONS: Operaciones básicas de Create, Read, Update, Delete para todas las entidades
    /// 2. DATA ABSTRACTION: Abstracción completa de la tecnología de persistencia utilizada
    /// 3. UNIT TESTING: Facilita testing unitario mediante mocking e inyección de dependencias
    /// 4. BUSINESS LOGIC SEPARATION: Separa lógica de negocio de detalles de persistencia
    /// 5. QUERY OPTIMIZATION: Centraliza optimizaciones de consultas y performance
    /// 6. AUDIT TRAIL: Implementación consistente de auditoría en todas las entidades
    /// 7. SECURITY: Control de acceso uniforme a nivel de datos para todas las entidades
    /// 8. CACHING: Implementación centralizada de estrategias de cache por entidad
    /// 9. TRANSACTION MANAGEMENT: Coordinación con Unit of Work para gestión transaccional
    /// 10. DATA VALIDATION: Validaciones de integridad de datos antes de persistencia
    /// 
    /// ARQUITECTURA:
    /// - PATRÓN: Repository Pattern - Encapsula lógica de acceso a datos
    /// - PRINCIPIO: Dependency Inversion - Depende de abstracciones, no implementaciones
    /// - GENERIC CONSTRAINT: Restringido a BaseEntity para garantizar propiedades comunes
    /// - ABSTRACTION LEVEL: Alto nivel de abstracción independiente de ORM específico
    /// 
    /// BENEFICIOS EMPRESARIALES:
    /// - MAINTAINABILITY: Facilita mantenimiento y evolución del código
    /// - TESTABILITY: Permite testing unitario completo de lógica de negocio
    /// - FLEXIBILITY: Facilita cambio de tecnologías de persistencia
    /// - CONSISTENCY: Garantiza implementación consistente de operaciones CRUD
    /// - REUSABILITY: Reutilización de código común para todas las entidades
    /// 
    /// PERFORMANCE CONSIDERATIONS:
    /// - ASYNC OPERATIONS: Todas las operaciones son asíncronas para escalabilidad
    /// - QUERY OPTIMIZATION: Soporte para consultas optimizadas mediante predicados
    /// - LAZY LOADING: Consideraciones para carga bajo demanda de entidades relacionadas
    /// - BATCH OPERATIONS: Potencial para operaciones por lote en implementaciones
    /// - CONNECTION POOLING: Optimización de conexiones de base de datos
    /// 
    /// SECURITY ASPECTS:
    /// - ACCESS CONTROL: Control de acceso a nivel de repositorio
    /// - DATA SANITIZATION: Sanitización de datos de entrada en consultas
    /// - AUDIT LOGGING: Registro de operaciones para compliance y security
    /// - INJECTION PREVENTION: Prevención de inyecciones SQL mediante predicados seguros
    /// 
    /// COMPLIANCE Y AUDITORÍA:
    /// - GDPR: Soporte para operaciones de privacidad (rectificación, olvido)
    /// - SOX: Auditoría de cambios en datos financieros críticos
    /// - DATA LINEAGE: Trazabilidad de operaciones de datos para compliance
    /// - BACKUP INTEGRATION: Integración con estrategias de backup y recovery
    /// </summary>
    /// <typeparam name="T">
    /// Tipo de entidad que debe heredar de BaseEntity para garantizar propiedades comunes
    /// como identificadores únicos, timestamps de auditoría y soft delete capabilities
    /// </typeparam>
    public interface IGenericRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Obtiene una entidad por su ID
        /// </summary>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene todas las entidades
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Obtiene entidades que cumplen con un predicado
        /// </summary>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Agrega una nueva entidad
        /// </summary>
        Task AddAsync(T entity);

        /// <summary>
        /// Actualiza una entidad existente
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// Elimina una entidad
        /// </summary>
        void Delete(T entity);

        /// <summary>
        /// Guarda los cambios en la base de datos
        /// </summary>
        Task<int> SaveChangesAsync();
    }
} 