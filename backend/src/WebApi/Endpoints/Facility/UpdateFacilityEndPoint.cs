using FastEndpoints;
using Playground.Application.Commands.Facility.Update;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Facility;

public class UpdateFacilityEndPoint : Endpoint<UpdateFacilityCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Put("/facility/update");
    }
    public override async Task<GenericResponse> ExecuteAsync(UpdateFacilityCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}