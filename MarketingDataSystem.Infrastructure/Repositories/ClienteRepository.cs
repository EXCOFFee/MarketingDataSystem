// ==================== REPOSITORIO DE PERSISTENCIA - CLIENTES ====================
// Este repositorio maneja toda la persistencia de datos para la entidad Cliente
// CAPA: Infrastructure - Acceso a datos y persistencia en Clean Architecture
// PATRÓN: Repository Pattern - abstrae el acceso a datos de la lógica de negocio
// SOLID: Cumple principios L (Liskov), S (responsabilidad única), D (inversión dependencias)
// ORM: Utiliza Entity Framework Core para mapeo objeto-relacional automático

using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Infrastructure.Data;

namespace MarketingDataSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio especializado para operaciones de persistencia de clientes
    /// RESPONSABILIDAD: Proporcionar acceso a datos específico para entidad Cliente
    /// PATRÓN: Repository Pattern - encapsula lógica de acceso a datos
    /// HERENCIA: Extiende GenericRepository para operaciones CRUD básicas
    /// SOLID:
    /// - L: Sustituible por IClienteRepository (Principio de Liskov)
    /// - S: Una sola responsabilidad (persistencia de clientes)
    /// - D: Implementa interfaz IClienteRepository (inversión de dependencias)
    /// CLEAN ARCHITECTURE: Capa Infrastructure - detalles de implementación
    /// ORM: Entity Framework Core maneja SQL y mapeo automáticamente
    /// </summary>
    public class ClienteRepository : GenericRepository<Cliente>, IClienteRepository
    {
        /// <summary>
        /// Constructor que inicializa el repositorio con contexto de base de datos
        /// INYECCIÓN: Recibe MarketingDataContext del contenedor de dependencias
        /// HERENCIA: Pasa contexto al GenericRepository padre para operaciones base
        /// PATRÓN: Constructor Injection - dependencia explícita y testeable
        /// </summary>
        /// <param name="context">
        /// Contexto de Entity Framework con configuración de base de datos
        /// Contiene DbSet&lt;Cliente&gt; y configuraciones de mapeo
        /// </param>
        public ClienteRepository(MarketingDataContext context) : base(context) 
        { 
            // ========== INICIALIZACIÓN HEREDADA ==========
            // El constructor base configura:
            // - DbSet<Cliente> para operaciones CRUD
            // - Contexto para transacciones
            // - Change Tracking de Entity Framework
            // - Configuraciones de mapeo definidas en MarketingDataContext
        }
        
        // ========== MÉTODOS HEREDADOS DISPONIBLES ==========
        // Heredados de GenericRepository<Cliente>:
        // - GetByIdAsync(int id) - Buscar cliente por ID
        // - GetAllAsync() - Obtener todos los clientes activos
        // - AddAsync(Cliente entity) - Agregar nuevo cliente
        // - Update(Cliente entity) - Actualizar cliente existente
        // - Delete(Cliente entity) - Eliminar cliente (soft/hard delete)
        // - FindAsync(Expression<Func<Cliente, bool>> predicate) - Búsquedas personalizadas
        
        // ========== MÉTODOS ESPECÍFICOS FUTUROS ==========
        // Este repositorio puede extenderse con métodos específicos para Cliente:
        
        /*
        /// <summary>
        /// Buscar clientes por email (método específico de dominio)
        /// FUTURO: Implementar cuando se requiera funcionalidad específica
        /// </summary>
        public async Task<Cliente> GetByEmailAsync(string email)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Email == email);
        }
        
        /// <summary>
        /// Obtener clientes con sus ventas asociadas (eager loading)
        /// FUTURO: Para consultas que requieran datos relacionados
        /// </summary>
        public async Task<IEnumerable<Cliente>> GetClientesConVentasAsync()
        {
            return await _context.Clientes
                .Include(c => c.Ventas)
                .ToListAsync();
        }
        */
    }
} 