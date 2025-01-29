using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Playground.Application.Commands.User.Create;

public class CreateUserValidator : Validator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre no puede estar vacío.")
            .NotNull().WithMessage("El nombre no puede ser nulo.")
            .Length(3, 15).WithMessage("El nombre debe tener entre 3 y 15 caracteres.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Los apellidos no pueden estar vacíos.")
            .NotNull().WithMessage("Los apellidos no pueden ser nulos.")
            .Length(3, 30).WithMessage("Los apellidos deben tener entre 3 y 30 caracteres.");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("El nombre de usuario no puede estar vacío.")
            .NotNull().WithMessage("El nombre de usuario no puede ser nulo.")
            .Matches("^(?=[a-zA-Z0-9._]{3,15}$)(?!.*[_.]{2})[^_.].*[^_.]$")
                .WithMessage("Nombre de usuario no válido.")
            .MustAsync(async (usn, ct) =>
            {
                var scope = CreateScope();
                var userManager = scope.Resolve<UserManager<Domain.Entities.Auth.User>>();
                var usr = await userManager.FindByNameAsync(usn);
                return usr == null;
            }).WithMessage("El nombre de usuario ya está en uso.");
        RuleFor(x => x.Password)
            .NotNull().WithMessage("La contraseña no debe estar vacía.")
            .NotEmpty().WithMessage("La contraseña no debe ser nula.");
        RuleFor(x => x.Email)
            .NotNull().WithMessage("El correo electrónico no puede estar vacío.")
            .NotEmpty().WithMessage("El correo electrónico no puede ser nulo.")
            .EmailAddress().WithMessage("Correo electrónico no válido.")
            .MustAsync(async (email, ct) =>
            {
                var scope = CreateScope();
                var userManager = scope.Resolve<UserManager<Domain.Entities.Auth.User>>();
                var usr = await userManager.FindByEmailAsync(email);
                return usr == null;
            }).WithMessage("El correo electrónico ya está en uso.");
        RuleFor(x => x.Password)
            .NotNull().WithMessage("La contraseña no debe estar vacía.")
            .NotEmpty().WithMessage("La contraseña no debe ser nula.")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.")
            .Must(IsStrongPassword).WithMessage("La contraseña no es fuerte, cámbiala por favor.");
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