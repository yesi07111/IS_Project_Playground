using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Commands.Review.Delete;

namespace Playground.WebApi.Endpoints.Review;

/// <summary>
/// Endpoint para la eliminación de una reseña.
/// </summary>
public class DeleteReviewEndpoint : Endpoint<DeleteReviewCommand, GenericResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de eliminación de reseñas.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Delete("/review/delete");
    }

    /// <summary>
    /// Ejecuta la eliminación de una reseña procesando el comando recibido.
    /// </summary>
    /// <param name="req">El comando que contiene los datos de la reseña a eliminar.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta genérica con el resultado de la operación.</returns>
    public override async Task<GenericResponse> ExecuteAsync(DeleteReviewCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
