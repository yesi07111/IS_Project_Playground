using System.Linq.Expressions;

namespace Playground.Domain.Specifications.BaseSpecifications
{
    /// <summary>
    /// Proporciona métodos para crear expresiones de comparación de fechas para entidades de tipo genérico.
    /// </summary>
    /// <typeparam name="T">El tipo de entidad para el cual se crea la especificación.</typeparam>
    public static class DateTimeSpecification<T>
    {
        /// <summary>
        /// Crea una expresión de comparación de fechas basada en el tipo de comparación especificado.
        /// </summary>
        /// <param name="dateSelector">Selector de la propiedad de fecha de la entidad.</param>
        /// <param name="date">La fecha a comparar.</param>
        /// <param name="comparison">El tipo de comparación a realizar ("greater", "greater-or-equal", "less", "less-or-equal", "equal").</param>
        /// <returns>Una expresión lambda que representa la comparación de fechas.</returns>
        public static Expression<Func<T, bool>> CreateDateComparisonExpression(
            Expression<Func<T, DateTime>> dateSelector, DateTime date, string comparison)
        {
            switch (comparison)
            {
                case "greater":
                    return Expression.Lambda<Func<T, bool>>(
                        Expression.GreaterThan(dateSelector.Body, Expression.Constant(date)),
                        dateSelector.Parameters);
                case "greater-or-equal":
                    return Expression.Lambda<Func<T, bool>>(
                        Expression.GreaterThanOrEqual(dateSelector.Body, Expression.Constant(date)),
                        dateSelector.Parameters);
                case "less":
                    return Expression.Lambda<Func<T, bool>>(
                        Expression.LessThan(dateSelector.Body, Expression.Constant(date)),
                        dateSelector.Parameters);
                case "less-or-equal":
                    return Expression.Lambda<Func<T, bool>>(
                        Expression.LessThanOrEqual(dateSelector.Body, Expression.Constant(date)),
                        dateSelector.Parameters);
                default:
                    return Expression.Lambda<Func<T, bool>>(
                        Expression.Equal(dateSelector.Body, Expression.Constant(date)),
                        dateSelector.Parameters);
            }
        }

        /// <summary>
        /// Crea una expresión de comparación de fechas basada en el tipo de comparación especificado, considerando fechas nulas.
        /// </summary>
        /// <param name="dateSelector">Selector de la propiedad de fecha de la entidad.</param>
        /// <param name="date">La fecha a comparar.</param>
        /// <param name="comparison">El tipo de comparación a realizar ("greater", "greater-or-equal", "less", "less-or-equal", "equal").</param>
        /// <returns>Una expresión lambda que representa la comparación de fechas.</returns>
        public static Expression<Func<T, bool>> CreateNullableDateComparisonExpression(
            Expression<Func<T, DateTime?>> dateSelector, DateTime? date, string comparison)
        {
            if (date == null)
            {
                return entity => !dateSelector.Compile()(entity).HasValue;
            }

            switch (comparison)
            {
                case "greater":
                    return entity => dateSelector.Compile()(entity) > date;
                case "greater-or-equal":
                    return entity => dateSelector.Compile()(entity) >= date;
                case "less":
                    return entity => dateSelector.Compile()(entity) < date;
                case "less-or-equal":
                    return entity => dateSelector.Compile()(entity) <= date;
                default:
                    return entity => dateSelector.Compile()(entity) == date;
            }
        }
    }
}