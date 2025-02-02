using FastEndpoints;
using Playground.Application.Commands.Reservation.Update;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Reservation;

public class UpdateReservationEndPoint : Endpoint<UpdateReservationCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Put("/reservation/update");
    }

    public override async Task<GenericResponse> ExecuteAsync(UpdateReservationCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}