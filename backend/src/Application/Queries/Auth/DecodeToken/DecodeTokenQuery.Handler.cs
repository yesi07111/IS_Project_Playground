using System.IdentityModel.Tokens.Jwt;
using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Queries.Auth.DecodeToken;

public class DecodeTokenQueryHandler : CommandHandler<DecodeTokenQuery, DecodedTokenResponse>
{
    public override async Task<DecodedTokenResponse> ExecuteAsync(DecodeTokenQuery query, CancellationToken ct = default)
    {
        var handler = new JwtSecurityTokenHandler();

        if (handler.ReadToken(query.Token) is not JwtSecurityToken jsonToken)
        {
            throw new ArgumentException("Token Inválido.");
        }

        // Extraer los claims del token
        var claims = jsonToken.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);

        // Crear una respuesta con los claims decodificados
        var decodedToken = new DecodedTokenResponse(query.Token, claims);
        return await Task.FromResult(decodedToken);
    }
}