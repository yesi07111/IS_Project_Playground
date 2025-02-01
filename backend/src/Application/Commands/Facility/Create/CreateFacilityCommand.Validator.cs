using FastEndpoints;
using FluentValidation;

namespace Playground.Application.Commands.Facility.Create;

public class CreateFacilityCommandValidator : Validator<CreateFacilityCommand>
{
    public CreateFacilityCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre no puede estar vacío.")
            .NotNull().WithMessage("El nombre no puede ser nulo.")
            .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.");
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("El tipo de instalación no puede estar vacío.")
            .NotNull().WithMessage("El tipo de instalación no puede ser nulo.")
            .MinimumLength(3).WithMessage("El tipo debe tener al menos 3 caracteres.");
        RuleFor(x => x.UsagePolicy)
            .NotEmpty().WithMessage("Las políticas de uso no pueden estar vacías.")
            .NotNull().WithMessage("Las políticas de uso no pueden ser nulas.");
        RuleFor(x => x.MaximumCapacity)
            .Must(x => x > 0).WithMessage("La capacidad máxima debe ser mayor que cero");
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("La ubicación no puede estar vacío.")
            .NotNull().WithMessage("La ubicación no puede ser nulo.")
            .MinimumLength(3).WithMessage("La ubicación debe tener al menos 3 caracteres.");
    }
}