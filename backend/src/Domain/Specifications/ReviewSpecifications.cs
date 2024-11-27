//newLaura

using System.Linq.Expressions;
using Playground.Domain.Entities;
using Playground.Domain.Entities.Auth;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Domain.Specifications
{
    public class ReviewSpecification : ISpecification<Review>
    {
        private readonly Expression<Func<Review, bool>> _expression;

        public ReviewSpecification(Expression<Func<Review, bool>> expression)
        {
            _expression = expression;
        }

        public ISpecification<Review> And(ISpecification<Review> other)
        {
            return new AndSpecification<Review>(this, other);
        }

        public ISpecification<Review> Not()
        {
            return new NotSpecification<Review>(this);
        }

        public ISpecification<Review> Or(ISpecification<Review> other)
        {
            return new AndSpecification<Review>(this, other);
        }

        public Expression<Func<Review, bool>> ToExpression()
        {
            return _expression;
        }

        public static ReviewSpecification ByDateTime(DateTime datetime) //ordenar por fecha y hora
        {
            return new ReviewSpecification(review => review.Date == datetime);
        }

        public static ReviewSpecification ByDate(DateTime date) //ordenar por fecha
        {
            return new ReviewSpecification(review => review.Date.Date == date.Date);
        }

        public static ReviewSpecification ByTime(int time) //ordenar por hora
        {
            return new ReviewSpecification(review => review.Date.Hour == time);
        }

        public static ReviewSpecification ByDayOfWeek(DayOfWeek day) //ordenar por dia de la semana
        {
            return new ReviewSpecification(review => review.Date.DayOfWeek == day);
        }

        public static ReviewSpecification ByParentId(User parentId) //por padre
        {
            return new ReviewSpecification(review => review.ParentId == parentId);
        }

        public static ReviewSpecification ByFacility(Facility facility) //por instalacion
        {
            return new ReviewSpecification(review => review.Facility == facility);
        }

        public static ReviewSpecification ByScore(int score) //por score
        {
            return new ReviewSpecification(review => review.Score == score);
        }
    }
}