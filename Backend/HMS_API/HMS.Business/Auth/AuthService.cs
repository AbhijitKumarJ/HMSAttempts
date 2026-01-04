using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using HMS.Data.Auth;
using HMS.Data.DBModel;
using HMS.Entity.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace HMS.Business.Auth;

public interface IAuthService
{
    // JObject GetUsers(int limit);
    // JObject GetUser(int id);
    // JObject CreateUser(CreateUserEntity entity);
    // JObject UpdateUser(UpdateUserEntity entity);
    // JObject DeleteUser(int id);

    Task<TokenResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<TokenResponse?> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    Task LogoutAsync(string refreshToken, CancellationToken cancellationToken);
    Task<string> HashPasswordAsync(string password);
    Task<bool> VerifyPasswordAsync(string password, string hash);
}

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IAuthRepository authRepository, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _authRepository = authRepository;
        _configuration = configuration;
        _logger = logger;
    }

    // public JObject GetUsers(int limit)
    // {
    //     return new JObject { ["Message"] = $"Retrieved {limit} users from Auth service." };
    // }

    // public JObject GetUser(int id)
    // {
    //     var user = _authRepository.GetUserById(id);
    //     return new JObject { ["Message"] = $"Retrieved user with ID {id} from Auth service.", ["Data"] = Newtonsoft.Json.Linq.JObject.FromObject(user) };
    // }

    // public JObject CreateUser(CreateUserEntity entity)
    // {
    //     return new JObject { ["Message"] = $"Created user: {entity.Username} in Auth service." };
    // }

    // public JObject UpdateUser(UpdateUserEntity entity)
    // {
    //     return new JObject { ["Message"] = $"Updated user with ID {entity.UserId} in Auth service." };
    // }

    // public JObject DeleteUser(int id)
    // {
    //     return new JObject { ["Message"] = $"Deleted user with ID {id} from Auth service." };
    // }

    public async Task<TokenResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = _authRepository.GetUserWithRoles(request.Username);
        if (user == null)
        {
            _logger.LogWarning("Login failed: User not found - {Username}", request.Username);
            return null;
        }

        var isPasswordValid = await VerifyPasswordAsync(request.Password, user.PasswordHash??"");
        if (!isPasswordValid)
        {
            _logger.LogWarning("Login failed: Invalid password for user - {Username}", request.Username);
            return null;
        }

        if (user.IsActive != true)
        {
            _logger.LogWarning("Login failed: User is inactive - {Username}", request.Username);
            return null;
        }

        var accessToken = await GenerateJwtTokenAsync(user);
        var refreshToken = await GenerateRefreshTokenAsync(user);

        _logger.LogInformation("User logged in successfully - {Username}", request.Username);
        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = 3600,
            IssuedAt = DateTime.UtcNow
        };
    }

    public async Task<TokenResponse?> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var tokenHash = HashToken(refreshToken);
        var storedToken = _authRepository.GetRefreshTokenByHash(tokenHash);

        if (storedToken == null)
        {
            _logger.LogWarning("Refresh token failed: Token not found");
            return null;
        }

        if (storedToken.IsRevoked == true)
        {
            _logger.LogWarning("Refresh token failed: Token is revoked - {TokenId}", storedToken.Id);
            return null;
        }

        if (storedToken.ExpiresAt < DateTime.UtcNow)
        {
            _logger.LogWarning("Refresh token failed: Token expired - {TokenId}", storedToken.Id);
            return null;
        }

        if (storedToken.User == null || storedToken.User.IsActive != true)
        {
            _logger.LogWarning("Refresh token failed: User inactive or deleted - {TokenId}", storedToken.Id);
            return null;
        }

        var accessToken = await GenerateJwtTokenAsync(storedToken.User);
        var newRefreshToken = await GenerateRefreshTokenAsync(storedToken.User);

        _authRepository.RevokeRefreshToken(storedToken.Id);

        _logger.LogInformation("Token refreshed successfully for user - {Username}", storedToken.User.Username);
        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresIn = 3600,
            IssuedAt = DateTime.UtcNow
        };
    }

    public async Task LogoutAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var tokenHash = HashToken(refreshToken);
        var storedToken = _authRepository.GetRefreshTokenByHash(tokenHash);

        if (storedToken != null)
        {
            _authRepository.RevokeRefreshToken(storedToken.Id);
            _logger.LogInformation("User logged out - TokenId: {TokenId}", storedToken.Id);
        }
    }

    public async Task<string> HashPasswordAsync(string password)
    {
        return await Task.FromResult(BCrypt.Net.BCrypt.HashPassword(password));
    }

    public async Task<bool> VerifyPasswordAsync(string password, string hash)
    {
        return await Task.FromResult(BCrypt.Net.BCrypt.Verify(password, hash));
    }

    private async Task<string> GenerateJwtTokenAsync(User user)
    {
        var secretKey = _configuration["Jwt:Secret"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var activeRole = user.Roles?.FirstOrDefault()?.Name ?? "Guest";

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new Claim("role", activeRole),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"] ?? "60")),
            signingCredentials: credentials
        );

        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    private async Task<string> GenerateRefreshTokenAsync(User user)
    {
        var token = Guid.NewGuid().ToString();
        var tokenHash = HashToken(token);

        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = tokenHash,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            IsRevoked = false
        };

        _authRepository.CreateRefreshToken(refreshTokenEntity);
        return await Task.FromResult(token);
    }

    private string HashToken(string token)
    {
        return BCrypt.Net.BCrypt.HashPassword(token);
    }
}