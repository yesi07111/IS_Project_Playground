using FastEndpoints;
using Playground.Application.Commands.Resource.Update;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Resource;

/// <summary>
/// Endpoint para actualizar un recurso.
/// </summary>
public class UpdateResourceEndPoint : Endpoint<UpdateResourceCommand, GenericResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de actualización de recursos.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Put("/resource/update");
    }

    /// <summary>
    /// Ejecuta la actualización de un recurso procesando el comando recibido.
    /// </summary>
    /// <param name="req">El comando que contiene los datos del recurso a actualizar.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta genérica con el resultado de la operación.</returns>
    public override async Task<GenericResponse> ExecuteAsync(UpdateResourceCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
