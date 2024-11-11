using Playground.Application.Commands.Auth.Register;
using Playground.Application.Commands.Dtos;
using FastEndpoints;

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