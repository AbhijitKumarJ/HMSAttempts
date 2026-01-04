using HMS.Business.Auth;
using HMS.Entity.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HMS.Tests.Auth;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly AuthController _controller;
    private readonly DefaultHttpContext _httpContext;

    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _controller = new AuthController(
            Mock.Of<Microsoft.Extensions.Logging.ILogger<AuthController>>(),
            _mockAuthService.Object
        );
        _httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = _httpContext
        };
    }

    [Fact]
    public async Task Login_ValidCredentials_Returns200AndSetsCookie()
    {
        var request = new LoginRequest { Username = "testuser", Password = "password" };
        var expectedResponse = new TokenResponse
        {
            AccessToken = "test-jwt-token",
            RefreshToken = "refresh-token-value",
            ExpiresIn = 3600,
            IssuedAt = DateTime.UtcNow
        };

        _mockAuthService.Setup(s => s.LoginAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.Login(request, CancellationToken.None) as OkObjectResult;

        Assert.NotNull(result);
        var response = result.Value as dynamic;
        Assert.NotNull(response);
        Assert.Equal(expectedResponse.AccessToken, response.accessToken);
        Assert.Equal(expectedResponse.ExpiresIn, response.expiresIn);

        var refreshTokenCookie = _httpContext.Response.Cookies.FirstOrDefault(c => c.Key == "refreshToken");
        Assert.NotNull(refreshTokenCookie);
        Assert.Equal(expectedResponse.RefreshToken, refreshTokenCookie.Value.Value);
    }

    [Fact]
    public async Task Login_InvalidCredentials_Returns401()
    {
        var request = new LoginRequest { Username = "testuser", Password = "wrongpassword" };

        _mockAuthService.Setup(s => s.LoginAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TokenResponse?)null);

        var result = await _controller.Login(request, CancellationToken.None) as UnauthorizedObjectResult;

        Assert.NotNull(result);
        Assert.Equal(401, result.StatusCode);
    }

    [Fact]
    public async Task_Login_MissingFields_Returns400()
    {
        var request = new LoginRequest { Username = "", Password = "" };

        var result = await _controller.Login(request, CancellationToken.None) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public async Task Refresh_ValidCookie_Returns200()
    {
        _httpContext.Request.Headers.Add("Cookie", "refreshToken=valid-token");
        var expectedResponse = new TokenResponse
        {
            AccessToken = "new-jwt-token",
            RefreshToken = "new-refresh-token",
            ExpiresIn = 3600,
            IssuedAt = DateTime.UtcNow
        };

        _mockAuthService.Setup(s => s.RefreshTokenAsync("valid-token", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.Refresh(CancellationToken.None) as OkObjectResult;

        Assert.NotNull(result);
        var response = result.Value as dynamic;
        Assert.NotNull(response);
        Assert.Equal(expectedResponse.AccessToken, response.accessToken);
    }

    [Fact]
    public async Task Refresh_NoCookie_Returns401()
    {
        _httpContext.Request.Headers.Clear();

        var result = await _controller.Refresh(CancellationToken.None) as UnauthorizedObjectResult;

        Assert.NotNull(result);
        Assert.Equal(401, result.StatusCode);
    }

    [Fact]
    public async Task Refresh_InvalidToken_Returns401AndClearsCookie()
    {
        _httpContext.Request.Headers.Add("Cookie", "refreshToken=invalid-token");
        _httpContext.Response.Cookies.Append("refreshToken", "old-token");

        _mockAuthService.Setup(s => s.RefreshTokenAsync("invalid-token", It.IsAny<CancellationToken>()))
            .ReturnsAsync((TokenResponse?)null);

        var result = await _controller.Refresh(CancellationToken.None) as UnauthorizedObjectResult;

        Assert.NotNull(result);
        Assert.Equal(401, result.StatusCode);

        var refreshCookie = _httpContext.Response.Cookies.FirstOrDefault(c => c.Key == "refreshToken");
        Assert.Null(refreshCookie);
    }

    [Fact]
    public async Task Logout_ValidCookie_ClearsCookie()
    {
        _httpContext.Request.Headers.Add("Cookie", "refreshToken=valid-token");
        _httpContext.Response.Cookies.Append("refreshToken", "old-token");

        _mockAuthService.Setup(s => s.LogoutAsync("valid-token", It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _controller.Logout(CancellationToken.None) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var refreshCookie = _httpContext.Response.Cookies.FirstOrDefault(c => c.Key == "refreshToken");
        Assert.Null(refreshCookie);

        _mockAuthService.Verify(s => s.LogoutAsync("valid-token", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Logout_NoCookie_Returns200()
    {
        _httpContext.Request.Headers.Clear();

        _mockAuthService.Setup(s => s.LogoutAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _controller.Logout(CancellationToken.None) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }
}
