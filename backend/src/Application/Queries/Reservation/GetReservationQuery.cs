using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Queries.Reservation.Get;

public record GetReservationQuery : ICommand<GetReservationResponse>
{
    public string Id { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
}