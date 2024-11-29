using Playground.Domain.Entities.Auth;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Playground.Application.Commands.Auth.ConfirmEmail;

public class ConfirmEmailCommandValidator : Validator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("El nombre de usuario no puede estar vacío.")
            .NotNull().WithMessage("El nombre de usuario no puede ser nulo.")
            .MustAsync(async (usn, ct) =>
            {
                var scope = CreateScope();
                var userManager = scope.Resolve<UserManager<User>>();

                return await userManager.FindByNameAsync(usn) != null;
            }).WithMessage("Usuario no encontrado.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("El código no puede estar vacío.")
            .NotNull().WithMessage("El código no puede ser nulo.");
    }
}