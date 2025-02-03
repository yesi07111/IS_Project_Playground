using FastEndpoints;
using Playground.Application.Queries.Activity.List;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Activity;

/// <summary>
/// Endpoint para obtener una lista de actividades.
/// </summary>
public class ListActivityEndpoint : Endpoint<ListActivityQuery, ListActivityResponse>
{
    /// <summary>
    /// Configura el endpoint para obtener todas las actividades.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/activity/get-all");
    }

    /// <summary>
    /// Maneja la solicitud para obtener todas las actividades.
    /// </summary>
    /// <param name="req">Consulta que contiene los datos necesarios para obtener la lista de actividades.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta que contiene la lista de actividades solicitadas.</returns>
    public override async Task<ListActivityResponse> ExecuteAsync(ListActivityQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
