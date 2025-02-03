using FastEndpoints;
using Playground.Application.Commands.Reservation.Update;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Reservation;

/// <summary>
/// Endpoint para actualizar una reserva.
/// </summary>
public class UpdateReservationEndPoint : Endpoint<UpdateReservationCommand, GenericResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de actualización de reservas.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Put("/reservation/update");
    }

    /// <summary>
    /// Ejecuta la actualización de una reserva procesando el comando recibido.
    /// </summary>
    /// <param name="req">El comando que contiene los datos de la reserva a actualizar.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta genérica con el resultado de la operación.</returns>
    public override async Task<GenericResponse> ExecuteAsync(UpdateReservationCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
