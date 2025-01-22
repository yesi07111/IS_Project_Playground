namespace Playground.Application.Responses;

public record GoogleTokenValidationResponse(bool IsValid, IDictionary<string, string>? Claims);