using System.Linq.Expressions;
using Playground.Domain.Entities;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Domain.Specifications
{
    /// <summary>
    /// Representa una especificación que se utiliza para filtrar objetos de tipo <see cref="ResourceDate"/> en función de una expresión booleana.
    /// </summary>
    public class ResourceDateSpecification : ISpecification<ResourceDate>
    {
        private readonly Expression<Func<ResourceDate, bool>> _expression;

        /// <summary>
        /// Inicializa una nueva instancia de la especificación con una expresión booleana.
        /// </summary>
        /// <param name="expression">La expresión booleana que se utilizará para filtrar los objetos de tipo <see cref="ResourceDate"/>.</param>
        public ResourceDateSpecification(Expression<Func<ResourceDate, bool>> expression)
        {
            _expression = expression;
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica AND.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación de ambas especificaciones con una operación AND.</returns>
        public ISpecification<ResourceDate> And(ISpecification<ResourceDate> other)
        {
            return new AndSpecification<ResourceDate>(this, other);
        }

        /// <summary>
        /// Negates the current specification, returning a new specification that represents the opposite condition.
        /// </summary>
        /// <returns>Una nueva especificación que representa la negación de la especificación actual.</returns>
        public ISpecification<ResourceDate> Not()
        {
            return new NotSpecification<ResourceDate>(this);
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica OR.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación de ambas especificaciones con una operación OR.</returns>
        public ISpecification<ResourceDate> Or(ISpecification<ResourceDate> other)
        {
            return new OrSpecification<ResourceDate>(this, other);
        }

        /// <summary>
        /// Combina la especificación actual con la negación de otra especificación utilizando una operación lógica AND.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación de la especificación actual con la negación de la otra especificación usando una operación AND.</returns>
        public ISpecification<ResourceDate> AndNot(ISpecification<ResourceDate> other)
        {
            return new AndSpecification<ResourceDate>(this, new NotSpecification<ResourceDate>(other));
        }

        /// <summary>
        /// Combina la especificación actual con la negación de otra especificación utilizando una operación lógica OR.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación de la especificación actual con la negación de la otra especificación usando una operación OR.</returns>
        public ISpecification<ResourceDate> OrNot(ISpecification<ResourceDate> other)
        {
            return new OrSpecification<ResourceDate>(this, new NotSpecification<ResourceDate>(other));
        }

        /// <summary>
        /// Convierte la especificación en una expresión booleana que puede ser utilizada en consultas.
        /// </summary>
        /// <returns>Una expresión booleana que representa la especificación.</returns>
        public Expression<Func<ResourceDate, bool>> ToExpression()
        {
            return _expression;
        }

        /// <summary>
        /// Crea una especificación que filtra por el ID de un recurso.
        /// </summary>
        /// <param name="resourceId">El ID del recurso que se utilizará para filtrar.</param>
        /// <returns>Una nueva especificación que filtra por el ID del recurso.</returns>
        public static ResourceDateSpecification ByResource(string resourceId)
        {
            if (resourceId is null) return new ResourceDateSpecification(resourceDate => resourceDate.Resource == null);
            return new ResourceDateSpecification(resourceDate => resourceDate.Resource.Id.ToString() == resourceId);
        }

        /// <summary>
        /// Crea una especificación que filtra por una fecha específica.
        /// </summary>
        /// <param name="date">La fecha que se utilizará para filtrar.</param>
        /// <returns>Una nueva especificación que filtra por la fecha proporcionada.</returns>
        public static ResourceDateSpecification ByDate(DateTime date)
        {
            return new ResourceDateSpecification(resourceDate => resourceDate.Date.ToDateTime(default) == date);
        }
    }
}
