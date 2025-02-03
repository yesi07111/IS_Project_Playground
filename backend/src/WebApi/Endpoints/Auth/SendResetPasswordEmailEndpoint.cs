using FastEndpoints;
using Playground.Application.Responses;
using Playground.Application.Queries.Auth.SendResetPasswordEmail;

namespace Playground.WebApi.Endpoints.Auth;

/// <summary>
/// Endpoint para enviar un correo electrónico de restablecimiento de contraseña.
/// </summary>
public class SendResetPasswordEmailEndpoint : Endpoint<SendResetPasswordEmailQuery, UserActionResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y define la ruta para enviar un correo de restablecimiento de contraseña.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/auth/send-reset-password-email");
    }

    /// <summary>
    /// Maneja la solicitud para enviar un correo electrónico de restablecimiento de contraseña.
    /// </summary>
    /// <param name="req">Consulta que contiene la información necesaria para enviar el correo de restablecimiento.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta que indica si la acción fue exitosa o no.</returns>
    public override async Task<UserActionResponse> ExecuteAsync(SendResetPasswordEmailQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
