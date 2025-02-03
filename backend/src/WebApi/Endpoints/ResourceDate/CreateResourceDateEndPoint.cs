using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Commands.ResourceDate.Create;

namespace Playground.WebApi.Endpoints.ResourceDate;

/// <summary>
/// Endpoint para crear una nueva fecha de recurso.
/// </summary>
public class CreateResourceDateEndPoint : Endpoint<CreateResourceDateCommand, GenericResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de creación de fechas de recursos.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Post("/resourceDate/create");
    }

    /// <summary>
    /// Ejecuta la creación de una fecha de recurso procesando el comando recibido.
    /// </summary>
    /// <param name="req">El comando que contiene los datos de la fecha de recurso a crear.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta genérica con el resultado de la operación.</returns>
    public override async Task<GenericResponse> ExecuteAsync(CreateResourceDateCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
