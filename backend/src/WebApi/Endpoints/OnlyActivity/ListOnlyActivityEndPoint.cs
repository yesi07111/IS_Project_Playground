using FastEndpoints;
using OneOf.Types;
using Playground.Application.Queries.OnlyActivity;
using Playground.Application.Queries.Responses;

namespace Playground.WebApi.Endpoints.OnlyActivity;

public class ListOnlyActivityEndPoint : Endpoint<ListOnlyActivityQuery, ListOnlyActivityResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/onlyActivity/get-all");
    }

    public override async Task<ListOnlyActivityResponse> ExecuteAsync(ListOnlyActivityQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}