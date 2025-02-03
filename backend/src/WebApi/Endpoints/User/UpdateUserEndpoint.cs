using FastEndpoints;
using Playground.Application.Commands.User.Update;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.User;

/// <summary>
/// Endpoint para actualizar la información de un usuario.
/// </summary>
public class UpdateUserEndpoint : Endpoint<UpdateUserCommand, UpdateUserResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de actualización de usuario.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Put("/user/edit");
    }

    /// <summary>
    /// Ejecuta la actualización del usuario procesando el comando recibido.
    /// </summary>
    /// <param name="req">El comando que contiene los datos del usuario a actualizar.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta con el resultado de la actualización del usuario.</returns>
    public override async Task<UpdateUserResponse> ExecuteAsync(UpdateUserCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
