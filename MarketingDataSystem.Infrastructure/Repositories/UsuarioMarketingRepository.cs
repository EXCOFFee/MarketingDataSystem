using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Infrastructure.Data;

namespace MarketingDataSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio concreto para UsuarioMarketing.
    /// Cumple con el principio de sustitución de Liskov (L de SOLID) y el patrón Repository.
    /// </summary>
    public class UsuarioMarketingRepository : GenericRepository<UsuarioMarketing>, IUsuarioMarketingRepository
    {
        public UsuarioMarketingRepository(MarketingDataContext context) : base(context) { }
    }
} 