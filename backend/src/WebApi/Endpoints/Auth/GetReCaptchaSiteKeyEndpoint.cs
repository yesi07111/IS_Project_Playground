using FastEndpoints;
using Playground.Application.Responses;
using Playground.Application.Queries.Auth.GetRecaptchaSiteKey;

namespace Playground.WebApi.Endpoints;

/// <summary>
/// Endpoint para obtener la clave del sitio de reCAPTCHA para la autenticación.
/// </summary>
public class GetRecaptchaSiteKeyEndpoint : EndpointWithoutRequest<ReCaptchaSiteKeyResponse>
{
    private readonly GetRecaptchaSiteKeyQueryHandler _queryHandler;

    /// <summary>
    /// Inicializa una nueva instancia del endpoint para obtener la clave del sitio de reCAPTCHA.
    /// </summary>
    /// <param name="queryHandler">Manejador de consulta que maneja la solicitud para obtener la clave del sitio de reCAPTCHA.</param>
    public GetRecaptchaSiteKeyEndpoint(GetRecaptchaSiteKeyQueryHandler queryHandler)
    {
        _queryHandler = queryHandler;
    }

    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y define la ruta para obtener la clave del sitio de reCAPTCHA.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/auth/get-captcha-site-key");
    }

    /// <summary>
    /// Maneja la solicitud para obtener la clave del sitio de reCAPTCHA y envía la respuesta.
    /// </summary>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta con la clave del sitio de reCAPTCHA.</returns>
    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await _queryHandler.ExecuteAsync(ct);
        await SendAsync(response, cancellation: ct);
    }
}
