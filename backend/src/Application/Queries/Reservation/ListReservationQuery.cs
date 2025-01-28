using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Queries.Reservation.List;

public record ListReservationQuery : ICommand<ListReservationResponse>
{
    public string Id { get; init; } = string.Empty;
}