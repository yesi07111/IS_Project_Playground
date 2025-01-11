using System.Linq.Expressions;
using Playground.Domain.Entities;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Domain.Specifications
{
    public class ResourceDateSpecification : ISpecification<ResourceDate>
    {
        private readonly Expression<Func<ResourceDate, bool>> _expression;

        public ResourceDateSpecification(Expression<Func<ResourceDate, bool>> expression)
        {
            _expression = expression;
        }

        public ISpecification<ResourceDate> And(ISpecification<ResourceDate> other)
        {
            return new AndSpecification<ResourceDate>(this, other);
        }

        public ISpecification<ResourceDate> Not()
        {
            return new NotSpecification<ResourceDate>(this);
        }

        public ISpecification<ResourceDate> Or(ISpecification<ResourceDate> other)
        {
            return new OrSpecification<ResourceDate>(this, other);
        }

        public Expression<Func<ResourceDate, bool>> ToExpression()
        {
            return _expression;
        }

        public ResourceDateSpecification ByResource(string resourceId)
        {
            return new ResourceDateSpecification(resourceDate => resourceDate.Resource.Id.ToString() == resourceId);
        }
    }
}