using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Infrastructure.Data;

namespace MarketingDataSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio concreto para DescargaReporte.
    /// Cumple con el principio de sustitución de Liskov (L de SOLID) y el patrón Repository.
    /// </summary>
    public class DescargaReporteRepository : GenericRepository<DescargaReporte>, IDescargaReporteRepository
    {
        public DescargaReporteRepository(MarketingDataContext context) : base(context) { }
    }
} 