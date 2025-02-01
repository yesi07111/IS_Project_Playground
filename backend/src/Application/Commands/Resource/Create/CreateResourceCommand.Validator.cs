using FastEndpoints;
using FluentValidation;

namespace Playground.Application.Commands.Resource.Create;

public class CreateResourceCommandValidator : Validator<CreateResourceCommand>
{
    public CreateResourceCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre no puede estar vacío.")
            .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("El tipo no puede estar vacío.")
            .MinimumLength(3).WithMessage("El tipo debe tener al menos 3 caracteres.");
    }
}