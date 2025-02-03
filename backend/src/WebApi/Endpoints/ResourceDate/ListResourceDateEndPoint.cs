using FastEndpoints;
using Playground.Application.Queries.ResourceDate;
using Playground.Application.Queries.Responses;

namespace Playground.WebApi.Endpoints.ResourceDate;

/// <summary>
/// Endpoint para obtener una lista de fechas de recursos.
/// </summary>
public class ListResourceDateEndPoint : Endpoint<ListResourceDateQuery, ListResourceDateResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de obtención de la lista de fechas de recursos.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/resourceDate/get-all");
    }

    /// <summary>
    /// Ejecuta la consulta para obtener la lista de fechas de recursos procesando el query recibido.
    /// </summary>
    /// <param name="req">El query que contiene los criterios para listar las fechas de recursos.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta con la lista de fechas de recursos.</returns>
    public override async Task<ListResourceDateResponse> ExecuteAsync(ListResourceDateQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
