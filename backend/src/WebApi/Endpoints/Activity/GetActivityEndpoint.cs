using FastEndpoints;
using Playground.Application.Queries.Activity.Get;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Activity;

/// <summary>
/// Endpoint para obtener una actividad específica.
/// </summary>
public class GetActivityEndpoint : Endpoint<GetActivityQuery, GetActivityResponse>
{
    /// <summary>
    /// Configura el endpoint para obtener una actividad.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/activity/get");
    }

    /// <summary>
    /// Maneja la solicitud para obtener una actividad específica.
    /// </summary>
    /// <param name="req">Consulta que contiene los datos necesarios para obtener la actividad.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta que contiene los detalles de la actividad solicitada.</returns>
    public override async Task<GetActivityResponse> ExecuteAsync(GetActivityQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
