using FastEndpoints;
using Playground.Application.Commands.Facility.Update;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Facility;

/// <summary>
/// Endpoint para actualizar una instalación.
/// </summary>
public class UpdateFacilityEndPoint : Endpoint<UpdateFacilityCommand, GenericResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta para actualizar una instalación.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Put("/facility/update");
    }

    /// <summary>
    /// Ejecuta la actualización de una instalación con los datos proporcionados en el comando.
    /// </summary>
    /// <param name="req">El comando que contiene los datos necesarios para actualizar la instalación.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta genérica indicando el resultado de la operación.</returns>
    public override async Task<GenericResponse> ExecuteAsync(UpdateFacilityCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
