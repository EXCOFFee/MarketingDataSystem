using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Infrastructure.Data;

namespace MarketingDataSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio concreto para Stock.
    /// Cumple con el principio de sustitución de Liskov (L de SOLID) y el patrón Repository.
    /// </summary>
    public class StockRepository : GenericRepository<Stock>, IStockRepository
    {
        public StockRepository(MarketingDataContext context) : base(context) { }
    }
} 