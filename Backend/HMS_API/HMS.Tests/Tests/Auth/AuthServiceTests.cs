using HMS.Business.Auth;
using HMS.Data.Auth;
using HMS.Data.DBModel;
using HMS.Entity.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HMS.Tests.Auth;

public class AuthServiceTests
{
    private readonly Mock<IAuthRepository> _mockRepository;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<ILogger<AuthService>> _mockLogger;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockRepository = new Mock<IAuthRepository>();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockLogger = new Mock<ILogger<AuthService>>();

        _mockConfiguration.Setup(c => c["Jwt:Secret"]).Returns("TestSecretKeyForJWTGenerationPurposesOnly");
        _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("HMS.Test");
        _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("HMS.Test.Clients");
        _mockConfiguration.Setup(c => c["Jwt:AccessTokenExpirationMinutes"]).Returns("60");

        _authService = new AuthService(
            _mockRepository.Object,
            _mockConfiguration.Object,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task HashPasswordAsync_Password_ReturnsHash()
    {
        var password = "TestPassword123!";
        var hash = await _authService.HashPasswordAsync(password);

        Assert.NotNull(hash);
        Assert.NotEqual(password, hash);
        Assert.StartsWith("$2a$", hash);
    }

    [Fact]
    public async Task VerifyPasswordAsync_CorrectPassword_ReturnsTrue()
    {
        var password = "TestPassword123!";
        var hash = await _authService.HashPasswordAsync(password);

        var result = await _authService.VerifyPasswordAsync(password, hash);

        Assert.True(result);
    }

    [Fact]
    public async Task VerifyPasswordAsync_WrongPassword_ReturnsFalse()
    {
        var password = "TestPassword123!";
        var hash = await _authService.HashPasswordAsync(password);
        var wrongPassword = "WrongPassword!";

        var result = await _authService.VerifyPasswordAsync(wrongPassword, hash);

        Assert.False(result);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsToken()
    {
        var password = "CorrectPassword123";
        var passwordHash = await _authService.HashPasswordAsync(password);

        var user = new User
        {
            Id = 1,
            Username = "testuser",
            PasswordHash = passwordHash,
            IsActive = true,
            Roles = new List<Role> { new Role { Id = 1, Name = "Doctor" } }
        };

        _mockRepository.Setup(r => r.GetUserWithRoles("testuser")).Returns(user);

        var request = new LoginRequest { Username = "testuser", Password = password };
        var result = await _authService.LoginAsync(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.RefreshToken);
        Assert.Equal(3600, result.ExpiresIn);
    }

    [Fact]
    public async Task LoginAsync_UserNotFound_ReturnsNull()
    {
        _mockRepository.Setup(r => r.GetUserWithRoles("nonexistent")).Returns((User?)null);

        var request = new LoginRequest { Username = "nonexistent", Password = "password" };
        var result = await _authService.LoginAsync(request, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ReturnsNull()
    {
        var correctPassword = "CorrectPassword123";
        var passwordHash = await _authService.HashPasswordAsync(correctPassword);

        var user = new User
        {
            Id = 1,
            Username = "testuser",
            PasswordHash = passwordHash,
            IsActive = true,
            Roles = new List<Role> { new Role { Id = 1, Name = "Doctor" } }
        };

        _mockRepository.Setup(r => r.GetUserWithRoles("testuser")).Returns(user);

        var request = new LoginRequest { Username = "testuser", Password = "WrongPassword" };
        var result = await _authService.LoginAsync(request, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_InactiveUser_ReturnsNull()
    {
        var password = "CorrectPassword123";
        var passwordHash = await _authService.HashPasswordAsync(password);

        var user = new User
        {
            Id = 1,
            Username = "testuser",
            PasswordHash = passwordHash,
            IsActive = false,
            Roles = new List<Role> { new Role { Id = 1, Name = "Doctor" } }
        };

        _mockRepository.Setup(r => r.GetUserWithRoles("testuser")).Returns(user);

        var request = new LoginRequest { Username = "testuser", Password = password };
        var result = await _authService.LoginAsync(request, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_UserWithMultipleRoles_ReturnsTokenWithFirstRole()
    {
        var password = "CorrectPassword123";
        var passwordHash = await _authService.HashPasswordAsync(password);

        var user = new User
        {
            Id = 1,
            Username = "testuser",
            PasswordHash = passwordHash,
            IsActive = true,
            Roles = new List<Role>
            {
                new Role { Id = 1, Name = "Doctor" },
                new Role { Id = 2, Name = "Nurse" }
            }
        };

        _mockRepository.Setup(r => r.GetUserWithRoles("testuser")).Returns(user);

        var request = new LoginRequest { Username = "testuser", Password = password };
        var result = await _authService.LoginAsync(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Contains("role", result.AccessToken);
    }

    [Fact]
    public async Task RefreshTokenAsync_ValidToken_ReturnsNewToken()
    {
        var password = "CorrectPassword123";
        var passwordHash = await _authService.HashPasswordAsync(password);

        var user = new User
        {
            Id = 1,
            Username = "testuser",
            PasswordHash = passwordHash,
            IsActive = true,
            Roles = new List<Role> { new Role { Id = 1, Name = "Doctor" } }
        };

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = 1,
            TokenHash = await _authService.HashPasswordAsync("validtoken"),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            IsRevoked = false,
            User = user
        };

        _mockRepository.Setup(r => r.GetRefreshTokenByHash(It.IsAny<string>())).Returns(refreshToken);
        _mockRepository.Setup(r => r.RevokeRefreshToken(It.IsAny<Guid>())).Returns(true);

        var result = await _authService.RefreshTokenAsync("validtoken", CancellationToken.None);

        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.RefreshToken);
    }

    [Fact]
    public async Task RefreshTokenAsync_ExpiredToken_ReturnsNull()
    {
        var password = "CorrectPassword123";
        var passwordHash = await _authService.HashPasswordAsync(password);

        var user = new User
        {
            Id = 1,
            Username = "testuser",
            PasswordHash = passwordHash,
            IsActive = true,
            Roles = new List<Role> { new Role { Id = 1, Name = "Doctor" } }
        };

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = 1,
            TokenHash = await _authService.HashPasswordAsync("expiredtoken"),
            ExpiresAt = DateTime.UtcNow.AddDays(-1),
            CreatedAt = DateTime.UtcNow.AddDays(-7),
            IsRevoked = false,
            User = user
        };

        _mockRepository.Setup(r => r.GetRefreshTokenByHash(It.IsAny<string>())).Returns(refreshToken);

        var result = await _authService.RefreshTokenAsync("expiredtoken", CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task RefreshTokenAsync_RevokedToken_ReturnsNull()
    {
        var password = "CorrectPassword123";
        var passwordHash = await _authService.HashPasswordAsync(password);

        var user = new User
        {
            Id = 1,
            Username = "testuser",
            PasswordHash = passwordHash,
            IsActive = true,
            Roles = new List<Role> { new Role { Id = 1, Name = "Doctor" } }
        };

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = 1,
            TokenHash = await _authService.HashPasswordAsync("revokedtoken"),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            IsRevoked = true,
            User = user
        };

        _mockRepository.Setup(r => r.GetRefreshTokenByHash(It.IsAny<string>())).Returns(refreshToken);

        var result = await _authService.RefreshTokenAsync("revokedtoken", CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task LogoutAsync_ValidToken_MarksAsRevoked()
    {
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = 1,
            TokenHash = "hashedtoken",
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            IsRevoked = false
        };

        _mockRepository.Setup(r => r.GetRefreshTokenByHash(It.IsAny<string>())).Returns(refreshToken);
        _mockRepository.Setup(r => r.RevokeRefreshToken(refreshToken.Id)).Returns(true);

        await _authService.LogoutAsync("tokenvalue", CancellationToken.None);

        _mockRepository.Verify(r => r.RevokeRefreshToken(refreshToken.Id), Times.Once);
    }

    [Fact]
    public async Task LogoutAsync_InvalidToken_DoesNotThrow()
    {
        _mockRepository.Setup(r => r.GetRefreshTokenByHash(It.IsAny<string>())).Returns((RefreshToken?)null);

        await _authService.LogoutAsync("invalidtoken", CancellationToken.None);

        _mockRepository.Verify(r => r.RevokeRefreshToken(It.IsAny<Guid>()), Times.Never);
    }
}
