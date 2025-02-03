using Playground.Application.Commands.Auth.GoogleAccess;
using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Auth;

/// <summary>
/// Endpoint para gestionar el acceso a través de Google en el sistema.
/// </summary>
public class GoogleAccessEndpoint : Endpoint<GoogleAccessCommand, UserActionResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y define la ruta para el acceso a través de Google.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Post("/auth/google-access");
    }

    /// <summary>
    /// Maneja la solicitud para gestionar el acceso a través de Google y devuelve una respuesta de acción del usuario.
    /// </summary>
    /// <param name="req">Comando que contiene la información necesaria para gestionar el acceso de Google.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta con la acción que el sistema debe realizar después de gestionar el acceso.</returns>
    public override async Task<UserActionResponse> ExecuteAsync(GoogleAccessCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
