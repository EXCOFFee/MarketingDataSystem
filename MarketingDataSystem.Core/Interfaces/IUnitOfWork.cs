using System;
using System.Threading.Tasks;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Core.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace MarketingDataSystem.Core.Interfaces
{
    /// <summary>
    /// Interfaz para la unidad de trabajo (Unit of Work).
    /// Cumple con el principio de inversión de dependencias (D de SOLID) y el patrón Unit of Work,
    /// centralizando la gestión de transacciones y repositorios.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IUsuarioMarketingRepository UsuariosMarketing { get; }
        IReporteRepository Reportes { get; }
        IDescargaReporteRepository DescargasReporte { get; }
        IFuenteDeDatosRepository FuentesDeDatos { get; }
        IIngestionLogRepository IngestionLogs { get; }
        IDatoCrudoRepository DatosCrudos { get; }
        IDatoNormalizadoRepository DatosNormalizados { get; }
        IMetadataRepository Metadatas { get; }
        IStockRepository Stocks { get; }
        IProductoRepository Productos { get; }
        IClienteRepository Clientes { get; }
        IVentaRepository Ventas { get; }

        /// <summary>
        /// Guarda los cambios en la base de datos
        /// </summary>
        Task<int> SaveChangesAsync();

        Task<IDbContextTransaction> BeginTransactionAsync();
    }
} 