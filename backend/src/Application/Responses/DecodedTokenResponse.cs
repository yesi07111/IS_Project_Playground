namespace Playground.Application.Responses;

public record DecodedTokenResponse(string Token, IDictionary<string, string> Claims);
