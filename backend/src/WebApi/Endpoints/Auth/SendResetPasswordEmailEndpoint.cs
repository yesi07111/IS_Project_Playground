using FastEndpoints;
using Playground.Application.Responses;
using Playground.Application.Queries.Auth.SendResetPasswordEmail;

namespace Playground.WebApi.Endpoints.Auth;

public class SendResetPasswordEmailEndpoint : Endpoint<SendResetPasswordEmailQuery, UserActionResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/auth/send-reset-password-email");
    }

    public override async Task<UserActionResponse> ExecuteAsync(SendResetPasswordEmailQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}