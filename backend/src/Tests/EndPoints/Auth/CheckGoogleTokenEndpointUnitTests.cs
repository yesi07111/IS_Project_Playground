using FastEndpoints;
using Playground.Application.Queries.Auth.CheckGoogleToken;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Auth;

public class CheckGoogleTokenEndpoint : Endpoint<CheckGoogleTokenQuery, GoogleTokenValidationResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/auth/check-google-token");
    }

    public override async Task<GoogleTokenValidationResponse> ExecuteAsync(CheckGoogleTokenQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}