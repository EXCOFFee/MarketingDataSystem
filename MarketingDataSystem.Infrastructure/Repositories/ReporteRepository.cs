using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Infrastructure.Data;

namespace MarketingDataSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio concreto para Reporte.
    /// Cumple con el principio de sustitución de Liskov (L de SOLID) y el patrón Repository.
    /// </summary>
    public class ReporteRepository : GenericRepository<Reporte>, IReporteRepository
    {
        public ReporteRepository(MarketingDataContext context) : base(context) { }
    }
} 