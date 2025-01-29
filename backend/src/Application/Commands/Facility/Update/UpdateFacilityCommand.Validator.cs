using FastEndpoints;
using FluentValidation;

namespace Playground.Application.Commands.Facility.Update;

public class UpdateFacilityCommandValidator: Validator<UpdateFacilityCommand>
{
    public UpdateFacilityCommandValidator()
    {
        RuleFor(x => x.MaximumCapacity)
            .Must(x => x > 0).WithMessage("La capacidad máxima debe ser mayor que cero");
    }
}