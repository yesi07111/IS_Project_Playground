namespace Playground.Application.Responses;

/// <summary>
/// Representa la respuesta para una lista de usuarios.
/// </summary>
public record ListUserResponse(IEnumerable<object> Users);