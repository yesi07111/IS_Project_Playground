using Playground.Application.Commands.Auth.ResetPassword;
using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Auth;

/// <summary>
/// Endpoint para gestionar la solicitud de restablecimiento de contraseña.
/// </summary>
public class ResetPasswordEndpoint : Endpoint<ResetPasswordCommand, UserCreationResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y define la ruta para el restablecimiento de la contraseña.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Post("/auth/reset-password");
    }

    /// <summary>
    /// Maneja la solicitud para restablecer la contraseña de un usuario.
    /// </summary>
    /// <param name="req">Comando que contiene la información necesaria para restablecer la contraseña del usuario.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta con los detalles del restablecimiento de la contraseña o el estado de la operación.</returns>
    public override async Task<UserCreationResponse> ExecuteAsync(ResetPasswordCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
