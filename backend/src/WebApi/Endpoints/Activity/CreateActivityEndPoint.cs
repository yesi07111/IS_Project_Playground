using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Playground.Application.Commands.Activity.Create;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Activity;

public class CreateActivityEndPoint : Endpoint<CreateActivityCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("activity/create");
    }

    public override async Task<GenericResponse> ExecuteAsync(CreateActivityCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);	
    }
}