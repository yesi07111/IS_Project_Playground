using System.Security.Claims;
using Playground.Application.Services;
using Playground.Domain.Entities.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Playground.Infraestructure.Services;

/// <summary>
/// Clase que representa la sesión activa de un usuario en la aplicación.
/// Proporciona métodos para obtener información sobre el usuario actual y su contexto de sesión.
/// </summary>
public sealed class ActiveSession : IActiveSession
{
    private readonly ClaimsPrincipal? _userContext;
    private readonly UserManager<User> _userManager;
    private string? _currentUserId;
    private string? _baseUrl;
    private User? _user;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ActiveSession"/>.
    /// </summary>
    /// <param name="accessor">Accesor para obtener el contexto HTTP actual.</param>
    /// <param name="_userManager">Administrador de usuarios para gestionar la identidad de usuario.</param>
    public ActiveSession(IHttpContextAccessor accessor, UserManager<User> _userManager)
    {
        _userContext = accessor?.HttpContext?.User;
        var request = accessor?.HttpContext?.Request;
        _baseUrl = $"{request?.Scheme}://{request?.Host}{request?.PathBase}";

        this._userManager = _userManager;
    }

    /// <summary>
    /// Obtiene la URL base de la aplicación.
    /// </summary>
    /// <returns>La URL base como una cadena de texto.</returns>
    public string BaseUrl() => _baseUrl ?? string.Empty;

    /// <summary>
    /// Obtiene el usuario activo actual.
    /// </summary>
    /// <returns>Una tarea que representa la operación asincrónica, con el usuario activo como resultado.</returns>
    public async Task<User?> GetActiveUser()
    {
        if (_user != null) return _user;

        return _user = await _userManager.FindByIdAsync(UserId() ?? string.Empty);
    }

    /// <summary>
    /// Obtiene el principal de reclamaciones del usuario actual.
    /// </summary>
    /// <returns>El principal de reclamaciones del usuario actual.</returns>
    public ClaimsPrincipal? GetClaimPrincipal() => _userContext;

    /// <summary>
    /// Obtiene el identificador del usuario actual.
    /// </summary>
    /// <returns>El identificador del usuario como una cadena de texto.</returns>
    public string? UserId() => _currentUserId ??= _userContext?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}