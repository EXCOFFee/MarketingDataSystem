using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Infrastructure.Data;

namespace MarketingDataSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio concreto para DatoCrudo.
    /// Cumple con el principio de sustitución de Liskov (L de SOLID) y el patrón Repository.
    /// </summary>
    public class DatoCrudoRepository : GenericRepository<DatoCrudo>, IDatoCrudoRepository
    {
        public DatoCrudoRepository(MarketingDataContext context) : base(context) { }
    }
} 