using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Commands.Review.Update;

namespace Playground.WebApi.Endpoints.Review;

/// <summary>
/// Endpoint para actualizar una reseña.
/// </summary>
public class UpdateReviewEndpoint : Endpoint<UpdateReviewCommand, GenericResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de actualización de reseñas.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Put("/review/update");
    }

    /// <summary>
    /// Ejecuta la actualización de una reseña procesando el comando recibido.
    /// </summary>
    /// <param name="req">El comando que contiene los datos de la reseña a actualizar.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta genérica con el resultado de la operación.</returns>
    public override async Task<GenericResponse> ExecuteAsync(UpdateReviewCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
