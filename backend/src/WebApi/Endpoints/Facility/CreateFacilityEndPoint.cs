using FastEndpoints;
using Playground.Application.Commands.Facility.Create;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Facility;

public class CreateFacilityEndPoint : Endpoint<CreateFacilityCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/facility/create");
    }

    public override async Task<GenericResponse> ExecuteAsync(CreateFacilityCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}