using System.Linq.Expressions;
using Playground.Domain.Entities;
using Playground.Domain.Entities.Auth;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Domain.Specifications
{
    /// <summary>
    /// Especificación para filtrar entidades de reseña.
    /// Proporciona métodos estáticos para crear especificaciones basadas en diferentes propiedades de la reseña.
    /// </summary>
    public class ReviewSpecification : ISpecification<Review>
    {
        private readonly Expression<Func<Review, bool>> _expression;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ReviewSpecification"/>.
        /// </summary>
        /// <param name="expression">La expresión lambda que define la especificación.</param>
        public ReviewSpecification(Expression<Func<Review, bool>> expression)
        {
            _expression = expression;
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica AND.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<Review> And(ISpecification<Review> other)
        {
            return new AndSpecification<Review>(this, other);
        }

        /// <summary>
        /// Invierte la especificación actual utilizando una operación lógica NOT.
        /// </summary>
        /// <returns>Una nueva especificación que representa la inversión.</returns>
        public ISpecification<Review> Not()
        {
            return new NotSpecification<Review>(this);
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica OR.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<Review> Or(ISpecification<Review> other)
        {
            return new AndSpecification<Review>(this, other);
        }

        /// <summary>
        /// Convierte la especificación en una expresión lambda.
        /// </summary>
        /// <returns>Una expresión lambda que representa la especificación.</returns>
        public Expression<Func<Review, bool>> ToExpression()
        {
            return _expression;
        }

        /// <summary>
        /// Crea una especificación para filtrar reseñas por fecha y hora exactas.
        /// </summary>
        /// <param name="datetime">La fecha y hora de la reseña.</param>
        /// <returns>Una especificación que filtra por fecha y hora.</returns>
        public static ReviewSpecification ByDateTime(DateTime datetime)
        {
            return new ReviewSpecification(review => review.CreatedAt == datetime);
        }

        /// <summary>
        /// Crea una especificación para filtrar reseñas por fecha.
        /// </summary>
        /// <param name="date">La fecha de la reseña.</param>
        /// <returns>Una especificación que filtra por fecha.</returns>
        public static ReviewSpecification ByDate(DateTime date)
        {
            return new ReviewSpecification(review => review.CreatedAt.Date == date.Date);
        }

        /// <summary>
        /// Crea una especificación para filtrar reseñas por hora.
        /// </summary>
        /// <param name="time">La hora de la reseña.</param>
        /// <returns>Una especificación que filtra por hora.</returns>
        public static ReviewSpecification ByTime(int time)
        {
            return new ReviewSpecification(review => review.CreatedAt.Hour == time);
        }

        /// <summary>
        /// Crea una especificación para filtrar reseñas por día de la semana.
        /// </summary>
        /// <param name="day">El día de la semana de la reseña.</param>
        /// <returns>Una especificación que filtra por día de la semana.</returns>
        public static ReviewSpecification ByDayOfWeek(DayOfWeek day)
        {
            return new ReviewSpecification(review => review.CreatedAt.DayOfWeek == day);
        }

        /// <summary>
        /// Crea una especificación para filtrar reseñas por el usuario padre.
        /// </summary>
        /// <param name="parent">El usuario padre asociado a la reseña.</param>
        /// <returns>Una especificación que filtra por usuario padre.</returns>
        public static ReviewSpecification ByParent(string parent)
        {
            return new ReviewSpecification(review => review.Parent.Id == parent);
        }

        /// <summary>
        /// Crea una especificación para filtrar reseñas por actividad.
        /// </summary>
        /// <param name="activity">La actividad asociada a la reseña.</param>
        /// <returns>Una especificación que filtra por actividad.</returns>
        public static ReviewSpecification ByActivity(Guid activity)
        {
            return new ReviewSpecification(review => review.ActivityDate.Activity.Id == activity);
        }

        /// <summary>
        /// Crea una especificación para filtrar reseñas por puntuación.
        /// </summary>
        /// <param name="score">La puntuación de la reseña.</param>
        /// <returns>Una especificación que filtra por puntuación.</returns>
        public static ReviewSpecification ByScore(int score)
        {
            return new ReviewSpecification(review => review.Score == score);
        }

        /// <summary>
        /// Crea una especificación para filtrar reseñas por su fecha de creación.
        /// </summary>
        /// <param name="createdAt">La fecha de creación de la reseña.</param>
        /// <param name="comparison">El tipo de comparación a realizar (por defecto es "equal").</param>
        /// <returns>Una especificación que filtra por fecha de creación.</returns>
        public static ReviewSpecification ByCreatedAt(DateTime createdAt, string comparison = "equal")
        {
            return new ReviewSpecification(DateTimeSpecification<Review>.CreateDateComparisonExpression(review => review.CreatedAt, createdAt, comparison));
        }

        /// <summary>
        /// Crea una especificación para filtrar reseñas por su fecha de actualización.
        /// </summary>
        /// <param name="updateAt">La fecha de actualización de la reseña.</param>
        /// <param name="comparison">El tipo de comparación a realizar (por defecto es "equal").</param>
        /// <returns>Una especificación que filtra por fecha de actualización.</returns>
        public static ReviewSpecification ByUpdateAt(DateTime updateAt, string comparison = "equal")
        {
            return new ReviewSpecification(DateTimeSpecification<Review>.CreateDateComparisonExpression(review => review.UpdateAt, updateAt, comparison));
        }

        /// <summary>
        /// Crea una especificación para filtrar reseñas por su fecha de eliminación.
        /// </summary>
        /// <param name="deleteAt">La fecha de eliminación de la reseña.</param>
        /// <param name="comparison">El tipo de comparación a realizar (por defecto es "equal").</param>
        /// <returns>Una especificación que filtra por fecha de eliminación.</returns>
        public static ReviewSpecification ByDeletedAt(DateTime? deleteAt, string comparison = "equal")
        {
            return new ReviewSpecification(DateTimeSpecification<Review>.CreateNullableDateComparisonExpression(review => review.DeletedAt, deleteAt, comparison));
        }
    }
}