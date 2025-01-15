using FastEndpoints;
using Playground.Application.Queries.Resource.List;
using Playground.Application.Queries.Responses;

namespace Playground.WebApi.Endpoints.Resource;

public class ListResourceEndPoint : Endpoint<ListResourceQuery, ListResourceResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/resource/get-all");
    }

    public override async Task<ListResourceResponse> ExecuteAsync(ListResourceQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}