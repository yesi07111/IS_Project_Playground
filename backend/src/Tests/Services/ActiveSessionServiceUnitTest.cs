using AutoFixture;
using Moq;
using Playground.Domain.Entities.Auth;
using Playground.Infraestructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Playground.Tests.Services;

public class ActiveSessionServiceUnitTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly ClaimsPrincipal _userPrincipal;

    public ActiveSessionServiceUnitTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);

        _userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user-id")
        }, "mock"));
    }

    [Fact]
    public void ActiveSession_WithValidHttpContext_ReturnsBaseUrl()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("localhost");
        httpContext.Request.PathBase = "/api";
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext!);

        var activeSession = new ActiveSession(_httpContextAccessorMock.Object, _userManagerMock.Object);

        // Act
        var baseUrl = activeSession.BaseUrl();

        // Assert
        Assert.Equal("https://localhost/api", baseUrl);
    }

    [Fact]
    public async Task ActiveSession_WithValidUser_ReturnsActiveUser()
    {
        // Arrange
        var user = _fixture.Build<User>()
            .With(u => u.Id, "test-user-id")
            .Create();

        _httpContextAccessorMock.Setup(x => x.HttpContext!.User).Returns(_userPrincipal);
        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

        var activeSession = new ActiveSession(_httpContextAccessorMock.Object, _userManagerMock.Object);

        // Act
        var activeUser = await activeSession.GetActiveUser();

        // Assert
        Assert.NotNull(activeUser);
        Assert.Equal("test-user-id", activeUser.Id);
    }

    [Fact]
    public void ActiveSession_WithValidUser_ReturnsClaimPrincipal()
    {
        // Arrange
        _httpContextAccessorMock.Setup(x => x.HttpContext!.User).Returns(_userPrincipal);

        var activeSession = new ActiveSession(_httpContextAccessorMock.Object, _userManagerMock.Object);

        // Act
        var claimPrincipal = activeSession.GetClaimPrincipal();

        // Assert
        Assert.NotNull(claimPrincipal);
        Assert.Equal(_userPrincipal, claimPrincipal);
    }

    [Fact]
    public void ActiveSession_WithValidUser_ReturnsUserId()
    {
        // Arrange
        _httpContextAccessorMock.Setup(x => x.HttpContext!.User).Returns(_userPrincipal);

        var activeSession = new ActiveSession(_httpContextAccessorMock.Object, _userManagerMock.Object);

        // Act
        var userId = activeSession.UserId();

        // Assert
        Assert.Equal("test-user-id", userId);
    }

    [Fact]
    public void ActiveSession_WithNullHttpContext_ReturnsEmptyBaseUrl()
    {
        // Arrange
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext?)null);

        var activeSession = new ActiveSession(_httpContextAccessorMock.Object, _userManagerMock.Object);

        // Act
        var baseUrl = activeSession.BaseUrl();

        // Assert
        Assert.Equal("://", baseUrl);
    }

    [Fact]
    public async Task ActiveSession_WithNullUser_ReturnsNullActiveUser()
    {
        // Arrange
        _httpContextAccessorMock.Setup(x => x.HttpContext!.User).Returns(_userPrincipal);
        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

        var activeSession = new ActiveSession(_httpContextAccessorMock.Object, _userManagerMock.Object);

        // Act
        var activeUser = await activeSession.GetActiveUser();

        // Assert
        Assert.Null(activeUser);
    }

    [Fact]
    public void ActiveSession_WithNullUserContext_ReturnsNullClaimPrincipal()
    {
        // Arrange
        _httpContextAccessorMock.Setup(x => x.HttpContext!.User).Returns((ClaimsPrincipal?)null!);

        var activeSession = new ActiveSession(_httpContextAccessorMock.Object, _userManagerMock.Object);

        // Act
        var claimPrincipal = activeSession.GetClaimPrincipal();

        // Assert
        Assert.Null(claimPrincipal);
    }

    [Fact]
    public void ActiveSession_WithNullUserContext_ReturnsNullUserId()
    {
        // Arrange
        _httpContextAccessorMock.Setup(x => x.HttpContext!.User).Returns((ClaimsPrincipal?)null!);

        var activeSession = new ActiveSession(_httpContextAccessorMock.Object, _userManagerMock.Object);

        // Act
        var userId = activeSession.UserId();

        // Assert
        Assert.Null(userId);
    }
}
