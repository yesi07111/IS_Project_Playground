using FastEndpoints;
using Playground.Application.Queries.ReservationStats.List;
using Playground.Application.Queries.Responses;

namespace Playground.WebApi.Endpoints.Reservation;

public class ListReservationStatsEndpoint : Endpoint<ListReservationStatsQuery, ListReservationStatsResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/reservation/get-stats");
    }

    public override async Task<ListReservationStatsResponse> ExecuteAsync(ListReservationStatsQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}