using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Queries.Auth.VerifyRecaptcha;

public record VerifyRecaptchaQuery(string Token) : ICommand<RecaptchaVerificationResponse>;