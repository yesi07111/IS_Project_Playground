using Playground.Domain.Entities.Auth;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Playground.Application.Commands.Auth.ConfirmEmail;

public class ConfirmEmailCommandValidator : Validator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username must not be empty")
            .NotNull().WithMessage("Username must not be null")
            .MustAsync(async (usn, ct) =>
            {
                var scope = CreateScope();
                var userManager = scope.Resolve<UserManager<User>>();

                return await userManager.FindByNameAsync(usn) != null;
            }).WithMessage("Username does not exists");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code must not be empty")
            .NotNull().WithMessage("Code must not be null");
    }
}