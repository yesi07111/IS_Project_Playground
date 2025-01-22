using FastEndpoints;
using Playground.Application.Commands.Auth.RefreshToken;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Auth;

public class RefreshTokenEndpoint : Endpoint<RefreshTokenCommand, UserActionResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/auth/refresh-token");
    }

    public override async Task<UserActionResponse> ExecuteAsync(RefreshTokenCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}