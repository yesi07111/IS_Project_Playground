using Playground.Application.Commands.Auth.Register;
using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Auth;

public class RegisterEndpoint : Endpoint<RegisterCommand, UserCreationResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/auth/register");
    }

    public override async Task<UserCreationResponse> ExecuteAsync(RegisterCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}