using FastEndpoints;
using Playground.Application.Commands.Resource.Delete;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Resource;

public class DeleteResourceEndPoint : Endpoint<DeleteResourceCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Delete("/resource/delete");
    }

    public override async Task<GenericResponse> ExecuteAsync(DeleteResourceCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}