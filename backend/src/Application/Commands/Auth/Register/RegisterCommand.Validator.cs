using Playground.Domain.Entities.Auth;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Playground.Application.Commands.Auth.Register;

public class RegisterCommandValidator : Validator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotNull().NotEmpty().WithMessage("Username cannot be null or empty")
            .Length(3, 15).WithMessage("User name must be between 3 and 15 characters long")
            .Must(x => x.Trim().Length > 0).WithMessage("Username cannot be whitespace characters")
            .Matches("^(?=[a-zA-Z0-9._]{3,15}$)(?!.*[_.]{2})[^_.].*[^_.]$")
                .WithMessage("Username not valid")
            .MustAsync(async (usn, ct) =>
            {
                var scope = CreateScope();
                var userManager = scope.Resolve<UserManager<User>>();
                var usr = await userManager.FindByNameAsync(usn);
                Console.WriteLine(usr?.UserName);
                return usr == null;
            }).WithMessage("Username is already used");

        RuleFor(x => x.Email)
            .NotEmpty().NotNull().WithMessage("Email cannot be empty or null")
            .EmailAddress().WithMessage("Not valid Email provided");

        RuleFor(x => x.Password)
            .NotEmpty().NotNull().WithMessage("Password must not be null or empty")
            .MinimumLength(6).WithMessage("Password must have at least 6 characters")
            .Must(IsStrongPassword).WithMessage("Password not strong, change it please");
    }

    private bool IsStrongPassword(string password)
    {
        if (!password.Any(c => char.IsDigit(c)))
            return false;

        if (!password.Any(c => char.IsUpper(c)))
            return false;

        if (!password.Any(c => char.IsLower(c)))
            return false;

        return true;
    }
}