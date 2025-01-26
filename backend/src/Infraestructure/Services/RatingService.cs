using Playground.Application.Repositories;
using Playground.Application.Services;
using Playground.Domain.Entities;
using Playground.Domain.Specifications;

namespace Playground.Infraestructure.Services
{
    /// <summary>
    /// Implementación del servicio de calificaciones para actividades.
    /// </summary>
    public class RatingService : IRatingService
    {
        /// <inheritdoc />
        public double CalculateAverageRating(ActivityDate activity, IRepository<Review> reviewRepository)
        {
            var reviews = reviewRepository.GetBySpecificationAsync(
                ReviewSpecification.ByActivityDate(activity.Id),
                r => r.ActivityDate,
                r => r.ActivityDate.Activity
            ).Result;

            if (reviews.Any())
            {
                return Math.Round(reviews.Average(r => r.Score), 1);
            }
            return 0.0;
        }

        public double CalculateAverageRating(ActivityDate activity, IRepository<Review> reviewRepository, int round)
        {
            var reviews = reviewRepository.GetBySpecificationAsync(
                            ReviewSpecification.ByActivityDate(activity.Id),
                            r => r.ActivityDate,
                            r => r.ActivityDate.Activity
                        ).Result;

            if (reviews.Any())
            {
                return Math.Round(reviews.Average(r => r.Score), round);
            }
            return 0.0;
        }
    }
}