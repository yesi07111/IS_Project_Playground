namespace Playground.Domain.Specifications.BaseSpecifications
{
    public class OrSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public bool IsSatisfiedBy(T entity)
        {
            return _left.IsSatisfiedBy(entity) || _right.IsSatisfiedBy(entity);
        }

        public ISpecification<T> And(ISpecification<T> other) => new AndSpecification<T>(this, other);
        public ISpecification<T> Or(ISpecification<T> other) => new OrSpecification<T>(this, other);
        public ISpecification<T> Not() => new NotSpecification<T>(this);
    }

}