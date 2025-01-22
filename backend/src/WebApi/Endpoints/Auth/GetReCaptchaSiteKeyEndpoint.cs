using FastEndpoints;
using Playground.Application.Responses;
using Playground.Application.Queries.Auth.GetRecaptchaSiteKey;

namespace Playground.WebApi.Endpoints;

public class GetRecaptchaSiteKeyEndpoint : EndpointWithoutRequest<ReCaptchaSiteKeyResponse>
{
    private readonly GetRecaptchaSiteKeyQueryHandler _queryHandler;

    public GetRecaptchaSiteKeyEndpoint(GetRecaptchaSiteKeyQueryHandler queryHandler)
    {
        _queryHandler = queryHandler;
    }

    public override void Configure()
    {
        AllowAnonymous();
        Get("/auth/get-captcha-site-key");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await _queryHandler.ExecuteAsync(ct);
        await SendAsync(response, cancellation: ct);
    }
}