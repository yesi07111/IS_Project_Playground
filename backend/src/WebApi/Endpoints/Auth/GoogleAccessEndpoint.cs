using Playground.Application.Commands.Auth.GoogleAccess;
using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Auth;

public class GoogleAccessEndpoint : Endpoint<GoogleAccessCommand, UserActionResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/auth/google-access");
    }

    public override async Task<UserActionResponse> ExecuteAsync(GoogleAccessCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}