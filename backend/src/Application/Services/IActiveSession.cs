using System.Security.Claims;
using Playground.Domain.Entities.Auth;

namespace Playground.Application.Services;

/// <summary>
/// Interfaz para gestionar la sesión activa de un usuario.
/// Proporciona métodos para obtener información sobre el usuario activo y su contexto.
/// </summary>
public interface IActiveSession
{
    /// <summary>
    /// Obtiene el identificador del usuario activo.
    /// </summary>
    /// <returns>El identificador del usuario activo o null si no hay usuario activo.</returns>
    string? UserId();

    /// <summary>
    /// Obtiene el usuario activo actual.
    /// </summary>
    /// <returns>Una tarea que representa la operación asincrónica, con el usuario activo como resultado.</returns>
    Task<User?> GetActiveUser();

    /// <summary>
    /// Obtiene la URL base de la aplicación.
    /// </summary>
    /// <returns>La URL base de la aplicación.</returns>
    string BaseUrl();

    /// <summary>
    /// Obtiene el principal de reclamaciones del usuario activo.
    /// </summary>
    /// <returns>El principal de reclamaciones del usuario activo o null si no hay usuario activo.</returns>
    ClaimsPrincipal? GetClaimPrincipal();
}