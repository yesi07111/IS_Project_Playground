using FastEndpoints;
using Playground.Application.Commands.Activity.Update;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Activity;

public class UpdateActivityEndPoint : Endpoint<UpdateActivityCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Put("/activity/update");
    }

    public override async Task<GenericResponse> ExecuteAsync(UpdateActivityCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);	
    }
}