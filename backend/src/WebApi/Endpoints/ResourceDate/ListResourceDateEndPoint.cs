using FastEndpoints;
using Playground.Application.Queries.ResourceDate;
using Playground.Application.Queries.Responses;

namespace Playground.WebApi.Endpoints.ResourceDate;

public class ListResourceDateEndPoint : Endpoint<ListResourceDateQuery, ListResourceDateResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/resourceDate/get-all");
    }

    public override async Task<ListResourceDateResponse> ExecuteAsync(ListResourceDateQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}