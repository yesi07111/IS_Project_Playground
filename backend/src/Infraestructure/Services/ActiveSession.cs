using System.Security.Claims;
using Playground.Application.Services;
using Playground.Domain.Entities.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Playground.Infraestructure.Services;

public sealed class ActiveSession : IActiveSession
{
    private readonly ClaimsPrincipal? _userContext;
    private readonly UserManager<User> _userManager;
    private string? _currentUserId;
    private string? _baseUrl;
    private User? _user;

    public ActiveSession(IHttpContextAccessor accessor, UserManager<User> _userManager)
    {
        _userContext = accessor?.HttpContext?.User;
        var request = accessor?.HttpContext?.Request;
        _baseUrl = $"{request?.Scheme}://{request?.Host}{request?.PathBase}";

        this._userManager = _userManager;
    }

    public string BaseUrl() => _baseUrl ?? String.Empty;

    public async Task<User?> GetActiveUser()
    {
        if (_user != null) return _user;

        return _user = await _userManager.FindByIdAsync(UserId() ?? string.Empty);
    }

    public ClaimsPrincipal? GetClaimPrincipal() => _userContext;

    public string? UserId() => _currentUserId ??= _userContext?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}