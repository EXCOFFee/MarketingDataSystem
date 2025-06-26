using Microsoft.EntityFrameworkCore;
using MarketingDataSystem.Core.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace MarketingDataSystem.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets para todas las entidades
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<UsuarioMarketing> UsuariosMarketing { get; set; }
        public DbSet<FuenteDeDatos> FuentesDeDatos { get; set; }
        public DbSet<DatoCrudo> DatosCrudos { get; set; }
        public DbSet<DatoNormalizado> DatosNormalizados { get; set; }
        public DbSet<Reporte> Reportes { get; set; }
        public DbSet<DescargaReporte> DescargasReporte { get; set; }
        public DbSet<Metadata> Metadatas { get; set; }
        public DbSet<IngestionLog> IngestionLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar relaciones y restricciones
            ConfigurarEntidades(modelBuilder);
            
            // Agregar datos semilla
            SeedData(modelBuilder);
        }

        private void ConfigurarEntidades(ModelBuilder modelBuilder)
        {
            // Configuración de Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            });

            // Configuración de Producto
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Precio).HasColumnType("decimal(18,2)");
            });

            // Configuración de Venta
            modelBuilder.Entity<Venta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PrecioUnitario).HasColumnType("real");
                entity.HasOne<Cliente>().WithMany().HasForeignKey("IdCliente").OnDelete(DeleteBehavior.NoAction);
                entity.HasOne<Producto>().WithMany().HasForeignKey("IdProducto").OnDelete(DeleteBehavior.NoAction);
            });

            // Configuración de Stock
            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<Producto>().WithMany().HasForeignKey("IdProducto").OnDelete(DeleteBehavior.NoAction);
            });
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Usuario administrador por defecto - simplificado para evitar problemas de migración
            modelBuilder.Entity<UsuarioMarketing>().HasData(
                new UsuarioMarketing 
                { 
                    Id = 1, 
                    Username = "admin",
                    Nombre = "Administrador",
                    Email = "admin@marketingsystem.com",
                    PasswordHash = "$2a$11$8K1p/a0dclZhDnID4ZpOyeJ9pz26.EkwG/z7zKmFPQ/6hZv2K.nT.",  // admin123
                    Role = "Admin",
                    FechaCreacion = new DateTime(2024, 1, 1),
                    Activo = true
                }
            );

            // Los demás datos se pueden agregar a través de la API una vez que el sistema esté funcionando
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return Database.BeginTransactionAsync();
        }
    }
} 