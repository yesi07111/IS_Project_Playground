using System.Linq.Expressions;

namespace Playground.Domain.Specifications.BaseSpecifications
{
    /// <summary>
    /// Interfaz para definir especificaciones que pueden ser utilizadas para filtrar entidades.
    /// Proporciona métodos para combinar especificaciones utilizando operaciones lógicas.
    /// </summary>
    /// <typeparam name="T">El tipo de entidad al que se aplica la especificación.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Convierte la especificación en una expresión lambda.
        /// </summary>
        /// <returns>Una expresión lambda que representa la especificación.</returns>
        Expression<Func<T, bool>> ToExpression();

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica AND.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        ISpecification<T> And(ISpecification<T> other);

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica OR.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        ISpecification<T> Or(ISpecification<T> other);

        /// <summary>
        /// Invierte la especificación actual utilizando una operación lógica NOT.
        /// </summary>
        /// <returns>Una nueva especificación que representa la inversión.</returns>
        ISpecification<T> Not();
    }
}