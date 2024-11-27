//new Laura
using System.Diagnostics;
using System.Linq.Expressions;
using Playground.Domain.Specifications.BaseSpecifications;
using Playground.Domain.Entities;
using Activity = Playground.Domain.Entities.Activity;
using Playground.Domain.Entities.Auth;

namespace Playground.Domain.Specifications
{
    public class ActivitySpecification : ISpecification<Activity>
    {
        private readonly Expression<Func<Activity, bool>> _expression;

        public ActivitySpecification(Expression<Func<Activity, bool>> expression)
        {
            _expression = expression;
        }

        public ISpecification<Activity> And(ISpecification<Activity> other)
        {
            return new AndSpecification<Activity>(this, other);
        }

        public ISpecification<Activity> Not()
        {
            return new NotSpecification<Activity>(this);
        }

        public ISpecification<Activity> Or(ISpecification<Activity> other)
        {
            return new OrSpecification<Activity>(this, other);
        }

        public Expression<Func<Activity, bool>> ToExpression()
        {
            return _expression;
        }

        public static ActivitySpecification ByName(string name) //ordenar por nombre
        {
            return new ActivitySpecification(activity => activity.Name == name);
        }

        public static ActivitySpecification ByDateTime(DateTime datetime) //ordenar por fecha y hora
        {
            return new ActivitySpecification(activity => activity.Date == datetime);
        }

        public static ActivitySpecification ByDate(DateTime date) //ordenar por fecha
        {
            return new ActivitySpecification(activity => activity.Date.Date == date.Date);
        }

        public static ActivitySpecification ByTime(int time) //ordenar por hora
        {
            return new ActivitySpecification(activity => activity.Date.Hour == time);
        }

        public static ActivitySpecification ByDayOfWeek(DayOfWeek day) //ordenar por dia de la semana
        {
            return new ActivitySpecification(activity => activity.Date.DayOfWeek == day);
        }

        public static ActivitySpecification ByEducador(User educatorId) //por educador (tipo coger todas las actividades que gestiona fulano)
        {
            return new ActivitySpecification(activity => activity.EducatorId == educatorId);
        }

        public static ActivitySpecification ByType(string type) //por tipo de actvidad
        {
            return new ActivitySpecification(activity => activity.Type == type);
        }

        public static ActivitySpecification ByRecommendedAge(int recommendedAge) //por edad recomendada
        {
            return new ActivitySpecification(activity => activity.RecommendedAge == recommendedAge);
        }

        public static ActivitySpecification ByItsPrivate(bool itsPrivate) //por las que sean privadas o no
        {
            return new ActivitySpecification(activity => activity.ItsPrivate == itsPrivate);
        }

        public static ActivitySpecification ByFacility(Facility facility) //por institucion
        {
            return new ActivitySpecification(activity => activity.Facility == facility);
        }
    }
}