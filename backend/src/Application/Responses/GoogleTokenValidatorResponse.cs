namespace Playground.Application.Responses;

/// <summary>
/// Representa la respuesta de la validación de un token de Google.
/// </summary>
public record GoogleTokenValidationResponse(bool IsValid, IDictionary<string, string>? Claims);