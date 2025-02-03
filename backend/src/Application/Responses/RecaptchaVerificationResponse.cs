namespace Playground.Application.Responses;

/// <summary>
/// Representa la respuesta para la verificación de reCAPTCHA.
/// </summary>
public record RecaptchaVerificationResponse(bool IsValid, string Message);