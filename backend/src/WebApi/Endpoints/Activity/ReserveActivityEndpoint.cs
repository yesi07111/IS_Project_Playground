using FastEndpoints;
using Playground.Application.Commands.Activity.Reservation;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Activity;

public class ReserveActivityEndpoint : Endpoint<ReserveActivityCommand, ReservationCreationResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/activity/reserve");
    }

    public override async Task<ReservationCreationResponse> ExecuteAsync(ReserveActivityCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}