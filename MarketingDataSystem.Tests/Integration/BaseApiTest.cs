using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MarketingDataSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Linq;

namespace MarketingDataSystem.Tests.Integration
{
    public abstract class BaseApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly WebApplicationFactory<Program> _factory;
        protected readonly HttpClient _client;
        protected readonly ApplicationDbContext _context;

        protected BaseApiTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Configurar base de datos en memoria para pruebas
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}");
                    });

                    // Configurar servicios adicionales si es necesario
                    ConfigureServices(services);
                });

                builder.ConfigureAppConfiguration((context, config) =>
                {
                    // Configurar configuración adicional si es necesario
                    ConfigureAppConfiguration(context, config);
                });
            });

            _client = _factory.CreateClient();
            _context = _factory.Services.GetRequiredService<ApplicationDbContext>();
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            // Sobrescribir en clases derivadas para configurar servicios adicionales
        }

        protected virtual void ConfigureAppConfiguration(WebHostBuilderContext context, IConfigurationBuilder config)
        {
            // Sobrescribir en clases derivadas para configurar configuración adicional
        }

        protected async Task CleanupDatabase()
        {
            _context.Database.EnsureDeleted();
            await _context.Database.EnsureCreatedAsync();
        }
    }
} 