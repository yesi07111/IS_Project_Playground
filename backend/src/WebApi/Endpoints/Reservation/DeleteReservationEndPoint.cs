using FastEndpoints;
using Playground.Application.Commands.Reservation.Delete;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Reservation;

/// <summary>
/// Endpoint para eliminar una reserva.
/// </summary>
public class DeleteReservationEndPoint : Endpoint<DeleteReservationCommand, GenericResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de eliminación de reservas.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Delete("/reservation/delete");
    }

    /// <summary>
    /// Ejecuta la eliminación de una reserva procesando el comando recibido.
    /// </summary>
    /// <param name="req">El comando que contiene los datos de la reserva a eliminar.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta genérica con el resultado de la operación.</returns>
    public override async Task<GenericResponse> ExecuteAsync(DeleteReservationCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
