using System.Linq.Expressions;
using Playground.Domain.Entities;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Domain.Specifications
{
    /// <summary>
    /// Especificación para filtrar entidades de recurso.
    /// Proporciona métodos estáticos para crear especificaciones basadas en diferentes propiedades del recurso.
    /// </summary>
    public class ResourceSpecification : ISpecification<Resource>
    {
        private readonly Expression<Func<Resource, bool>> _expression;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ResourceSpecification"/>.
        /// </summary>
        /// <param name="expression">La expresión lambda que define la especificación.</param>
        public ResourceSpecification(Expression<Func<Resource, bool>> expression)
        {
            _expression = expression;
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica AND.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<Resource> And(ISpecification<Resource> other)
        {
            return new AndSpecification<Resource>(this, other);
        }

        /// <summary>
        /// Invierte la especificación actual utilizando una operación lógica NOT.
        /// </summary>
        /// <returns>Una nueva especificación que representa la inversión.</returns>
        public ISpecification<Resource> Not()
        {
            return new NotSpecification<Resource>(this);
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica OR.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<Resource> Or(ISpecification<Resource> other)
        {
            return new OrSpecification<Resource>(this, other);
        }

        /// <summary>
        /// Combina la especificación actual con la negación de otra especificación utilizando una operación lógica AND.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<Resource> AndNot(ISpecification<Resource> other)
        {
            return new AndSpecification<Resource>(this, new NotSpecification<Resource>(other));
        }

        /// <summary>
        /// Combina la especificación actual con la negación de otra especificación utilizando una operación lógica OR.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<Resource> OrNot(ISpecification<Resource> other)
        {
            return new OrSpecification<Resource>(this, new NotSpecification<Resource>(other));
        }


        /// <summary>
        /// Convierte la especificación en una expresión lambda.
        /// </summary>
        /// <returns>Una expresión lambda que representa la especificación.</returns>
        public Expression<Func<Resource, bool>> ToExpression()
        {
            return _expression;
        }

        /// <summary>
        /// Crea una especificación para filtrar recursos por nombre.
        /// </summary>
        /// <param name="name">El nombre del recurso.</param>
        /// <returns>Una especificación que filtra por nombre.</returns>
        public static ResourceSpecification ByName(string name)
        {
            return new ResourceSpecification(resource => resource.Name == name);
        }

        /// <summary>
        /// Crea una especificación para filtrar recursos por tipo.
        /// </summary>
        /// <param name="type">El tipo del recurso.</param>
        /// <returns>Una especificación que filtra por tipo.</returns>
        public static ResourceSpecification ByType(string type)
        {
            return new ResourceSpecification(resource => resource.Type == type);
        }

        public static ResourceSpecification ByFacilityType(string facilityType)
        {
            return new ResourceSpecification(resource => resource.Facility.Type == facilityType);
        }

        /// <summary>
        /// Crea una especificación para filtrar recursos por ubicación.
        /// </summary>
        /// <param name="location">La ubicación del recurso.</param>
        /// <returns>Una especificación que filtra por ubicación.</returns>
        public static ResourceSpecification ByLocation(string location)
        {
            return new ResourceSpecification(resource => resource.Facility.Location == location);
        }

        /// <summary>
        /// Crea una especificación para filtrar recursos por frecuencia de uso.
        /// </summary>
        /// <param name="useFrecuency">La frecuencia de uso del recurso.</param>
        /// <returns>Una especificación que filtra por frecuencia de uso.</returns>
        public static ResourceSpecification ByUseFrecuencyEqual(int useFrecuency)
        {
            return new ResourceSpecification(resource => resource.UseFrequency == useFrecuency);
        }

        public static ResourceSpecification ByUseFrequencyLessOrEqual(int useFrecuency)
        {
            return new ResourceSpecification(resource => resource.UseFrequency <= useFrecuency);
        }

        public static ResourceSpecification ByUseFrequencyMoreOrEqual(int useFrecuency)
        {
            return new ResourceSpecification(resource => resource.UseFrequency >= useFrecuency);
        }

        /// <summary>
        /// Crea una especificación para filtrar recursos por estado del recurso.
        /// </summary>
        /// <param name="resourceCondition">El estado del recurso.</param>
        /// <returns>Una especificación que filtra por estado del recurso.</returns>
        public static ResourceSpecification ByResourceCondition(string resourceCondition)
        {
            return new ResourceSpecification(resource => resource.ResourceCondition == resourceCondition);
        }

        /// <summary>
        /// Crea una especificación para filtrar recursos por instalación.
        /// </summary>
        /// <param name="facility">La instalación asociada al recurso.</param>
        /// <returns>Una especificación que filtra por instalación.</returns>
        public static ResourceSpecification ByFacility(Guid? facility)
        {
            if (facility is null) return new ResourceSpecification(resource => resource.Facility == null);
            return new ResourceSpecification(resource => resource.Facility.Id == facility);
        }

        /// <summary>
        /// Crea una especificación para filtrar recursos por su fecha de creación.
        /// </summary>
        /// <param name="createdAt">La fecha de creación del recurso.</param>
        /// <param name="comparison">El tipo de comparación a realizar (por defecto es "equal").</param>
        /// <returns>Una especificación que filtra por fecha de creación.</returns>
        public static ResourceSpecification ByCreatedAt(DateTime createdAt, string comparison = "equal")
        {
            return new ResourceSpecification(DateTimeSpecification<Resource>.CreateDateComparisonExpression(resource => resource.CreatedAt, createdAt, comparison));
        }

        /// <summary>
        /// Crea una especificación para filtrar recursos por su fecha de actualización.
        /// </summary>
        /// <param name="updateAt">La fecha de actualización del recurso.</param>
        /// <param name="comparison">El tipo de comparación a realizar (por defecto es "equal").</param>
        /// <returns>Una especificación que filtra por fecha de actualización.</returns>
        public static ResourceSpecification ByUpdateAt(DateTime updateAt, string comparison = "equal")
        {
            return new ResourceSpecification(DateTimeSpecification<Resource>.CreateDateComparisonExpression(resource => resource.UpdateAt, updateAt, comparison));
        }

        /// <summary>
        /// Crea una especificación para filtrar recursos por su fecha de eliminación.
        /// </summary>
        /// <param name="deleteAt">La fecha de eliminación del recurso.</param>
        /// <param name="comparison">El tipo de comparación a realizar (por defecto es "equal").</param>
        /// <returns>Una especificación que filtra por fecha de eliminación.</returns>
        public static ResourceSpecification ByDeletedAt(DateTime? deleteAt, string comparison = "equal")
        {
            return new ResourceSpecification(DateTimeSpecification<Resource>.CreateNullableDateComparisonExpression(resource => resource.DeletedAt, deleteAt, comparison));
        }
    }
}