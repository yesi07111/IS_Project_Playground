using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Commands.ResourceDate.Delete;

namespace Playground.WebApi.Endpoints.ResourceDate;

/// <summary>
/// Endpoint para eliminar una fecha de recurso.
/// </summary>
public class DeleteResourceDateEndPoint : Endpoint<DeleteResourceDateCommand, GenericResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de eliminación de fechas de recursos.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Delete("/resourceDate/delete");
    }

    /// <summary>
    /// Ejecuta la eliminación de una fecha de recurso procesando el comando recibido.
    /// </summary>
    /// <param name="req">El comando que contiene los datos de la fecha de recurso a eliminar.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta genérica con el resultado de la operación.</returns>
    public override async Task<GenericResponse> ExecuteAsync(DeleteResourceDateCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
