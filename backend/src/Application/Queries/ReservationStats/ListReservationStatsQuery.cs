using FastEndpoints;
using Playground.Application.Queries.Responses;

namespace Playground.Application.Queries.ReservationStats.List;

public record ListReservationStatsQuery : ICommand<ListReservationStatsResponse>
{
    public string UseCase { get; init; } = string.Empty;
}