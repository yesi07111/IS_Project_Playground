using System.Linq.Expressions;

namespace Playground.Domain.Specifications.BaseSpecifications
{
    /// <summary>
    /// Implementación de una especificación que invierte otra especificación utilizando una operación lógica NOT.
    /// </summary>
    /// <typeparam name="T">El tipo de entidad al que se aplica la especificación.</typeparam>
    public class NotSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _specification;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="NotSpecification{T}"/>.
        /// </summary>
        /// <param name="specification">La especificación a invertir.</param>
        public NotSpecification(ISpecification<T> specification)
        {
            _specification = specification;
        }

        /// <summary>
        /// Convierte la especificación en una expresión lambda que representa la inversión.
        /// </summary>
        /// <returns>Una expresión lambda que representa la especificación invertida.</returns>
        public Expression<Func<T, bool>> ToExpression()
        {
            var expression = _specification.ToExpression();
            var parameter = expression.Parameters[0];
            var body = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public ISpecification<T> And(ISpecification<T> other) => new AndSpecification<T>(this, other);
        public ISpecification<T> Or(ISpecification<T> other) => new OrSpecification<T>(this, other);
        public ISpecification<T> Not() => new NotSpecification<T>(this);
    }
}