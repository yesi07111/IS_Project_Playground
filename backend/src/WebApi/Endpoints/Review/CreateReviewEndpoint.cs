using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Commands.Review.Create;

namespace Playground.WebApi.Endpoints.Review;

/// <summary>
/// Endpoint para la creación de una reseña.
/// </summary>
public class CreateReviewEndpoint : Endpoint<CreateReviewCommand, GenericResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de creación de reseñas.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Post("/review/create");
    }

    /// <summary>
    /// Ejecuta la creación de una reseña procesando el comando recibido.
    /// </summary>
    /// <param name="req">El comando que contiene los datos de la reseña a crear.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta genérica con el resultado de la operación.</returns>
    public override async Task<GenericResponse> ExecuteAsync(CreateReviewCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
