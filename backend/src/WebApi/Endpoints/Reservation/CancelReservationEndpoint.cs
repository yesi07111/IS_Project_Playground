using FastEndpoints;
using Playground.Application.Commands.Reservation.Cancel;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Reservation;

/// <summary>
/// Endpoint para cancelar una reserva.
/// </summary>
public class CancelReservationEndpoint : Endpoint<CancelReservationCommand, UserActionResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de cancelación de reserva.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Put("/reservation/cancel");
    }

    /// <summary>
    /// Ejecuta la cancelación de una reserva procesando el comando recibido.
    /// </summary>
    /// <param name="req">El comando que contiene los datos necesarios para cancelar la reserva.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta que contiene el resultado de la cancelación de la reserva.</returns>
    public override async Task<UserActionResponse> ExecuteAsync(CancelReservationCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
