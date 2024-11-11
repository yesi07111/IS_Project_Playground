using Playground.Domain.Entities.Auth;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Playground.Application.Commands.Auth.Login;

public class LoginCommandValidator : Validator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotNull().NotEmpty().WithMessage("Username cannot be null or empty");

        RuleFor(x => x.Password)
            .NotEmpty().NotNull().WithMessage("Password must not be null or empty");
    }
}