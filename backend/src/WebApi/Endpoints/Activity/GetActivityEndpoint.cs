using FastEndpoints;
using Playground.Application.Queries.Activity.Get;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Activity;

public class GetActivityEndpoint : Endpoint<GetActivityQuery, GetActivityResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/activity/get");
    }

    public override async Task<GetActivityResponse> ExecuteAsync(GetActivityQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}