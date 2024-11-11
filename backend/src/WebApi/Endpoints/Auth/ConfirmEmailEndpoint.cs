using Playground.Application.Commands.Auth.ConfirmEmail;
using Playground.Application.Commands.Dtos;
using FastEndpoints;

namespace Playground.WebApi.Endpoints.Auth;

public class ConfirmEmailEndpoint : Endpoint<ConfirmEmailCommand, UserActionResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/auth/confirm-email");
    }

    public override async Task<UserActionResponse> ExecuteAsync(ConfirmEmailCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}