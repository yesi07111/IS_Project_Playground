namespace Playground.Application.Responses;

/// <summary>
/// Representa la respuesta de un token decodificado.
/// </summary>
public record DecodedTokenResponse(string Token, IDictionary<string, string> Claims);
