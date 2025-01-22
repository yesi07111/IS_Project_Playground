namespace Playground.Application.Responses;

public record RecaptchaVerificationResponse(bool IsValid, string Message);