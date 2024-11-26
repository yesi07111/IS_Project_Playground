using System.Linq.Expressions;

namespace Playground.Domain.Specifications.BaseSpecifications
{
    public class NotSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _specification;

        public NotSpecification(ISpecification<T> specification)
        {
            _specification = specification;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            var expression = _specification.ToExpression();
            var parameter = expression.Parameters[0];
            var body = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public ISpecification<T> And(ISpecification<T> other) => new AndSpecification<T>(this, other);
        public ISpecification<T> Or(ISpecification<T> other) => new OrSpecification<T>(this, other);
        public ISpecification<T> Not() => new NotSpecification<T>(this);
    }
}