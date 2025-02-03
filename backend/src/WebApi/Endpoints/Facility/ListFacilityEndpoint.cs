using FastEndpoints;
using Playground.Application.Queries.Facility.List;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Facility;

/// <summary>
/// Endpoint para listar todas las instalaciones.
/// </summary>
public class ListFacilityEndpoint : Endpoint<ListFacilityQuery, ListFacilityResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta para obtener la lista de instalaciones.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/facility/get-all");
    }

    /// <summary>
    /// Ejecuta la consulta para obtener la lista de todas las instalaciones.
    /// </summary>
    /// <param name="req">La consulta que contiene los parámetros necesarios para obtener la lista de instalaciones.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta que contiene la lista de todas las instalaciones.</returns>
    public override async Task<ListFacilityResponse> ExecuteAsync(ListFacilityQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
