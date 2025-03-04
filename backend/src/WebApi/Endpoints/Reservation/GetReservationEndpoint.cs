using FastEndpoints;
using Playground.Application.Queries.Reservation.Get;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Reservation;

/// <summary>
/// Endpoint para obtener una Geta de reservas.
/// </summary>
public class GetReservationEndpoint : Endpoint<GetReservationQuery, GetReservationResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de obtención de la reserva.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/reservation/get");
    }

    /// <summary>
    /// Ejecuta la consulta para obtener la reserva procesando el query recibido.
    /// </summary>
    /// <param name="req">El query que contiene los criterios para obtener la reserva.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta con la reserva.</returns>
    public override async Task<GetReservationResponse> ExecuteAsync(GetReservationQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
