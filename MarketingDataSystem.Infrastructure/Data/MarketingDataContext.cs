using Microsoft.EntityFrameworkCore;
using MarketingDataSystem.Core.Entities;

namespace MarketingDataSystem.Infrastructure.Data
{
    /// <summary>
    /// Contexto de la base de datos para el sistema de marketing
    /// </summary>
    public class MarketingDataContext : DbContext
    {
        public MarketingDataContext(DbContextOptions<MarketingDataContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet para la entidad Usuario
        /// </summary>
        public DbSet<Usuario> Usuarios { get; set; }

        /// <summary>
        /// DbSet para la entidad UsuarioMarketing
        /// </summary>
        public DbSet<UsuarioMarketing> UsuariosMarketing { get; set; }

        /// <summary>
        /// DbSet para la entidad Reporte
        /// </summary>
        public DbSet<Reporte> Reportes { get; set; }

        /// <summary>
        /// DbSet para la entidad DescargaReporte
        /// </summary>
        public DbSet<DescargaReporte> DescargasReporte { get; set; }

        /// <summary>
        /// DbSet para la entidad FuenteDeDatos
        /// </summary>
        public DbSet<FuenteDeDatos> FuentesDeDatos { get; set; }

        /// <summary>
        /// DbSet para la entidad IngestionLog
        /// </summary>
        public DbSet<IngestionLog> IngestionLogs { get; set; }

        /// <summary>
        /// DbSet para la entidad DatoCrudo
        /// </summary>
        public DbSet<DatoCrudo> DatosCrudos { get; set; }

        /// <summary>
        /// DbSet para la entidad DatoNormalizado
        /// </summary>
        public DbSet<DatoNormalizado> DatosNormalizados { get; set; }

        /// <summary>
        /// DbSet para la entidad Metadata
        /// </summary>
        public DbSet<Metadata> Metadatas { get; set; }

        /// <summary>
        /// DbSet para la entidad Stock
        /// </summary>
        public DbSet<Stock> Stocks { get; set; }

        /// <summary>
        /// DbSet para la entidad Producto
        /// </summary>
        public DbSet<Producto> Productos { get; set; }

        /// <summary>
        /// DbSet para la entidad Cliente
        /// </summary>
        public DbSet<Cliente> Clientes { get; set; }

        /// <summary>
        /// DbSet para la entidad Venta
        /// </summary>
        public DbSet<Venta> Ventas { get; set; }

        // Listo para agregar nuevas entidades del dominio según el SRS

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Limpiar configuraciones antiguas. Listo para nuevas entidades.
        }
    }
} 