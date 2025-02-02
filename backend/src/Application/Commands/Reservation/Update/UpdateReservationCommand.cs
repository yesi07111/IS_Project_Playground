using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.Reservation.Update;

public class UpdateReservationCommand : ICommand<GenericResponse>
{
    public string ReservationId { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
}