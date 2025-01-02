using System.Linq.Expressions;
using Playground.Domain.Entities;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Domain.Specifications
{
    /// <summary>
    /// Especificación para filtrar entidades de actividad.
    /// Proporciona métodos estáticos para crear especificaciones basadas en diferentes propiedades de la actividad.
    /// </summary>
    public class ActivityDateSpecification : ISpecification<ActivityDate>
    {
        private readonly Expression<Func<ActivityDate, bool>> _expression;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ActivityDateSpecification"/>.
        /// </summary>
        /// <param name="expression">La expresión lambda que define la especificación.</param>
        public ActivityDateSpecification(Expression<Func<ActivityDate, bool>> expression)
        {
            _expression = expression;
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica AND.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<ActivityDate> And(ISpecification<ActivityDate> other)
        {
            return new AndSpecification<ActivityDate>(this, other);
        }

        /// <summary>
        /// Invierte la especificación actual utilizando una operación lógica NOT.
        /// </summary>
        /// <returns>Una nueva especificación que representa la inversión.</returns>
        public ISpecification<ActivityDate> Not()
        {
            return new NotSpecification<ActivityDate>(this);
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica OR.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<ActivityDate> Or(ISpecification<ActivityDate> other)
        {
            return new OrSpecification<ActivityDate>(this, other);
        }

        /// Convierte la especificación en una expresión lambda.
        /// </summary>
        /// <returns>Una expresión lambda que representa la especificación.</returns>
        public Expression<Func<ActivityDate, bool>> ToExpression()
        {
            return _expression;
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades que ocurren en o después de una fecha dada.
        /// </summary>
        /// <param name="date">La fecha de la actividad.</param>
        /// <returns>Una especificación que filtra por fecha mayor o igual.</returns>
        public static ActivityDateSpecification ByDateMoreOrEqual(DateTime date)
        {
            return new ActivityDateSpecification(activityDate => activityDate.DateTime.Date >= date.Date);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades que ocurren en o antes de una fecha dada.
        /// </summary>
        /// <param name="date">La fecha de la actividad.</param>
        /// <returns>Una especificación que filtra por fecha menor o igual.</returns>
        public static ActivityDateSpecification ByDateLessOrEqual(DateTime date)
        {
            return new ActivityDateSpecification(activityDate => activityDate.DateTime.Date <= date.Date);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades que ocurren en una fecha exacta.
        /// </summary>
        /// <param name="date">La fecha de la actividad.</param>
        /// <returns>Una especificación que filtra por fecha exacta.</returns>
        public static ActivityDateSpecification ByDateEqual(DateTime date)
        {
            return new ActivityDateSpecification(activityDate => activityDate.DateTime.Date == date.Date);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades que ocurren a una hora exacta.
        /// </summary>
        /// <param name="time">La hora de la actividad.</param>
        /// <returns>Una especificación que filtra por hora exacta.</returns>
        public static ActivityDateSpecification ByTimeEqual(TimeSpan time)
        {
            return new ActivityDateSpecification(activityDate => activityDate.DateTime.TimeOfDay == time);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades que ocurren a una hora igual o posterior.
        /// </summary>
        /// <param name="time">La hora de la actividad.</param>
        /// <returns>Una especificación que filtra por hora mayor o igual.</returns>
        public static ActivityDateSpecification ByTimeMoreOrEqual(TimeSpan time)
        {
            System.Console.WriteLine("DateTime.UtcNow.TimeOfDay >= time: {0} y DateTime.UtcNow.TimeOfDay: {1}", DateTime.UtcNow.TimeOfDay >= time, DateTime.UtcNow.TimeOfDay);
            return new ActivityDateSpecification(activityDate => activityDate.DateTime.TimeOfDay >= time);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades que ocurren a una hora igual o anterior.
        /// </summary>
        /// <param name="time">La hora de la actividad.</param>
        /// <returns>Una especificación que filtra por hora menor o igual.</returns>
        public static ActivityDateSpecification ByTimeLessOrEqual(TimeSpan time)
        {
            return new ActivityDateSpecification(activityDate => activityDate.DateTime.TimeOfDay <= time);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades que ocurren a partir de hoy.
        /// </summary>
        /// <returns>Una especificación que filtra por actividades que ocurren a partir hoy.</returns>
        public static ActivityDateSpecification FromToday()
        {
            DateTime today = DateTime.UtcNow.Date;
            return new ActivityDateSpecification(activityDate => activityDate.DateTime.Date >= today);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades que ocurren hoy.
        /// </summary>
        /// <returns>Una especificación que filtra por actividades que ocurren hoy.</returns>
        public static ActivityDateSpecification ByToday()
        {
            DateTime today = DateTime.UtcNow.Date;
            return new ActivityDateSpecification(activityDate => activityDate.DateTime.Date == today);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades que ocurren mañana.
        /// </summary>
        /// <returns>Una especificación que filtra por actividades que ocurren mañana.</returns>
        public static ActivityDateSpecification ByTomorrow()
        {
            DateTime tomorrow = DateTime.UtcNow.Date.AddDays(1);
            return new ActivityDateSpecification(activityDate => activityDate.DateTime.Date == tomorrow);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades por día de la semana.
        /// </summary>
        /// <param name="day">El día de la semana de la actividad.</param>
        /// <returns>Una especificación que filtra por día de la semana.</returns>
        public static ActivityDateSpecification ByDayOfWeek(DayOfWeek day)
        {
            return new ActivityDateSpecification(activityDate => activityDate.DateTime.DayOfWeek == day);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades que ocurren durante la semana actual.
        /// </summary>
        /// <returns>Una especificación que filtra por actividades que ocurren esta semana.</returns>
        public static ActivityDateSpecification ByThisWeek()
        {
            DateTime today = DateTime.UtcNow.Date;
            // Asumiendo que la semana comienza el lunes
            int daysUntilStartOfWeek = (int)today.DayOfWeek - (int)DayOfWeek.Monday;
            if (daysUntilStartOfWeek < 0)
            {
                daysUntilStartOfWeek += 7; // Ajuste si el día de hoy es domingo
            }
            DateTime startOfWeek = today.AddDays(-daysUntilStartOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            return new ActivityDateSpecification(activityDate => activityDate.DateTime.Date >= startOfWeek && activityDate.DateTime.Date <= endOfWeek);
        }

        /// <summary>
        /// Crea una especificación para filtrar actividades por capacidad disponible.
        /// </summary>
        /// <param name="capacity">La capacidad mínima disponible requerida.</param>
        /// <returns>Una especificación que filtra por capacidad disponible.</returns>
        public static ActivityDateSpecification ByAvailableCapacity(int capacity)
        {
            return new ActivityDateSpecification(activityDate => (activityDate.Activity.Facility.MaximumCapacity - activityDate.ReservedPlaces) >= capacity);
        }

    }
}