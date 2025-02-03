using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Playground.Application.Commands.Activity.Create;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints.Activity;

/// <summary>
/// Endpoint para crear una nueva actividad.
/// </summary>
public class CreateActivityEndPoint : Endpoint<CreateActivityCommand, GenericResponse>
{
    /// <summary>
    /// Configura el endpoint para crear una actividad.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Post("activity/create");
    }

    /// <summary>
    /// Maneja la solicitud para crear una nueva actividad.
    /// </summary>
    /// <param name="req">Comando que contiene los datos necesarios para crear la actividad.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta que contiene el resultado de la creación de la actividad.</returns>
    public override async Task<GenericResponse> ExecuteAsync(CreateActivityCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);	
    }
}
