using FastEndpoints;
using Playground.Application.Commands.Reservation.Create;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Reservation;

/// <summary>
/// Endpoint para reservar una actividad.
/// </summary>
public class ReserveActivityEndpoint : Endpoint<ReserveActivityCommand, ReservationCreationResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de reserva de actividad.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Post("/reserve/activity");
    }

    /// <summary>
    /// Ejecuta la reserva de una actividad procesando el comando recibido.
    /// </summary>
    /// <param name="req">El comando que contiene los datos necesarios para realizar la reserva de la actividad.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta que contiene el resultado de la creación de la reserva.</returns>
    public override async Task<ReservationCreationResponse> ExecuteAsync(ReserveActivityCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
