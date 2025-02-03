using FastEndpoints;
using Playground.Application.Commands.Auth.RefreshToken;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Auth;

/// <summary>
/// Endpoint para gestionar el proceso de actualización del token de acceso.
/// </summary>
public class RefreshTokenEndpoint : Endpoint<RefreshTokenCommand, UserActionResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y define la ruta para la actualización del token de acceso.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Post("/auth/refresh-token");
    }

    /// <summary>
    /// Maneja la solicitud para actualizar el token de acceso y devuelve una respuesta con la acción del usuario.
    /// </summary>
    /// <param name="req">Comando que contiene la información necesaria para realizar la actualización del token.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta con la acción que el sistema debe realizar después de intentar actualizar el token de acceso.</returns>
    public override async Task<UserActionResponse> ExecuteAsync(RefreshTokenCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
