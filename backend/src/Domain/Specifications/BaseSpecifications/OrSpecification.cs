using System.Linq.Expressions;

namespace Playground.Domain.Specifications.BaseSpecifications
{
    /// <summary>
    /// Implementación de una especificación que combina dos especificaciones utilizando una operación lógica OR.
    /// </summary>
    /// <typeparam name="T">El tipo de entidad al que se aplica la especificación.</typeparam>
    public class OrSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        /// <summary>
        /// Inicializar una nueva instancia de la clase <see cref="OrSpecification{T}"/>.
        /// </summary>
        /// <param name="left">La primera especificación a combinar.</param>
        /// <param name="right">La segunda especificación a combinar.</param>
        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        /// <summary>
        /// Convierte la especificación en una expresión lambda que representa la combinación OR.
        /// </summary>
        /// <returns>Una expresión lambda que representa la especificación combinada.</returns>
        public Expression<Func<T, bool>> ToExpression()
        {
            var leftExpression = _left.ToExpression();
            var rightExpression = _right.ToExpression();

            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.OrElse(
                Expression.Invoke(leftExpression, parameter),
                Expression.Invoke(rightExpression, parameter)
            );

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public ISpecification<T> And(ISpecification<T> other) => new AndSpecification<T>(this, other);
        public ISpecification<T> Or(ISpecification<T> other) => new OrSpecification<T>(this, other);
        public ISpecification<T> Not() => new NotSpecification<T>(this);
        public ISpecification<T> AndNot(ISpecification<T> other) => new AndSpecification<T>(this, new NotSpecification<T>(other));
        public ISpecification<T> OrNot(ISpecification<T> other) => new OrSpecification<T>(this, new NotSpecification<T>(other));

        public ISpecification<T> AndNot()
        {
            throw new NotImplementedException();
        }

        public ISpecification<T> OrNot()
        {
            throw new NotImplementedException();
        }
    }
}