using System.Data;
using FastEndpoints;
using FluentValidation;

namespace Playground.Application.Commands.Activity.Create;

public class CreateActivityCommandValidator : Validator<CreateActivityCommand>
{
    public CreateActivityCommandValidator()
    {
        RuleFor(x => x.RecommendedAge)
            .GreaterThan(1)
            .WithMessage("La edad recomendada debe ser mayor que 1.");
    }
}