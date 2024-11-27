//newLaura

using System.Linq.Expressions;
using Playground.Domain.Entities;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Domain.Specifications
{
    public class ResourceSpecification : ISpecification<Resource>
    {
        private readonly Expression<Func<Resource, bool>> _expression;

        public ResourceSpecification(Expression<Func<Resource, bool>> expression)
        {
            _expression = expression;
        }

        public ISpecification<Resource> And(ISpecification<Resource> other)
        {
            return new AndSpecification<Resource>(this, other);
        }

        public ISpecification<Resource> Not()
        {
            return new NotSpecification<Resource>(this);
        }

        public ISpecification<Resource> Or(ISpecification<Resource> other)
        {
            return new AndSpecification<Resource>(this, other);
        }

        public Expression<Func<Resource, bool>> ToExpression()
        {
            return _expression;
        }

        public static ResourceSpecification ByName(string name) //por nombre
        {
            return new ResourceSpecification(resource => resource.Name == name);
        }

        public static ResourceSpecification ByType(string type) //por tipo
        {
            return new ResourceSpecification(resource => resource.Type == type);
        }

        public static ResourceSpecification ByLocation(string location) //por ubicacion 
        {
            return new ResourceSpecification(resource => resource.Facility.Location == location);
        }

        public static ResourceSpecification ByUseFrecuency(float useFrecuency) //por frecuencia de uso
        {
            return new ResourceSpecification(resource => resource.UseFrecuency == useFrecuency);
        }

        public static ResourceSpecification ByResourceCondition(string resourceCondition) //por estado del recurso
        {
            return new ResourceSpecification(resource => resource.ResourceCondition == resourceCondition);
        }

        public static ResourceSpecification ByFacility(Facility facility) //por institucion
        {
            return new ResourceSpecification(resource => resource.Facility == facility);
        }
    }
}