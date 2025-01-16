using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Commands.UsageFrequency;

namespace Playground.WebApi.Endpoints.Resource;

public class PostUsageFrequencyEndPoint : Endpoint<PostUsageFrequencyCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/resource/post-useFrequency");
    }

    public override async Task<GenericResponse> ExecuteAsync(PostUsageFrequencyCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}