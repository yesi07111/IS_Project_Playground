using FastEndpoints;
using Playground.Application.Commands.Facility.Create;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Facility;

/// <summary>
/// Endpoint para crear una nueva instalación.
/// </summary>
public class CreateFacilityEndPoint : Endpoint<CreateFacilityCommand, GenericResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta para la creación de una instalación.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Post("/facility/create");
    }

    /// <summary>
    /// Ejecuta el comando para crear una nueva instalación.
    /// </summary>
    /// <param name="req">El comando que contiene los datos necesarios para crear la instalación.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta que indica el resultado de la creación de la instalación.</returns>
    public override async Task<GenericResponse> ExecuteAsync(CreateFacilityCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
