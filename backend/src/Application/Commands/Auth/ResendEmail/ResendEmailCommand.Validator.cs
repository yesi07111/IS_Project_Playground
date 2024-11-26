using Playground.Domain.Entities.Auth;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Playground.Application.Commands.Auth.ResendEmail;

public class ResendEmailCommandValidator : Validator<ResendEmailCommand>
{
    public ResendEmailCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario no debe estar vacío")
            .NotNull().WithMessage("El nombre de usuario no debe ser nulo")
            .MustAsync(async (usn, ct) =>
            {
                var scope = CreateScope();
                var userManager = scope.Resolve<UserManager<User>>();

                return await userManager.FindByNameAsync(usn) != null;
            }).WithMessage("El nombre de usuario no existe");
    }
}