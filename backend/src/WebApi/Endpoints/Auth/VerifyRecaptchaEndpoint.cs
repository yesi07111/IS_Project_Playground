using Playground.Application.Queries.Auth.VerifyRecaptcha;
using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints;

public class VerifyRecaptchaEndpoint : Endpoint<VerifyRecaptchaQuery, RecaptchaVerificationResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/auth/verify-captcha");
    }

    public override Task<RecaptchaVerificationResponse> ExecuteAsync(VerifyRecaptchaQuery req, CancellationToken ct)
    {
        return req.ExecuteAsync(ct);
    }
}