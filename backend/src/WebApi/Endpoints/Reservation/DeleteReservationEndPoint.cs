using FastEndpoints;
using Playground.Application.Commands.Reservation.Delete;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Reservation;

public class DeleteReservationEndPoint : Endpoint<DeleteReservationCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Delete("/reservation/delete");
    }

    public override async Task<GenericResponse> ExecuteAsync(DeleteReservationCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}