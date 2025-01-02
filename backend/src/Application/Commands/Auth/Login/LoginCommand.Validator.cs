using FastEndpoints;
using FluentValidation;

namespace Playground.Application.Commands.Auth.Login;

public class LoginCommandValidator : Validator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage("El nombre de usuario o correo electrónico no puede ser vacío.")
            .NotNull().WithMessage("El nombre de usuario o correo electrónico no puede ser nulo.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña no debe ser vacía.")
            .NotNull().WithMessage("La contraseña no debe ser nula.");
    }
}