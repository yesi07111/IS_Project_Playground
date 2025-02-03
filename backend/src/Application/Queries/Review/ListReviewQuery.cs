using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Queries.Review.List;

/// <summary>
/// Representa una consulta para obtener una lista de reseñas.
/// Esta consulta implementa la interfaz <see cref="ICommand{TResponse}"/> donde <see cref="ListReviewResponse"/>
/// es el tipo de respuesta esperada.
/// </summary>
/// <remarks>
/// Esta consulta puede ser utilizada para filtrar reseñas por un ID de usuario específico y un caso de uso.
/// </remarks>
public record ListReviewQuery : ICommand<ListReviewResponse>
{
    /// <summary>
    /// Obtiene o inicializa el ID del usuario para filtrar las reseñas.
    /// </summary>
    /// <value>
    /// El ID del usuario. Puede ser nulo si no se desea filtrar por usuario.
    /// </value>
    public string? UserId { get; init; }

    /// <summary>
    /// Obtiene o inicializa el caso de uso asociado a la consulta.
    /// </summary>
    /// <value>
    /// El caso de uso. Por defecto es una cadena vacía.
    /// </value>
    public string UseCase { get; init; } = string.Empty;
}