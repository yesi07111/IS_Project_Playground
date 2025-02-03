using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Commands.User.Delete;

namespace Playground.WebApi.Endpoints.User;

/// <summary>
/// Endpoint para la eliminación de un usuario.
/// </summary>
public class DeleteUserEndpoint : Endpoint<DeleteUserCommand, GenericResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de eliminación de usuario.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Delete("/user/delete");
    }

    /// <summary>
    /// Ejecuta la eliminación del usuario procesando el comando recibido.
    /// </summary>
    /// <param name="req">El comando que contiene los datos del usuario a eliminar.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta genérica con el resultado de la operación.</returns>
    public override async Task<GenericResponse> ExecuteAsync(DeleteUserCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}