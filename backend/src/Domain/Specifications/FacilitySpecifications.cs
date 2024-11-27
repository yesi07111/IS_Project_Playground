//newLaura
using System.Linq.Expressions;
using Playground.Domain.Entities;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Domain.Specifications
{
    public class FacilitySpecification : ISpecification<Facility>
    {
        private readonly Expression<Func<Facility, bool>> _expression;

        public FacilitySpecification(Expression<Func<Facility, bool>> expression)
        {
            _expression = expression;
        }
        public ISpecification<Facility> And(ISpecification<Facility> other)
        {
            return new AndSpecification<Facility>(this, other);
        }

        public ISpecification<Facility> Not()
        {
            return new NotSpecification<Facility>(this);
        }

        public ISpecification<Facility> Or(ISpecification<Facility> other)
        {
            return new AndSpecification<Facility>(this, other);
        }

        public Expression<Func<Facility, bool>> ToExpression()
        {
            return _expression;
        }

        public static FacilitySpecification ByName(string name) //por nombre
        {
            return new FacilitySpecification(facility => facility.Name == name);
        }

        public static FacilitySpecification ByLocation(string location) //por ubicacion
        {
            return new FacilitySpecification(facility => facility.Location == location);
        }

        public static FacilitySpecification ByType(string type) //por tipo de instalacion
        {
            return new FacilitySpecification(facility => facility.Type == type);
        }

        public static FacilitySpecification ByMaximumCapacity(int maximumCapacity) //por maximo de cpacidad
        {
            return new FacilitySpecification(facility => facility.MaximumCapacity == maximumCapacity);
        }
    }
}