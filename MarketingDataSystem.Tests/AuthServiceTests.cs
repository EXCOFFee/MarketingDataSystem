using Xunit;
using Moq;
using System.Threading.Tasks;
using MarketingDataSystem.Application.Services;
using MarketingDataSystem.Core.DTOs;
using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Configuration;

public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _configurationMock = new Mock<IConfiguration>();
        
        // Configurar la sección JWT del configuration
        _configurationMock.Setup(x => x["Jwt:Key"]).Returns("test-secret-key-for-jwt-token-generation");
        _configurationMock.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");
        _configurationMock.Setup(x => x["Jwt:Audience"]).Returns("TestAudience");
        _configurationMock.Setup(x => x["Jwt:ExpiryInMinutes"]).Returns("60");
        
        _service = new AuthService(_unitOfWorkMock.Object, _mapperMock.Object, _configurationMock.Object);
    }

    [Fact]
    public async Task LoginAsync_ReturnsAuthResponse_WhenCredentialsAreValid()
    {
        var loginDto = new LoginDto { Email = "test@mail.com", Password = "1234" };
        var user = new UsuarioMarketing { Id = 1, Email = "test@mail.com", Password = BCrypt.Net.BCrypt.HashPassword("1234"), Nombre = "Test" };
        
        _unitOfWorkMock.Setup(u => u.UsuariosMarketing.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<UsuarioMarketing, bool>>>()))
            .ReturnsAsync(new[] { user });
        _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(new UserDto { Id = 1, Email = "test@mail.com", Username = "Test" });

        var result = await _service.LoginAsync(loginDto);

        Assert.NotNull(result);
        Assert.Equal("test@mail.com", result.User.Email);
        Assert.NotNull(result.Token);
    }

    [Fact]
    public async Task LoginAsync_ThrowsUnauthorized_WhenCredentialsAreInvalid()
    {
        var loginDto = new LoginDto { Email = "wrong@mail.com", Password = "wrong" };
        _unitOfWorkMock.Setup(u => u.UsuariosMarketing.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<UsuarioMarketing, bool>>>()))
            .ReturnsAsync(new UsuarioMarketing[0]);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.LoginAsync(loginDto));
    }

    [Fact]
    public async Task RegisterAsync_ReturnsUserDto_WhenRegistrationIsSuccessful()
    {
        var createUserDto = new CreateUserDto { Email = "new@mail.com", Username = "new", Password = "1234", Role = "User" };
        var user = new UsuarioMarketing { Id = 2, Email = "new@mail.com", Nombre = "new" };
        
        _unitOfWorkMock.Setup(u => u.UsuariosMarketing.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<UsuarioMarketing, bool>>>()))
            .ReturnsAsync(new UsuarioMarketing[0]);
        _mapperMock.Setup(m => m.Map<UsuarioMarketing>(createUserDto)).Returns(user);
        _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(new UserDto { Id = 2, Email = "new@mail.com", Username = "new" });
        _unitOfWorkMock.Setup(u => u.UsuariosMarketing.AddAsync(user)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _service.RegisterAsync(createUserDto);

        Assert.NotNull(result);
        Assert.Equal("new@mail.com", result.Email);
    }

    [Fact]
    public async Task RegisterAsync_ThrowsInvalidOperation_WhenEmailExists()
    {
        var createUserDto = new CreateUserDto { Email = "exists@mail.com", Username = "exists", Password = "1234", Role = "User" };
        var user = new UsuarioMarketing { Id = 3, Email = "exists@mail.com", Nombre = "exists" };
        _unitOfWorkMock.Setup(u => u.UsuariosMarketing.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<UsuarioMarketing, bool>>>()))
            .ReturnsAsync(new[] { user });

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.RegisterAsync(createUserDto));
    }
} 