using FastEndpoints;
using Playground.Application.Commands.Resource.Delete;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Resource;

/// <summary>
/// Endpoint para eliminar un recurso.
/// </summary>
public class DeleteResourceEndPoint : Endpoint<DeleteResourceCommand, GenericResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de eliminación de recursos.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Delete("/resource/delete");
    }

    /// <summary>
    /// Ejecuta la eliminación de un recurso procesando el comando recibido.
    /// </summary>
    /// <param name="req">El comando que contiene los datos del recurso a eliminar.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta genérica con el resultado de la operación.</returns>
    public override async Task<GenericResponse> ExecuteAsync(DeleteResourceCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
