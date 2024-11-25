using Playground.Domain.Entities.Auth;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Playground.Application.Commands.Auth.Login;

public class LoginCommandValidator : Validator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Identifier)
            .NotNull().WithMessage("El nombre de usuario o correo electrónico no puede ser nulo.")
            .NotEmpty().WithMessage("El nombre de usuario o correo electrónico no puede ser vacío.");

        RuleFor(x => x.Password)
            .NotNull().WithMessage("La contraseña no debe ser vacía")
            .NotEmpty().WithMessage("La contraseña no debe ser nula.");
    }
}