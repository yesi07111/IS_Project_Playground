using Playground.Application.Repositories;

namespace Playground.Application.Services
{
    /// <summary>
    /// Proporciona métodos para obtener comentarios relacionados con actividades.
    /// </summary>
    public interface ICommentsService
    {
        /// <summary>
        /// Obtiene los comentarios de una actividad específica.
        /// </summary>
        /// <param name="activityId">El identificador único de la actividad.</param>
        /// <param name="reviewRepository">El repositorio de reseñas utilizado para obtener los comentarios.</param>
        /// <returns>Una lista de comentarios en formato de cadena.</returns>
        Task<IEnumerable<string>> GetCommentsAsync(Guid activityId, IRepository<Domain.Entities.Review> reviewRepository);
    }
}