using System.Linq.Expressions;

namespace Playground.Domain.Specifications.BaseSpecifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpression();
        ISpecification<T> And(ISpecification<T> other);
        ISpecification<T> Or(ISpecification<T> other);
        ISpecification<T> Not();
    }
}