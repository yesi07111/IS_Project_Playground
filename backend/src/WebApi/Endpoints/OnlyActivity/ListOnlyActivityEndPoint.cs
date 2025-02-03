using FastEndpoints;
using OneOf.Types;
using Playground.Application.Queries.OnlyActivity;
using Playground.Application.Queries.Responses;

namespace Playground.WebApi.Endpoints.OnlyActivity;

/// <summary>
/// Endpoint para obtener una lista de actividades exclusivas.
/// </summary>
public class ListOnlyActivityEndPoint : Endpoint<ListOnlyActivityQuery, ListOnlyActivityResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de obtención de actividades exclusivas.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/onlyActivity/get-all");
    }

    /// <summary>
    /// Ejecuta la consulta para obtener una lista de actividades exclusivas procesando el comando recibido.
    /// </summary>
    /// <param name="req">El comando que contiene los datos necesarios para obtener la lista de actividades exclusivas.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta que contiene la lista de actividades exclusivas.</returns>
    public override async Task<ListOnlyActivityResponse> ExecuteAsync(ListOnlyActivityQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
