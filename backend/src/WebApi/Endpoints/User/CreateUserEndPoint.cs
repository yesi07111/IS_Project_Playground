using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Commands.User.Create;

namespace Playground.WebApi.Endpoints.User;

/// <summary>
/// Endpoint para la creación de un usuario.
/// </summary>
public class CreateUserEndpoint : Endpoint<CreateUserCommand, GenericResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de creación de usuario.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Post("/user/create");
    }

    /// <summary>
    /// Ejecuta la creación del usuario procesando el comando recibido.
    /// </summary>
    /// <param name="req">El comando que contiene los datos del usuario a crear.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta genérica con el resultado de la operación.</returns>
    public override async Task<GenericResponse> ExecuteAsync(CreateUserCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}