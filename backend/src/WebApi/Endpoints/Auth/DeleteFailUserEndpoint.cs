using FastEndpoints;
using Playground.Application.Commands.DeleteFailUser;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints;

/// <summary>
/// Endpoint para eliminar un usuario fallido de autenticación.
/// </summary>
public class DeleteFailUserEndpoint : Endpoint<DeleteFailUserCommand, DeleteFailUserResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta para eliminar un usuario fallido.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Delete("/auth/delete-fail-user");
    }

    /// <summary>
    /// Ejecuta la eliminación del usuario fallido de autenticación.
    /// </summary>
    /// <param name="req">El comando que contiene la solicitud para eliminar el usuario.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta indicando el resultado de la operación.</returns>
    public override async Task<DeleteFailUserResponse> ExecuteAsync(DeleteFailUserCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
