using FluentValidation;

namespace Playground.Application.Queries.Resource.List;

public class ListResourceQueryValidator : AbstractValidator<ListResourceQuery>
{
    public ListResourceQueryValidator()
    {
        RuleFor(query => query)
            .Must(query=> query.MinUseFrequency > 0 && query.MaxUseFrequency > 0)
            .WithMessage("Min y Max UseFrequency deben ser mayores que 0");
        RuleFor(query => query)
            .Must(query => !query.MinUseFrequency.HasValue || !query.MaxUseFrequency.HasValue || query.MinUseFrequency <= query.MaxUseFrequency)
            .WithMessage("MinUseFrequency debe ser menor o igual a MaxUseFrequency.");
    }
}