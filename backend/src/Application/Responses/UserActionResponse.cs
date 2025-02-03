namespace Playground.Application.Responses;

/// <summary>
/// Representa la respuesta a una acción del usuario en el sistema, incluyendo información de autenticación y autorización.
/// </summary>
public record UserActionResponse(Guid Id, string Username, string Token, string RolName);