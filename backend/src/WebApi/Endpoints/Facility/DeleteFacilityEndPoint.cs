using FastEndpoints;
using Org.BouncyCastle.Ocsp;
using Playground.Application.Commands.Facility.Delete;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Facility;

/// <summary>
/// Endpoint para eliminar una instalación.
/// </summary>
public class DeleteFacilityEndpoint : Endpoint<DeleteFacilityCommand, GenericResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta para eliminar una instalación.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Delete("/facility/delete");
    }

    /// <summary>
    /// Ejecuta el comando para eliminar una instalación.
    /// </summary>
    /// <param name="req">El comando que contiene la información necesaria para eliminar la instalación.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta que indica el resultado de la eliminación de la instalación.</returns>
    public override async Task<GenericResponse> ExecuteAsync(DeleteFacilityCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
