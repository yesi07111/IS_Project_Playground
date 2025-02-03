using FastEndpoints;
using Playground.Application.Queries.HomePage;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.HomePage;

public class GetHomePageInfoEndpoint(GetHomePageInfoQueryHandler queryHandler) : EndpointWithoutRequest<GetHomePageInfoResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/get/homepage");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await queryHandler.ExecuteAsync(ct);
        await SendAsync(response, cancellation: ct);
    }
}