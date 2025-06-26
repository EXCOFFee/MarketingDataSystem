using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Infrastructure.Data;

namespace MarketingDataSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio concreto para Venta.
    /// Cumple con el principio de sustitución de Liskov (L de SOLID) y el patrón Repository.
    /// </summary>
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        public VentaRepository(MarketingDataContext context) : base(context) { }
    }
} 