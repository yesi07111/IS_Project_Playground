using FastEndpoints;
using Playground.Application.Responses;
using Playground.Application.Queries.Auth.GetGoogleClientId;

namespace Playground.WebApi.Endpoints.Auth;

public class GetGoogleClientIdEndpoint : EndpointWithoutRequest<GoogleClientIdResponse>
{
    private readonly GetGoogleClientIdQueryHandler _queryHandler;

    public GetGoogleClientIdEndpoint(GetGoogleClientIdQueryHandler queryHandler)
    {
        _queryHandler = queryHandler;
    }

    public override void Configure()
    {
        AllowAnonymous();
        Get("/auth/get-google-client-id");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await _queryHandler.ExecuteAsync(ct);
        await SendAsync(response, cancellation: ct);
    }
}