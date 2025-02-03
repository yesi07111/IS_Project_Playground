using FastEndpoints;
using Playground.Application.Commands.Activity.Delete;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Activity;

public class DeleteActivityEndpoint : Endpoint<DeleteActivityCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Delete("/activity/delete");
    }

    public override async Task<GenericResponse> ExecuteAsync (DeleteActivityCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}