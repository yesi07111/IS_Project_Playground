using FastEndpoints;
using Playground.Application.Queries.Activity.List;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Activity;

public class ListActivityEndpoint : Endpoint<ListActivityQuery, ListActivityResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/activity/get-all");
    }

    public override async Task<ListActivityResponse> ExecuteAsync(ListActivityQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}