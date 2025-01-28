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

        /// <summary>
        /// Combina la especificación actual con la negación de otra especificación utilizando una operación lógica AND.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<ResourceDate> AndNot(ISpecification<ResourceDate> other)
        {
            return new AndSpecification<ResourceDate>(this, new NotSpecification<ResourceDate>(other));
        }

        /// <summary>
        /// Combina la especificación actual con la negación de otra especificación utilizando una operación lógica OR.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<ResourceDate> OrNot(ISpecification<ResourceDate> other)
        {
            return new OrSpecification<ResourceDate>(this, new NotSpecification<ResourceDate>(other));
        }

        public Expression<Func<ResourceDate, bool>> ToExpression()
        {
            return _expression;
        }

        public static ResourceDateSpecification ByResource(string resourceId)
        {
            if (resourceId is null) return new ResourceDateSpecification(resourceDate => resourceDate.Resource == null);
            return new ResourceDateSpecification(resourceDate => resourceDate.Resource.Id.ToString() == resourceId);
        }

        public static ResourceDateSpecification ByDate(DateTime date)
        {
            return new ResourceDateSpecification(resourceDate => resourceDate.Date.ToDateTime(default) == date);
        }
    }
}