using FastEndpoints;
using FluentValidation;

namespace Playground.Application.Commands.ResourceDate;

public class CreateResourceDateCommandValidator : Validator<CreateResourceDateCommand>
{
    public CreateResourceDateCommandValidator()
    {
        RuleFor(x=> x.UsageFrequency)
            .Must(x => x > 0).WithMessage("La frecuencia de uso debe ser mayor que cero");
    }
}