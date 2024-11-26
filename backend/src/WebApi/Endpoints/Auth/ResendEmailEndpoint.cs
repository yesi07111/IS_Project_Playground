using Playground.Application.Commands.Auth.ResendEmail;
using Playground.Application.Commands.Dtos;
using FastEndpoints;

namespace Playground.WebApi.Endpoints.Auth;

public class ResendVerificationEmailEndpoint : Endpoint<ResendEmailCommand, UserCreationResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/auth/resend-verification-email");
    }

    public override async Task<UserCreationResponse> ExecuteAsync(ResendEmailCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}