using System.Linq.Expressions;

namespace Playground.Domain.Specifications.BaseSpecifications
{
    public class AndSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            var leftExpression = _left.ToExpression();
            var rightExpression = _right.ToExpression();

            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.AndAlso(
                Expression.Invoke(leftExpression, parameter),
                Expression.Invoke(rightExpression, parameter)
            );

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public ISpecification<T> And(ISpecification<T> other) => new AndSpecification<T>(this, other);
        public ISpecification<T> Or(ISpecification<T> other) => new OrSpecification<T>(this, other);
        public ISpecification<T> Not() => new NotSpecification<T>(this);
    }
}