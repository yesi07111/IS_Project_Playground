using Playground.Application.Queries.Auth.VerifyRecaptcha;
using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints;

/// <summary>
/// Endpoint para verificar un token de reCAPTCHA.
/// </summary>
public class VerifyRecaptchaEndpoint : Endpoint<VerifyRecaptchaQuery, RecaptchaVerificationResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir la verificación de reCAPTCHA.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/auth/verify-captcha");
    }

    /// <summary>
    /// Maneja la solicitud para verificar el token de reCAPTCHA.
    /// </summary>
    /// <param name="req">Consulta que contiene el token de reCAPTCHA a verificar.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta con el resultado de la verificación de reCAPTCHA.</returns>
    public override Task<RecaptchaVerificationResponse> ExecuteAsync(VerifyRecaptchaQuery req, CancellationToken ct)
    {
        return req.ExecuteAsync(ct);
    }
}
