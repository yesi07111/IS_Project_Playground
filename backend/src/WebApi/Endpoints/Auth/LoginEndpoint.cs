using Playground.Application.Commands.Auth.Login;
using Playground.Application.Commands.Dtos;
using FastEndpoints;

namespace Playground.WebApi.Endpoints;

public class LoginEndpoint : Endpoint<LoginCommand, UserActionResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/auth/login");
    }

    public override Task<UserActionResponse> ExecuteAsync(LoginCommand req, CancellationToken ct)
    {
        return req.ExecuteAsync(ct);
    }
}