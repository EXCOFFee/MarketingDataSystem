using System;
using System.Threading.Tasks;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Infrastructure.Data;
using MarketingDataSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace MarketingDataSystem.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Implementación concreta del patrón Unit of Work.
    /// Centraliza la gestión de repositorios y transacciones.
    /// Cumple con los principios SOLID:
    /// - S: Responsabilidad única (gestión de transacciones)
    /// - O: Abierto/Cerrado (extensible con nuevos repositorios)
    /// - L: Sustitución de Liskov (implementa correctamente IUnitOfWork)
    /// - I: Segregación de interfaces (implementa solo lo necesario)
    /// - D: Inversión de dependencias (depende de abstracciones)
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MarketingDataContext _context;
        private bool _disposed = false;

        // Repositorios lazy-loaded
        private IUsuarioMarketingRepository? _usuariosMarketing;
        private IReporteRepository? _reportes;
        private IDescargaReporteRepository? _descargasReporte;
        private IFuenteDeDatosRepository? _fuentesDeDatos;
        private IIngestionLogRepository? _ingestionLogs;
        private IDatoCrudoRepository? _datosCrudos;
        private IDatoNormalizadoRepository? _datosNormalizados;
        private IMetadataRepository? _metadatas;
        private IStockRepository? _stocks;
        private IProductoRepository? _productos;
        private IClienteRepository? _clientes;
        private IVentaRepository? _ventas;

        public UnitOfWork(MarketingDataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Propiedades de repositorios con lazy loading
        public IUsuarioMarketingRepository UsuariosMarketing =>
            _usuariosMarketing ??= new UsuarioMarketingRepository(_context);

        public IReporteRepository Reportes =>
            _reportes ??= new ReporteRepository(_context);

        public IDescargaReporteRepository DescargasReporte =>
            _descargasReporte ??= new DescargaReporteRepository(_context);

        public IFuenteDeDatosRepository FuentesDeDatos =>
            _fuentesDeDatos ??= new FuenteDeDatosRepository(_context);

        public IIngestionLogRepository IngestionLogs =>
            _ingestionLogs ??= new IngestionLogRepository(_context);

        public IDatoCrudoRepository DatosCrudos =>
            _datosCrudos ??= new DatoCrudoRepository(_context);

        public IDatoNormalizadoRepository DatosNormalizados =>
            _datosNormalizados ??= new DatoNormalizadoRepository(_context);

        public IMetadataRepository Metadatas =>
            _metadatas ??= new MetadataRepository(_context);

        public IStockRepository Stocks =>
            _stocks ??= new StockRepository(_context);

        public IProductoRepository Productos =>
            _productos ??= new ProductoRepository(_context);

        public IClienteRepository Clientes =>
            _clientes ??= new ClienteRepository(_context);

        public IVentaRepository Ventas =>
            _ventas ??= new VentaRepository(_context);

        /// <summary>
        /// Guarda todos los cambios pendientes en una sola transacción
        /// </summary>
        /// <returns>Número de entidades afectadas</returns>
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (usando el sistema de logging existente)
                throw new InvalidOperationException("Error al guardar cambios en la base de datos", ex);
            }
        }

        /// <summary>
        /// Inicia una transacción explícita para operaciones complejas
        /// </summary>
        /// <returns>Transacción de Entity Framework</returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Libera los recursos utilizados
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context?.Dispose();
            }
            _disposed = true;
        }

        /// <summary>
        /// Implementación del patrón Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
} 