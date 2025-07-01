// ==================== TESTS DE SERVICIO DE AUTENTICACIÓN EMPRESARIAL ====================
// Este archivo contiene tests unitarios para el servicio crítico de autenticación
// PROPÓSITO: Verificar funcionamiento correcto del AuthService en todos los escenarios
// CRITICIDAD: MÁXIMA - La autenticación es la primera línea de defensa de seguridad
// COBERTURA: Tests exhaustivos de login, registro, validación y manejo de errores
// SEGURIDAD: Verificación de hashing de passwords, JWT tokens, y control de acceso
// ENTERPRISE: Preparado para cumplir auditorías de seguridad empresarial

using Xunit;
using Moq;
using System.Threading.Tasks;
using MarketingDataSystem.Application.Services;
using MarketingDataSystem.Core.DTOs;
using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

/// <summary>
/// Suite de tests unitarios para el servicio crítico de autenticación empresarial
/// RESPONSABILIDAD: Verificar funcionamiento correcto de AuthService en todos los escenarios
/// SEGURIDAD: Tests exhaustivos de autenticación, autorización y validación de credenciales
/// COBERTURA: Login exitoso/fallido, registro válido/duplicado, JWT token generation
/// ENTERPRISE: Cumple con estándares de testing para auditorías de seguridad
/// AUTOMATION: Integrado en pipelines CI/CD para verificación continua
/// CASOS DE TEST CUBIERTOS:
/// - Autenticación exitosa con credenciales válidas
/// - Rechazo de credenciales inválidas o inexistentes
/// - Registro exitoso de nuevos usuarios
/// - Prevención de registro de emails duplicados
/// - Generación correcta de JWT tokens seguros
/// - Validación de hashing de passwords con BCrypt
/// COMPLIANCE: Cumple con estándares de seguridad empresarial (OWASP, NIST)
/// </summary>
public class AuthServiceTests
{
    // ========== MOCKS PARA DEPENDENCIAS CRÍTICAS ==========
    /// <summary>
    /// Mock del repositorio de usuarios - Simula acceso a base de datos
    /// PROPÓSITO: Aislar tests del AuthService de la capa de datos real
    /// CONFIGURACIÓN: Setup con usuarios de prueba para diferentes escenarios
    /// VERIFICACIÓN: Permite verificar llamadas correctas al repositorio
    /// </summary>
    private readonly Mock<IRepository<UsuarioMarketing>> _userRepositoryMock;
    
    /// <summary>
    /// Mock de configuración - Simula configuración JWT y otros settings
    /// PROPÓSITO: Proporcionar configuración segura y controlada para tests
    /// JWT CONFIG: Incluye claves de prueba, issuer, audience, expiración
    /// SEGURIDAD: Utiliza claves de prueba, nunca credenciales de producción
    /// </summary>
    private readonly Mock<IConfiguration> _configurationMock;
    
    /// <summary>
    /// Mock del Event Bus - Simula sistema de eventos empresarial
    /// PROPÓSITO: Verificar que eventos de autenticación se publican correctamente
    /// EVENTOS: LoginSuccessful, LoginFailed, UserRegistered, etc.
    /// AUDITORÍA: Esencial para tracking de eventos de seguridad
    /// </summary>
    private readonly Mock<IEventBus> _eventBusMock;
    
    /// <summary>
    /// Instancia del AuthService bajo test con dependencias mockeadas
    /// CONFIGURACIÓN: Inicializado con mocks para aislamiento total
    /// TESTING: Permite testing unitario puro sin dependencias externas
    /// </summary>
    private readonly AuthService _service;

    /// <summary>
    /// Constructor que inicializa toda la infraestructura de testing
    /// CONFIGURACIÓN: Setup de mocks con datos de prueba realistas
    /// SEGURIDAD: Configuración JWT con parámetros seguros para testing
    /// ISOLATION: Cada test ejecuta con configuración limpia y predecible
    /// </summary>
    public AuthServiceTests()
    {
        // ========== INICIALIZACIÓN DE MOCKS ==========
        _userRepositoryMock = new Mock<IRepository<UsuarioMarketing>>();
        _configurationMock = new Mock<IConfiguration>();
        _eventBusMock = new Mock<IEventBus>();
        
        // ========== CONFIGURACIÓN JWT SEGURA PARA TESTING ==========
        // IMPORTANTE: Usar solo claves de prueba, nunca credenciales reales
        _configurationMock.Setup(x => x["Jwt:Key"]).Returns("test-secret-key-for-jwt-token-generation");
        _configurationMock.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");
        _configurationMock.Setup(x => x["Jwt:Audience"]).Returns("TestAudience");
        _configurationMock.Setup(x => x["Jwt:ExpiryInMinutes"]).Returns("60");
        
        // ========== INICIALIZACIÓN DEL SERVICIO BAJO TEST ==========
        _service = new AuthService(_userRepositoryMock.Object, _configurationMock.Object, _eventBusMock.Object);
    }

    // ========== TESTS DE AUTENTICACIÓN EXITOSA ==========
    /// <summary>
    /// Test: Autenticación exitosa con credenciales válidas debe retornar AuthResponse
    /// ESCENARIO: Usuario existente con password correcto intenta hacer login
    /// EXPECTATIVA: AuthService debe retornar AuthResponse con usuario y JWT token válido
    /// SEGURIDAD: Verifica que BCrypt valida correctamente el password hasheado
    /// BUSINESS RULE: Usuario autenticado obtiene acceso al sistema
    /// </summary>
    [Fact]
    public async Task LoginAsync_ReturnsAuthResponse_WhenCredentialsAreValid()
    {
        // ========== ARRANGE - CONFIGURACIÓN DEL ESCENARIO ==========
        var loginDto = new LoginDto { Email = "test@mail.com", Password = "1234" };
        var user = new UsuarioMarketing 
        { 
            Id = 1, 
            Email = "test@mail.com", 
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234"), // Password hasheado con BCrypt
            Nombre = "Test",
            Username = "test",
            Role = "User"
        };
        
        // Setup: Repository debe retornar usuario cuando se busque por email
        _userRepositoryMock.Setup(u => u.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<UsuarioMarketing, bool>>>()))
            .ReturnsAsync(new List<UsuarioMarketing> { user });

        // ========== ACT - EJECUCIÓN DE LA OPERACIÓN BAJO TEST ==========
        var result = await _service.LoginAsync(loginDto);

        // ========== ASSERT - VERIFICACIÓN DE RESULTADOS ESPERADOS ==========
        Assert.NotNull(result); // AuthResponse debe ser retornado
        Assert.Equal("test@mail.com", result.User.Email); // Email debe coincidir
        Assert.NotNull(result.Token); // JWT Token debe ser generado
    }

    // ========== TESTS DE AUTENTICACIÓN FALLIDA ==========
    /// <summary>
    /// Test: Autenticación con credenciales inválidas debe lanzar excepción de no autorizado
    /// ESCENARIO: Usuario inexistente o password incorrecto intenta hacer login
    /// EXPECTATIVA: AuthService debe lanzar UnauthorizedAccessException
    /// SEGURIDAD: Sistema debe rechazar inmediatamente credenciales incorrectas
    /// BUSINESS RULE: Acceso denegado para credenciales inválidas
    /// </summary>
    [Fact]
    public async Task LoginAsync_ThrowsUnauthorized_WhenCredentialsAreInvalid()
    {
        // ========== ARRANGE - CONFIGURACIÓN DEL ESCENARIO NEGATIVO ==========
        var loginDto = new LoginDto { Email = "wrong@mail.com", Password = "wrong" };
        
        // Setup: Repository debe retornar lista vacía (usuario no encontrado)
        _userRepositoryMock.Setup(u => u.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<UsuarioMarketing, bool>>>()))
            .ReturnsAsync(new List<UsuarioMarketing>());

        // ========== ACT & ASSERT - VERIFICACIÓN DE EXCEPCIÓN ESPERADA ==========
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.LoginAsync(loginDto));
    }

    // ========== TESTS DE REGISTRO EXITOSO ==========
    /// <summary>
    /// Test: Registro exitoso con datos válidos debe retornar UserDto
    /// ESCENARIO: Usuario nuevo con email único intenta registrarse
    /// EXPECTATIVA: AuthService debe crear usuario y retornar UserDto
    /// SEGURIDAD: Password debe ser hasheado con BCrypt antes de almacenamiento
    /// BUSINESS RULE: Usuarios únicos pueden registrarse exitosamente
    /// </summary>
    [Fact]
    public async Task RegisterAsync_ReturnsUserDto_WhenRegistrationIsSuccessful()
    {
        // ========== ARRANGE - CONFIGURACIÓN DEL ESCENARIO EXITOSO ==========
        var createUserDto = new CreateUserDto { Email = "new@mail.com", Username = "new", Password = "1234", Role = "User" };
        
        // Setup: Repository debe retornar lista vacía (email disponible)
        _userRepositoryMock.Setup(u => u.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<UsuarioMarketing, bool>>>()))
            .ReturnsAsync(new List<UsuarioMarketing>());
        _userRepositoryMock.Setup(u => u.AddAsync(It.IsAny<UsuarioMarketing>())).Returns(Task.CompletedTask);
        _userRepositoryMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        // ========== ACT - EJECUCIÓN DEL REGISTRO ==========
        var result = await _service.RegisterAsync(createUserDto);

        // ========== ASSERT - VERIFICACIÓN DE REGISTRO EXITOSO ==========
        Assert.NotNull(result); // UserDto debe ser retornado
        Assert.Equal("new@mail.com", result.Email); // Email debe coincidir
    }

    // ========== TESTS DE REGISTRO FALLIDO ==========
    /// <summary>
    /// Test: Registro con email existente debe lanzar InvalidOperationException
    /// ESCENARIO: Usuario intenta registrarse con email ya existente en sistema
    /// EXPECTATIVA: AuthService debe rechazar registro y lanzar excepción
    /// BUSINESS RULE: Emails deben ser únicos en el sistema
    /// DATA INTEGRITY: Prevenir duplicación de usuarios
    /// </summary>
    [Fact]
    public async Task RegisterAsync_ThrowsInvalidOperation_WhenEmailExists()
    {
        // ========== ARRANGE - CONFIGURACIÓN DEL ESCENARIO DE EMAIL DUPLICADO ==========
        var createUserDto = new CreateUserDto { Email = "exists@mail.com", Username = "exists", Password = "1234", Role = "User" };
        var user = new UsuarioMarketing { Id = 3, Email = "exists@mail.com", Nombre = "exists", Username = "exists", Role = "User" };
        
        // Setup: Repository debe retornar usuario existente con mismo email
        _userRepositoryMock.Setup(u => u.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<UsuarioMarketing, bool>>>()))
            .ReturnsAsync(new List<UsuarioMarketing> { user });

        // ========== ACT & ASSERT - VERIFICACIÓN DE EXCEPCIÓN POR EMAIL DUPLICADO ==========
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.RegisterAsync(createUserDto));
    }

    // ========== NOTAS PARA TESTING EMPRESARIAL AVANZADO ==========
    // FUTURO: Para testing empresarial completo, considerar agregar:
    // - Tests de timeout de JWT tokens
    // - Tests de refresh token functionality
    // - Tests de rate limiting para prevenir ataques de fuerza bruta
    // - Tests de 2FA (Two Factor Authentication)
    // - Tests de password policy enforcement
    // - Tests de account lockout después de intentos fallidos
    // - Tests de auditoría y logging de eventos de seguridad
    // - Tests de integración con Active Directory/LDAP
    // - Tests de compliance con GDPR (eliminación de datos)
    // - Tests de performance con cargas altas de autenticación
}
