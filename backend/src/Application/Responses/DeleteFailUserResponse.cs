namespace Playground.Application.Responses;

/// <summary>
/// Representa la respuesta de una operación de eliminación de usuario fallida.
/// </summary>
public record DeleteFailUserResponse(bool Success, string Message);