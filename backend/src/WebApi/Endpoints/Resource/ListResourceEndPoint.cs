using FastEndpoints;
using Playground.Application.Queries.Resource.List;
using Playground.Application.Queries.Responses;

namespace Playground.WebApi.Endpoints.Resource;

/// <summary>
/// Endpoint para obtener una lista de recursos.
/// </summary>
public class ListResourceEndPoint : Endpoint<ListResourceQuery, ListResourceResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de obtención de la lista de recursos.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/resource/get-all");
    }

    /// <summary>
    /// Ejecuta la consulta para obtener la lista de recursos procesando el query recibido.
    /// </summary>
    /// <param name="req">El query que contiene los criterios para listar los recursos.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta con la lista de recursos.</returns>
    public override async Task<ListResourceResponse> ExecuteAsync(ListResourceQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
