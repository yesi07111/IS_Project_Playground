using FastEndpoints;
using Playground.Application.Queries.HomePage;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.HomePage;

/// <summary>
/// Endpoint para obtener la información de la página de inicio.
/// </summary>
public class GetHomePageInfoEndpoint(GetHomePageInfoQueryHandler queryHandler) : EndpointWithoutRequest<GetHomePageInfoResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de obtención de la información de la página de inicio.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/get/homepage");
    }

    /// <summary>
    /// Maneja la solicitud para obtener la información de la página de inicio ejecutando el query handler.
    /// </summary>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await queryHandler.ExecuteAsync(ct);
        await SendAsync(response, cancellation: ct);
    }
}
