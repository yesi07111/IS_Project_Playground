using FastEndpoints;
using Org.BouncyCastle.Ocsp;
using Playground.Application.Commands.Facility.Delete;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Facility;

public class DeleteFacilityEndpoint : Endpoint<DeleteFacilityCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Delete("/facility/delete");
    }

    public override async Task<GenericResponse> ExecuteAsync(DeleteFacilityCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}