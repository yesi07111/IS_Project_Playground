using FastEndpoints;
using Playground.Application.Responses;
using Playground.Application.Queries.Auth.GetGoogleClientId;

namespace Playground.WebApi.Endpoints.Auth;

/// <summary>
/// Endpoint para obtener el ID de cliente de Google para autenticación.
/// </summary>
public class GetGoogleClientIdEndpoint : EndpointWithoutRequest<GoogleClientIdResponse>
{
    private readonly GetGoogleClientIdQueryHandler _queryHandler;

    /// <summary>
    /// Inicializa una nueva instancia del endpoint para obtener el ID de cliente de Google.
    /// </summary>
    /// <param name="queryHandler">Manejador de consulta que maneja la solicitud de obtener el ID de cliente de Google.</param>
    public GetGoogleClientIdEndpoint(GetGoogleClientIdQueryHandler queryHandler)
    {
        _queryHandler = queryHandler;
    }

    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y define la ruta para obtener el ID de cliente de Google.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/auth/get-google-client-id");
    }

    /// <summary>
    /// Maneja la solicitud para obtener el ID de cliente de Google y envía la respuesta.
    /// </summary>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta con el ID de cliente de Google.</returns>
    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await _queryHandler.ExecuteAsync(ct);
        await SendAsync(response, cancellation: ct);
    }
}
