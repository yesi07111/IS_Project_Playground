using System;
using System.Linq.Expressions;
using Playground.Domain.Entities.Auth;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Domain.Specifications
{
    /// <summary>
    /// Especificación para la entidad Rol.
    /// </summary>
    public class RolSpecification : ISpecification<Rol>
    {
        private readonly Expression<Func<Rol, bool>> _expression;

        /// <summary>
        /// Constructor privado para crear una especificación basada en una expresión.
        /// </summary>
        /// <param name="expression">La expresión que define la especificación.</param>
        private RolSpecification(Expression<Func<Rol, bool>> expression)
        {
            _expression = expression;
        }

        /// <summary>
        /// Convierte la especificación en una expresión lambda.
        /// </summary>
        /// <returns>Una expresión lambda que representa la especificación.</returns>
        public Expression<Func<Rol, bool>> ToExpression()
        {
            return _expression;
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica AND.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<Rol> And(ISpecification<Rol> other)
        {
            return new RolSpecification(Expression.Lambda<Func<Rol, bool>>(
                Expression.AndAlso(_expression.Body, other.ToExpression().Body),
                _expression.Parameters));
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica OR.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<Rol> Or(ISpecification<Rol> other)
        {
            return new RolSpecification(Expression.Lambda<Func<Rol, bool>>(
                Expression.OrElse(_expression.Body, other.ToExpression().Body),
                _expression.Parameters));
        }

        /// <summary>
        /// Invierte la especificación actual utilizando una operación lógica NOT.
        /// </summary>
        /// <returns>Una nueva especificación que representa la inversión.</returns>
        public ISpecification<Rol> Not()
        {
            return new RolSpecification(Expression.Lambda<Func<Rol, bool>>(
                Expression.Not(_expression.Body),
                _expression.Parameters));
        }

        /// <summary>
        /// Crea una especificación para filtrar roles por su nombre.
        /// </summary>
        /// <param name="name">El nombre del rol.</param>
        /// <returns>Una especificación que filtra por nombre.</returns>
        public static RolSpecification ByName(string name)
        {
            return new RolSpecification(rol => rol.Name == name);
        }
    }
}