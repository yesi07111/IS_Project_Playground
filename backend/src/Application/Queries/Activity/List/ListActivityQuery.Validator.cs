using FluentValidation;

namespace Playground.Application.Queries.Activity.List;

public class ListActivityQueryValidator : AbstractValidator<ListActivityQuery>
{
    public ListActivityQueryValidator()
    {
        // Validar MinAge
        RuleFor(query => query.MinAge)
            .InclusiveBetween(2, 16)
            .When(query => query.MinAge.HasValue)
            .WithMessage("MinAge debe estar entre 2 y 16.");

        // Validar MaxAge
        RuleFor(query => query.MaxAge)
            .InclusiveBetween(3, 17)
            .When(query => query.MaxAge.HasValue)
            .WithMessage("MaxAge debe estar entre 3 y 17.");

        // Validar que MinAge sea menor que MaxAge
        RuleFor(query => query)
            .Must(query => !query.MinAge.HasValue || !query.MaxAge.HasValue || query.MinAge <= query.MaxAge)
            .WithMessage("MinAge debe ser menor o igual a MaxAge.");
    }
}
