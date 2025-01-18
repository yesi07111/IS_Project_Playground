using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Playground.Application.Commands.Activity.Post;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Activity;

public class PostActivityEndPoint : Endpoint<PostActivityCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("activity/post");
    }

    public override async Task<GenericResponse> ExecuteAsync(PostActivityCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);	
    }
}