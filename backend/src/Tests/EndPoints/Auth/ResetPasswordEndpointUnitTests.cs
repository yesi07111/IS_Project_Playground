using Playground.Application.Commands.Auth.ResetPassword;
using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Auth;

public class ResetPasswordEndpoint : Endpoint<ResetPasswordCommand, UserCreationResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/auth/reset-password");
    }

    public override async Task<UserCreationResponse> ExecuteAsync(ResetPasswordCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}