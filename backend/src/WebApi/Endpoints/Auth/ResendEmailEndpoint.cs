using FastEndpoints;
using Playground.Application.Responses;
using Playground.Application.Queries.Auth.ResendEmail;

namespace Playground.WebApi.Endpoints.Auth;

public class ResendVerificationEmailEndpoint : Endpoint<ResendEmailQuery, UserCreationResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/auth/resend-verification-email");
    }

    public override async Task<UserCreationResponse> ExecuteAsync(ResendEmailQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}