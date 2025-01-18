using System.Linq.Expressions;
using Playground.Domain.Specifications.BaseSpecifications;
using Playground.Domain.Entities;
using Activity = Playground.Domain.Entities.Activity;
using Playground.Domain.Entities.Auth;

namespace Playground.Domain.Specifications
{
    /// <summary>
    /// Especificación para filtrar entidades de actividad.
    /// Proporciona métodos estáticos para crear especificaciones basadas en diferentes propiedades de la actividad.
    /// </summary>
    public class ActivitySpecification : ISpecification<Activity>
    {
        private readonly Expression<Func<Activity, bool>> _expression;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ActivitySpecification"/>.
        /// </summary>
        /// <param name="expression">La expresión lambda que define la especificación.</param>
        public ActivitySpecification(Expression<Func<Activity, bool>> expression)
        {
            _expression = expression;
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica AND.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<Activity> And(ISpecification<Activity> other)
        {
            return new AndSpecification<Activity>(this, other);
        }

        /// <summary>
        /// Invierte la especificación actual utilizando una operación lógica NOT.
        /// </summary>
        /// <returns>Una nueva especificación que representa la inversión.</returns>
        public ISpecification<Activity> Not()
        {
            return new NotSpecification<Activity>(this);
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica OR.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<Activity> Or(ISpecification<Activity> other)
        {
            return new OrSpecification<Activity>(this, other);
        }

        /// Convierte la especificación en una expresión lambda.
        /// </summary>
        /// <returns>Una expresión lambda que representa la especificación.</returns>
        public Expression<Func<Activity, bool>> ToExpression()
        {
            return _expression;
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades por nombre.
        /// </summary>
        /// <param name="name">El nombre de la actividad.</param>
        /// <returns>Una especificación que filtra por nombre.</returns>
        public static ActivitySpecification ByName(string name)
        {
            return new ActivitySpecification(activity => activity.Name == name);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades por tipo de instalación.
        /// </summary>
        /// <param name="facilityType">El tipo de instalación de la actividad.</param>
        /// <returns>Una especificación que filtra por tipo de instalación.</returns>
        public static ActivitySpecification ByFacilityType(string facilityType)
        {
            return new ActivitySpecification(activity => activity.Facility.Type == facilityType);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades por educador.
        /// </summary>
        /// <param name="educatorId">El id del educador asociado a la actividad.</param>
        /// <returns>Una especificación que filtra por educador.</returns>
        public static ActivitySpecification ByEducator(string educatorId)
        {
            return new ActivitySpecification(activity => activity.Educator.Id == educatorId);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades por tipo.
        /// </summary>
        /// <param name="type">El tipo de la actividad.</param>
        /// <returns>Una especificación que filtra por tipo.</returns>
        public static ActivitySpecification ByType(string type)
        {
            return new ActivitySpecification(activity => activity.Type == type);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades por edad recomendada igual a un valor dado.
        /// </summary>
        /// <param name="recommendedAge">La edad recomendada para la actividad.</param>
        /// <returns>Una especificación que filtra por edad recomendada igual.</returns>
        public static ActivitySpecification ByRecommendedAgeEqual(int recommendedAge)
        {
            return new ActivitySpecification(activity => activity.RecommendedAge == recommendedAge);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades por edad recomendada menor o igual a un valor dado.
        /// </summary>
        /// <param name="recommendedAge">La edad recomendada para la actividad.</param>
        /// <returns>Una especificación que filtra por edad recomendada menor o igual.</returns>
        public static ActivitySpecification ByRecommendedAgeLessOrEqual(int recommendedAge)
        {
            return new ActivitySpecification(activity => activity.RecommendedAge <= recommendedAge);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades por edad recomendada mayor o igual a un valor dado.
        /// </summary>
        /// <param name="recommendedAge">La edad recomendada para la actividad.</param>
        /// <returns>Una especificación que filtra por edad recomendada mayor o igual.</returns>
        public static ActivitySpecification ByRecommendedAgeMoreOrEqual(int recommendedAge)
        {
            return new ActivitySpecification(activity => activity.RecommendedAge >= recommendedAge);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades por privacidad.
        /// </summary>
        /// <param name="itsPrivate">Indica si la actividad es privada.</param>
        /// <returns>Una especificación que filtra por privacidad.</returns>
        public static ActivitySpecification ByItsPrivate(bool itsPrivate)
        {
            return new ActivitySpecification(activity => activity.ItsPrivate == itsPrivate);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades por instalación.
        /// </summary>
        /// <param name="facility">La instalación asociada a la actividad.</param>
        /// <returns>Una especificación que filtra por instalación.</returns>
        public static ActivitySpecification ByFacility(Guid facility)
        {
            return new ActivitySpecification(activity => activity.Facility.Id == facility);
        }

        public static ActivitySpecification ByDescription(string description)
        {
            return new ActivitySpecification(activity => activity.Description == description);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades por su fecha de creación.
        /// </summary>
        /// <param name="createdAt">La fecha de creación de la actividad.</param>
        /// <param name="comparison">El tipo de comparación a realizar (por defecto es "equal").</param>
        /// <returns>Una especificación que filtra por fecha de creación.</returns>
        public static ActivitySpecification ByCreatedAt(DateTime createdAt, string comparison = "equal")
        {
            return new ActivitySpecification(DateTimeSpecification<Activity>.CreateDateComparisonExpression(activity => activity.CreatedAt, createdAt, comparison));
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades por su fecha de actualización.
        /// </summary>
        /// <param name="updateAt">La fecha de actualización de la actividad.</param>
        /// <param name="comparison">El tipo de comparación a realizar (por defecto es "equal").</param>
        /// <returns>Una especificación que filtra por fecha de actualización.</returns>
        public static ActivitySpecification ByUpdateAt(DateTime updateAt, string comparison = "equal")
        {
            return new ActivitySpecification(DateTimeSpecification<Activity>.CreateDateComparisonExpression(activity => activity.UpdateAt, updateAt, comparison));
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades por su fecha de eliminación.
        /// </summary>
        /// <param name="deleteAt">La fecha de eliminación de la actividad.</param>
        /// <param name="comparison">El tipo de comparación a realizar (por defecto es "equal").</param>
        /// <returns>Una especificación que filtra por fecha de eliminación.</returns>
        public static ActivitySpecification ByDeleteAt(DateTime? deleteAt, string comparison = "equal")
        {
            return new ActivitySpecification(DateTimeSpecification<Activity>.CreateNullableDateComparisonExpression(activity => activity.DeletedAt, deleteAt, comparison));
        }
    }
}