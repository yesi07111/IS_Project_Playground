using System.IdentityModel.Tokens.Jwt;
using FastEndpoints;
using Playground.Application.Responses;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace Playground.Application.Queries.Auth.CheckGoogleToken;

public class CheckGoogleTokenQueryHandler : CommandHandler<CheckGoogleTokenQuery, GoogleTokenValidationResponse>
{
    private readonly IConfiguration _configuration;

    public CheckGoogleTokenQueryHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public override async Task<GoogleTokenValidationResponse> ExecuteAsync(CheckGoogleTokenQuery command, CancellationToken ct = default)
    {
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(command.Token))
        {
            throw new ArgumentException("Token Inválido.");
        }

        var jsonToken = handler.ReadJwtToken(command.Token);

        // Obtener las claves públicas de Google
        var httpClient = new HttpClient();
        var googleCertsUrl = _configuration["Google:CertsUrl"];
        var response = await httpClient.GetStringAsync(googleCertsUrl);
        var keysToken = JObject.Parse(response)["keys"];

        if (keysToken == null)
        {
            throw new InvalidOperationException("No se pudieron obtener las claves públicas de Google.");
        }

        var keys = keysToken.Select(key => new JsonWebKey(key.ToString()));

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _configuration["Google:Issuer"],
            ValidateAudience = true,
            ValidAudience = _configuration["Google:ClientId"],
            ValidateLifetime = true,
            IssuerSigningKeys = keys
        };

        try
        {
            handler.ValidateToken(command.Token, validationParameters, out SecurityToken validatedToken);
            var jwtToken = validatedToken as JwtSecurityToken;
            var claims = jwtToken?.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
            return new GoogleTokenValidationResponse(true, claims);
        }
        catch (Exception)
        {
            return new GoogleTokenValidationResponse(false, null);
        }
    }
}