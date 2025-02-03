namespace Playground.Application.Responses;

/// <summary>
/// Representa la respuesta exitosa de la creación de un nuevo usuario en el sistema.
/// </summary>
public record UserCreationResponse(Guid Id, string Username);