using Playground.Domain.Entities.Auth;

namespace Playground.Application.Services;

/// <summary>
/// Interfaz para un generador de tokens JWT que proporciona métodos para generar tokens para usuarios.
/// </summary>
public interface IJwtGenerator
{
    /// <summary>
    /// Genera un token JWT para un usuario específico.
    /// </summary>
    /// <param name="user">El usuario para el que se genera el token.</param>
    /// <returns>Una tarea que representa la operación asincrónica, con el token JWT como resultado.</returns>
    string GetToken(User user);
}