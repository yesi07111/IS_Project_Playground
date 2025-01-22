using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Playground.Application.Queries.Auth.ResendEmail;

public class ResendEmailQueryValidator : Validator<ResendEmailQuery>
{
    public ResendEmailQueryValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario no debe estar vacío")
            .NotNull().WithMessage("El nombre de usuario no debe ser nulo")
            .MustAsync(async (usn, ct) =>
            {
                var scope = CreateScope();
                var userManager = scope.Resolve<UserManager<Domain.Entities.Auth.User>>();

                return await userManager.FindByNameAsync(usn) != null;
            }).WithMessage("El nombre de usuario no existe");
    }
}