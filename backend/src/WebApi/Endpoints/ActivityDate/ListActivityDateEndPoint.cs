using FastEndpoints;
using Playground.Application.Queries.ActivityDateOnly;
using Playground.Application.Queries.Responses;

namespace Playground.WebApi.Endpoints.ActivityDate;

public class ListActivityDateEndPoint : Endpoint<ListActivityDateQuery, ListActivityDateResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/activityDate/get-all");
    }

    public override async Task<ListActivityDateResponse> ExecuteAsync(ListActivityDateQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}