using FastEndpoints;
using Playground.Application.Commands.Resource.Update;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Resource;

public class UpdateResourceEndPoint : Endpoint<UpdateResourceCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Put("/resource/update");
    }

    public override async Task<GenericResponse> ExecuteAsync(UpdateResourceCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}