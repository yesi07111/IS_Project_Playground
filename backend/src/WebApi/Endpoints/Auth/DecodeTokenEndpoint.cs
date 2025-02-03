using FastEndpoints;
using Playground.Application.Queries.Auth.DecodeToken;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Auth;

/// <summary>
/// Endpoint para decodificar un token de autenticación.
/// </summary>
public class DecodeTokenEndpoint : Endpoint<DecodeTokenQuery, DecodedTokenResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta para decodificar el token.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/auth/decode-token");
    }

    /// <summary>
    /// Ejecuta la decodificación del token de autenticación.
    /// </summary>
    /// <param name="req">El objeto de consulta que contiene el token a decodificar.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta con la información decodificada del token.</returns>
    public override async Task<DecodedTokenResponse> ExecuteAsync(DecodeTokenQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
