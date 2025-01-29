using Playground.Application.Repositories;
using Playground.Application.Services;
using Playground.Domain.Entities;
using Playground.Domain.Specifications;

namespace Playground.Infraestructure.Services
{
    /// <summary>
    /// Implementación del servicio de comentarios para actividades.
    /// </summary>
    public class CommentsService : ICommentsService
    {
        /// <inheritdoc />
        public async Task<IEnumerable<string>> GetCommentsAsync(Guid activityId, IRepository<Review> reviewRepository)
        {
            // Obtén las reseñas relacionadas con la actividad
            var reviews = await reviewRepository.GetBySpecificationAsync(
                ReviewSpecification.ByActivityDate(activityId),
                r => r.Parent
            );

            if (reviews is null) return [];

            // Filtra y transforma los datos en memoria
            var filteredReviews = reviews
                .Where(r => r.DeletedAt == null && r.Parent != null)
                .Select(r => $"{r.Parent.UserName}:{r.Score}:{r.Comments}");

            return filteredReviews;
        }
    }
}