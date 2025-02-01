using FastEndpoints;
using FluentValidation;

namespace Playground.Application.Commands.Facility.Update;

public class UpdateFacilityCommandValidator: Validator<UpdateFacilityCommand>
{
    public UpdateFacilityCommandValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.");
        RuleFor(x => x.Type)
            .MinimumLength(3).WithMessage("El tipo debe tener al menos 3 caracteres.");
        RuleFor(x => x.MaximumCapacity)
            .Must(x => x > 0).WithMessage("La capacidad máxima debe ser mayor que cero");
        RuleFor(x => x.Location)
            .MinimumLength(3).WithMessage("La ubicación debe tener al menos 3 caracteres.");
    }
}