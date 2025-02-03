using FastEndpoints;
using Playground.Application.Queries.Reservation.List;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Reservation;

/// <summary>
/// Endpoint para obtener una lista de reservas.
/// </summary>
public class ListReservationEndpoint : Endpoint<ListReservationQuery, ListReservationResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de obtención de la lista de reservas.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/reservation/get-all");
    }

    /// <summary>
    /// Ejecuta la consulta para obtener la lista de reservas procesando el query recibido.
    /// </summary>
    /// <param name="req">El query que contiene los criterios para listar las reservas.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta con la lista de reservas.</returns>
    public override async Task<ListReservationResponse> ExecuteAsync(ListReservationQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
