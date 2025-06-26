using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Infrastructure.Data;

namespace MarketingDataSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio concreto para DatoNormalizado.
    /// Cumple con el principio de sustitución de Liskov (L de SOLID) y el patrón Repository.
    /// </summary>
    public class DatoNormalizadoRepository : GenericRepository<DatoNormalizado>, IDatoNormalizadoRepository
    {
        public DatoNormalizadoRepository(MarketingDataContext context) : base(context) { }
    }
} 