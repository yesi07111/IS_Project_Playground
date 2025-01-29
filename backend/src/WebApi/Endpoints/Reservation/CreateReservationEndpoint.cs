using FastEndpoints;
using Playground.Application.Commands.Reservation.Create;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Reservation;

public class ReserveActivityEndpoint : Endpoint<ReserveActivityCommand, ReservationCreationResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/reserve/activity");
    }

    public override async Task<ReservationCreationResponse> ExecuteAsync(ReserveActivityCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}