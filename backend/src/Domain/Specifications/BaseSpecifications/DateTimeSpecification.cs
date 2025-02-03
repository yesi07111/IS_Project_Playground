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
            return comparison switch
            {
                "greater" => Expression.Lambda<Func<T, bool>>(
                                        Expression.GreaterThan(dateSelector.Body, Expression.Constant(date)),
                                        dateSelector.Parameters),
                "greater-or-equal" => Expression.Lambda<Func<T, bool>>(
                                        Expression.GreaterThanOrEqual(dateSelector.Body, Expression.Constant(date)),
                                        dateSelector.Parameters),
                "less" => Expression.Lambda<Func<T, bool>>(
                                        Expression.LessThan(dateSelector.Body, Expression.Constant(date)),
                                        dateSelector.Parameters),
                "less-or-equal" => Expression.Lambda<Func<T, bool>>(
                                        Expression.LessThanOrEqual(dateSelector.Body, Expression.Constant(date)),
                                        dateSelector.Parameters),
                _ => Expression.Lambda<Func<T, bool>>(
                                        Expression.Equal(dateSelector.Body, Expression.Constant(date)),
                                        dateSelector.Parameters),
            };
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

            return comparison switch
            {
                "greater" => entity => dateSelector.Compile()(entity) > date,
                "greater-or-equal" => entity => dateSelector.Compile()(entity) >= date,
                "less" => entity => dateSelector.Compile()(entity) < date,
                "less-or-equal" => entity => dateSelector.Compile()(entity) <= date,
                _ => entity => dateSelector.Compile()(entity) == date,
            };
        }
    }
}