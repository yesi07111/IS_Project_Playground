using FastEndpoints;
using Playground.Application.Commands.Reservation.Cancel;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Reservation;

public class CancelReservationEndpoint : Endpoint<CancelReservationCommand, UserActionResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Put("/reservation/cancel");
    }

    public override async Task<UserActionResponse> ExecuteAsync(CancelReservationCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}