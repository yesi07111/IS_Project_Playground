using FastEndpoints;
using Playground.Application.Queries.Facility.List;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Facility;

public class ListFacilityEndpoint : Endpoint<ListFacilityQuery, ListFacilityResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/facility/get-all");
    }

    public override async Task<ListFacilityResponse> ExecuteAsync(ListFacilityQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}