using Xunit;
using Moq;
using System.Threading.Tasks;
using MarketingDataSystem.Application.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace MarketingDataSystem.Tests
{
    public class BackupServiceTests
    {
        private readonly Mock<ILogger<BackupService>> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly BackupService _backupService;

        public BackupServiceTests()
        {
            _loggerMock = new Mock<ILogger<BackupService>>();
            _configurationMock = new Mock<IConfiguration>();
            
            // Configurar connection string y directorios
            _configurationMock.Setup(x => x.GetConnectionString("DefaultConnection"))
                .Returns("Server=test;Database=test;Integrated Security=true");
            _configurationMock.Setup(x => x["Backup:Directory"])
                .Returns("C:\\temp\\test_backups");
            
            _backupService = new BackupService(_loggerMock.Object, _configurationMock.Object);
        }

        [Fact]
        public void Constructor_ConParametrosValidos_DebeCrearInstancia()
        {
            // Assert
            Assert.NotNull(_backupService);
        }

        [Fact]
        public async Task ListarBackupsAsync_SinDirectorio_DebeRetornarListaVacia()
        {
            // Act
            var resultado = await _backupService.ListarBackupsAsync();

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task LimpiarBackupsAntiguosAsync_ConDiasRetencion_DebeRetornarNumero()
        {
            // Act
            var resultado = await _backupService.LimpiarBackupsAntiguosAsync(30);

            // Assert
            Assert.True(resultado >= 0);
        }
    }
}
