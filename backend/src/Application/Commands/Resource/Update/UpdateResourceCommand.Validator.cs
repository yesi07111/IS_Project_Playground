using FastEndpoints;
using FluentValidation;

namespace Playground.Application.Commands.Resource.Update;

public class UpdateResourceCommandValidator : Validator<UpdateResourceCommand>
{
    public UpdateResourceCommandValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.");

        RuleFor(x => x.Type)
            .MinimumLength(3).WithMessage("El tipo debe tener al menos 3 caracteres.");
    }
}