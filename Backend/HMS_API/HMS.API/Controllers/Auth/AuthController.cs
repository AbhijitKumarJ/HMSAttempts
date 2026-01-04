using HMS.Business.Auth;
using HMS.Entity.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.API.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;

    public AuthController(ILogger<AuthController> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new { error = "Username and password are required" });
        }

        var result = await _authService.LoginAsync(request, cancellationToken);
        if (result == null)
        {
            return Unauthorized(new { error = "Invalid username or password" });
        }

        Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            Path = "/"
        });

        return Ok(new
        {
            accessToken = result.AccessToken,
            expiresIn = result.ExpiresIn,
            issuedAt = result.IssuedAt
        });
    }

    // POST: api/auth/refresh
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new { error = "Refresh token not found" });
        }

        var result = await _authService.RefreshTokenAsync(refreshToken, cancellationToken);
        if (result == null)
        {
            Response.Cookies.Delete("refreshToken");
            return Unauthorized(new { error = "Invalid or expired refresh token" });
        }

        Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            Path = "/"
        });

        return Ok(new
        {
            accessToken = result.AccessToken,
            expiresIn = result.ExpiresIn,
            issuedAt = result.IssuedAt
        });
    }

    // POST: api/auth/logout
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (!string.IsNullOrEmpty(refreshToken))
        {
            await _authService.LogoutAsync(refreshToken, cancellationToken);
        }

        Response.Cookies.Delete("refreshToken");
        return Ok(new { message = "Logged out successfully" });
    }

    // Legacy endpoints (kept for backward compatibility)

    // // GET: api/auth/users
    // [HttpGet("users")]
    // public async Task<IActionResult> GetUsers([FromQuery]int limit,CancellationToken cancellationToken)
    // {
    //     return Ok(_authService.GetUsers(limit));
    // }

    // // GET: api/auth/users/5
    // [HttpGet("users/{id:int}")]
    // public async Task<IActionResult> GetUser(int id, CancellationToken cancellationToken)
    // {
    //     return Ok(_authService.GetUser(id));
    // }

    // // POST: api/auth/users
    // [HttpPost("users")]
    // public async Task<IActionResult> CreateUser([FromBody] CreateUserEntity entity, CancellationToken cancellationToken)
    // {
    //     if (entity == null) return BadRequest();

    //     return CreatedAtAction(nameof(GetUser), new { id = 1 }, _authService.CreateUser(entity));
    // }

    // // POST: api/auth/users/update
    // [HttpPost("users/update")]
    // public async Task<IActionResult> UpdateUser([FromBody] UpdateUserEntity entity, CancellationToken cancellationToken)
    // {
    //     return Ok(_authService.UpdateUser(entity));
    // }

    // // GET: api/auth/users/delete/5
    // [HttpGet("users/delete/{id:int}")]
    // public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
    // {
    //     return Ok(_authService.DeleteUser(id));
    // }
}
