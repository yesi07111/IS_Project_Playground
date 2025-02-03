using FastEndpoints;
using Playground.Application.Responses;
using Playground.Application.Queries.Auth.ResendEmail;

namespace Playground.WebApi.Endpoints.Auth;

/// <summary>
/// Endpoint para gestionar el reenvío del correo de verificación a un usuario.
/// </summary>
public class ResendVerificationEmailEndpoint : Endpoint<ResendEmailQuery, UserCreationResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y define la ruta para el reenvío del correo de verificación.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/auth/resend-verification-email");
    }

    /// <summary>
    /// Maneja la solicitud para reenviar el correo de verificación a un usuario.
    /// </summary>
    /// <param name="req">Comando que contiene la información necesaria para reenviar el correo de verificación.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta con los detalles de la creación del usuario o el estado del reenvío del correo.</returns>
    public override async Task<UserCreationResponse> ExecuteAsync(ResendEmailQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
