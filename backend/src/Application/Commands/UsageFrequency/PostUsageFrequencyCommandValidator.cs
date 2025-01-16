using FastEndpoints;
using FluentValidation;

namespace Playground.Application.Commands.UsageFrequency;

public class PostUsageFrequencyCommandValidator : Validator<PostUsageFrequencyCommand>
{
    public PostUsageFrequencyCommandValidator()
    {
        RuleFor(x=> x.UsageFrequency)
            .Must(x => x > 0).WithMessage("La frecuencia de uso debe ser mayor que cero");
    }
}