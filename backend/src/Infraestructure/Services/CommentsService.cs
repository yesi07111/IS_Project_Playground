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
            var reviewSpecification = ReviewSpecification.ByActivityDate(activityId);
            var reviews = await reviewRepository.GetBySpecificationAsync(reviewSpecification, r => r.Parent);

            return reviews.Select(r => $"{r.Parent.UserName}:{r.Score}:{r.Comments}").ToList();
        }
    }
}