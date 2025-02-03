using FastEndpoints;
using Playground.Application.Queries.Auth.CheckGoogleToken;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Auth;

/// <summary>
/// Endpoint para verificar la validez de un token de Google.
/// </summary>
public class CheckGoogleTokenEndpoint : Endpoint<CheckGoogleTokenQuery, GoogleTokenValidationResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta para verificar el token de Google.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Post("/auth/check-google-token");
    }

    /// <summary>
    /// Ejecuta la validación del token de Google proporcionado en la consulta.
    /// </summary>
    /// <param name="req">El comando que contiene el token de Google a validar.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta que indica si el token de Google es válido o no.</returns>
    public override async Task<GoogleTokenValidationResponse> ExecuteAsync(CheckGoogleTokenQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
