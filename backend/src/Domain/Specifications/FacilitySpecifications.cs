using System.Linq.Expressions;
using Playground.Domain.Entities;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Domain.Specifications
{
    /// <summary>
    /// Especificación para filtrar entidades de instalación.
    /// Proporciona métodos estáticos para crear especificaciones basadas en diferentes propiedades de la instalación.
    /// </summary>
    public class FacilitySpecification : ISpecification<Facility>
    {
        private readonly Expression<Func<Facility, bool>> _expression;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="FacilitySpecification"/>.
        /// </summary>
        /// <param name="expression">La expresión lambda que define la especificación.</param>
        public FacilitySpecification(Expression<Func<Facility, bool>> expression)
        {
            _expression = expression;
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica AND.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<Facility> And(ISpecification<Facility> other)
        {
            return new AndSpecification<Facility>(this, other);
        }

        /// <summary>
        /// Invierte la especificación actual utilizando una operación lógica NOT.
        /// </summary>
        /// <returns>Una nueva especificación que representa la inversión.</returns>
        public ISpecification<Facility> Not()
        {
            return new NotSpecification<Facility>(this);
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica OR.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<Facility> Or(ISpecification<Facility> other)
        {
            return new AndSpecification<Facility>(this, other);
        }

        /// <summary>
        /// Convierte la especificación en una expresión lambda.
        /// </summary>
        /// <returns>Una expresión lambda que representa la especificación.</returns>
        public Expression<Func<Facility, bool>> ToExpression()
        {
            return _expression;
        }

        /// <summary>
        /// Crea una especificación para filtrar instalaciones por nombre.
        /// </summary>
        /// <param name="name">El nombre de la instalación.</param>
        /// <returns>Una especificación que filtra por nombre.</returns>
        public static FacilitySpecification ByName(string name)
        {
            return new FacilitySpecification(facility => facility.Name == name);
        }

        /// <summary>
        /// Crea una especificación para filtrar instalaciones por ubicación.
        /// </summary>
        /// <param name="location">La ubicación de la instalación.</param>
        /// <returns>Una especificación que filtra por ubicación.</returns>
        public static FacilitySpecification ByLocation(string location)
        {
            return new FacilitySpecification(facility => facility.Location == location);
        }

        /// <summary>
        /// Crea una especificación para filtrar instalaciones por tipo.
        /// </summary>
        /// <param name="type">El tipo de la instalación.</param>
        /// <returns>Una especificación que filtra por tipo.</returns>
        public static FacilitySpecification ByType(string type)
        {
            return new FacilitySpecification(facility => facility.Type == type);
        }

        /// <summary>
        /// Crea una especificación para filtrar instalaciones por capacidad máxima.
        /// </summary>
        /// <param name="maximumCapacity">La capacidad máxima de la instalación.</param>
        /// <returns>Una especificación que filtra por capacidad máxima.</returns>
        public static FacilitySpecification ByMaximumCapacity(int maximumCapacity)
        {
            return new FacilitySpecification(facility => facility.MaximumCapacity == maximumCapacity);
        }

        /// <summary>
        /// Crea una especificación para filtrar instalaciones por política de uso.
        /// </summary>
        /// <param name="usagePolicy">La política de uso de la instalación.</param>
        /// <returns>Una especificación que filtra por política de uso.</returns>
        public static FacilitySpecification ByUsagePolicy(string usagePolicy)
        {
            return new FacilitySpecification(facility => facility.UsagePolicy == usagePolicy);
        }

        /// <summary>
        /// Crea una especificación para filtrar instalaciones por su fecha de creación.
        /// </summary>
        /// <param name="createdAt">La fecha de creación de la instalación.</param>
        /// <param name="comparison">El tipo de comparación a realizar (por defecto es "equal").</param>
        /// <returns>Una especificación que filtra por fecha de creación.</returns>
        public static FacilitySpecification ByCreatedAt(DateTime createdAt, string comparison = "equal")
        {
            return new FacilitySpecification(DateTimeSpecification<Facility>.CreateDateComparisonExpression(facility => facility.CreatedAt, createdAt, comparison));
        }

        /// <summary>
        /// Crea una especificación para filtrar instalaciones por su fecha de actualización.
        /// </summary>
        /// <param name="updateAt">La fecha de actualización de la instalación.</param>
        /// <param name="comparison">El tipo de comparación a realizar (por defecto es "equal").</param>
        /// <returns>Una especificación que filtra por fecha de actualización.</returns>
        public static FacilitySpecification ByUpdateAt(DateTime updateAt, string comparison = "equal")
        {
            return new FacilitySpecification(DateTimeSpecification<Facility>.CreateDateComparisonExpression(facility => facility.UpdateAt, updateAt, comparison));
        }

        /// <summary>
        /// Crea una especificación para filtrar instalaciones por su fecha de eliminación.
        /// </summary>
        /// <param name="deleteAt">La fecha de eliminación de la instalación.</param>
        /// <param name="comparison">El tipo de comparación a realizar (por defecto es "equal").</param>
        /// <returns>Una especificación que filtra por fecha de eliminación.</returns>
        public static FacilitySpecification ByDeletedAt(DateTime? deleteAt, string comparison = "equal")
        {
            return new FacilitySpecification(DateTimeSpecification<Facility>.CreateNullableDateComparisonExpression(facility => facility.DeletedAt, deleteAt, comparison));
        }
    }
}