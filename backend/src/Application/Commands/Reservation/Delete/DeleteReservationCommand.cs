using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.Reservation.Delete;

public record DeleteReservationCommand : ICommand<GenericResponse>
{
    public string Id { get; init; } = string.Empty;
}