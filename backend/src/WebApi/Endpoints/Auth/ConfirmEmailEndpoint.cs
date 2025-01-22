using Playground.Application.Queries.Auth.ConfirmEmail;
using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Auth;

public class ConfirmEmailEndpoint : Endpoint<ConfirmEmailQuery, UserActionResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/auth/confirm-email");
    }

    public override async Task<UserActionResponse> ExecuteAsync(ConfirmEmailQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}