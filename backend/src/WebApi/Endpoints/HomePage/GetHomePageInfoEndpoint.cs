using FastEndpoints;
using Playground.Application.Queries.HomePage;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.HomePage;

public class GetHomePageInfoEndpoint(GetHomePageInfoQueryHandler queryHandler) : EndpointWithoutRequest<GetHomePageInfoResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/get");
    }

    public override async Task<GetHomePageInfoResponse> HandleAsync(CancellationToken ct)
    {
        return await queryHandler.ExecuteAsync(ct);
    }
}