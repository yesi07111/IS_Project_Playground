using FastEndpoints;
using Playground.Application.Commands.Resource.Create;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Resource;

/// <summary>
/// Endpoint para crear un nuevo recurso.
/// </summary>
public class CreateResourceEndPoint : Endpoint<CreateResourceCommand, GenericResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de creación de recursos.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Post("/resource/create");
    }

    /// <summary>
    /// Ejecuta la creación de un nuevo recurso procesando el comando recibido.
    /// </summary>
    /// <param name="req">El comando que contiene los datos del recurso a crear.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta genérica con el resultado de la operación.</returns>
    public override async Task<GenericResponse> ExecuteAsync(CreateResourceCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
