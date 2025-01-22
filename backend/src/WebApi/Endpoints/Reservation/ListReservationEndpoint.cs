using FastEndpoints;
using Playground.Application.Queries.Reservation.List;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Reservation;

public class ListReservationEndpoint : Endpoint<ListReservationQuery, ListReservationResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/reservation/get-all");
    }

    public override async Task<ListReservationResponse> ExecuteAsync(ListReservationQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}