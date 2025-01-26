using Playground.Application.Repositories;

namespace Playground.Application.Services
{
    /// <summary>
    /// Proporciona métodos para calcular calificaciones relacionadas con actividades.
    /// </summary>
    public interface IRatingService
    {
        /// <summary>
        /// Calcula la calificación promedio de una actividad específica, redondea a 1 lugar después de la coma por defecto.
        /// </summary>
        /// <param name="activity">La actividad para la cual se desea calcular la calificación promedio.</param>
        /// <param name="reviewRepository">El repositorio de reseñas utilizado para obtener las calificaciones.</param>
        /// <returns>La calificación promedio de la actividad.</returns>
        double CalculateAverageRating(Domain.Entities.ActivityDate activity, IRepository<Domain.Entities.Review> reviewRepository);

        /// <summary>
        /// Calcula la calificación promedio de una actividad específica, con una cantidad de dígitos específicos a redondear.
        /// </summary>
        /// <param name="activity">La actividad para la cual se desea calcular la calificación promedio.</param>
        /// <param name="reviewRepository">El repositorio de reseñas utilizado para obtener las calificaciones.</param>
        /// <param name="round">El valor utilizado para redondear las calificaciones.</param>
        /// <returns>La calificación promedio de la actividad.</returns>
        double CalculateAverageRating(Domain.Entities.ActivityDate activity, IRepository<Domain.Entities.Review> reviewRepository, int round);
    }
}