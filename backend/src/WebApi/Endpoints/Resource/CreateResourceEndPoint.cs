using FastEndpoints;
using Playground.Application.Commands.Resource.Create;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Resource;

public class CreateResourceEndPoint : Endpoint<CreateResourceCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/resource/create");
    }

    public override async Task<GenericResponse> ExecuteAsync(CreateResourceCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
} 