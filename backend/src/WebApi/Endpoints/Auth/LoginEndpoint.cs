using FastEndpoints;
using Playground.Application.Responses;
using Playground.Application.Queries.Auth.Login;

namespace Playground.WebApi.Endpoints;

public class LoginEndpoint : Endpoint<LoginQuery, UserActionResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/auth/login");
    }

    public override Task<UserActionResponse> ExecuteAsync(LoginQuery req, CancellationToken ct)
    {
        return req.ExecuteAsync(ct);
    }
}