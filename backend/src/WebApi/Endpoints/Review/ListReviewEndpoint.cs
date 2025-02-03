using FastEndpoints;
using Playground.Application.Queries.Review.List;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Review;

/// <summary>
/// Endpoint para obtener una lista de reseñas.
/// </summary>
public class ListReviewEndpoint : Endpoint<ListReviewQuery, ListReviewResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de obtención de la lista de reseñas.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/review/get-all");
    }

    /// <summary>
    /// Ejecuta la consulta para obtener la lista de reseñas procesando el query recibido.
    /// </summary>
    /// <param name="req">El query que contiene los criterios para listar las reseñas.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta con la lista de reseñas.</returns>
    public override async Task<ListReviewResponse> ExecuteAsync(ListReviewQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
