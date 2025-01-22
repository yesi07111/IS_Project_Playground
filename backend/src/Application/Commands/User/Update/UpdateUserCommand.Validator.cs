using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Commands.User.Update;
using Playground.Domain.Entities.Auth;

public class UpdateUserCommandValidator : Validator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        When(x => !string.IsNullOrEmpty(x.FirstName), () =>
        {
            RuleFor(x => x.FirstName)
                                .Length(3, 15).When(x => !string.IsNullOrEmpty(x.FirstName))
                                .WithMessage("El nombre debe tener entre 3 y 15 caracteres.");

        });

        When(x => !string.IsNullOrEmpty(x.LastName), () =>
        {
            RuleFor(x => x.LastName)
                        .Length(3, 30).When(x => !string.IsNullOrEmpty(x.LastName))
                        .WithMessage("Los apellidos deben tener entre 3 y 30 caracteres.");
        });

        When(x => !string.IsNullOrEmpty(x.Username), () =>
        {
            RuleFor(x => x.Username)
                    .Length(3, 15).When(x => !string.IsNullOrEmpty(x.Username))
                    .WithMessage("El nombre de usuario debe tener entre 3 y 15 caracteres.")
                    .Must(x => x.Trim().Length > 0).When(x => !string.IsNullOrEmpty(x.Username))
                    .WithMessage("El nombre de usuario no puede ser solo espacios en blanco.")
                    .Matches("^(?=[a-zA-Z0-9._]{3,15}$)(?!.*[_.]{2})[^_.].*[^_.]$")
                    .When(x => !string.IsNullOrEmpty(x.Username))
                    .WithMessage("Nombre de usuario no válido.");
        });


        When(x => !string.IsNullOrEmpty(x.Email), () =>
        {
            RuleFor(x => x.Email)
                        .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                        .WithMessage("Correo electrónico no válido.");
        });

        When(x => !string.IsNullOrEmpty(x.Password), () =>
        {
            RuleFor(x => x.Password)
                       .MinimumLength(6).When(x => !string.IsNullOrEmpty(x.Password))
                       .WithMessage("La contraseña debe tener al menos 6 caracteres.")
                       .Must(IsStrongPassword).When(x => !string.IsNullOrEmpty(x.Password))
                       .WithMessage("La contraseña no es fuerte, cámbiala por favor.");
        });

        RuleFor(x => x)
            .Must(x => string.IsNullOrEmpty(x.Password) || !string.IsNullOrEmpty(x.OldPassword))
            .WithMessage("Si se proporciona una nueva contraseña, la contraseña antigua también debe ser proporcionada.")
            .Must(x => string.IsNullOrEmpty(x.OldPassword) || !string.IsNullOrEmpty(x.Password))
            .WithMessage("Si se proporciona la contraseña antigua, la nueva contraseña también debe ser proporcionada.");

        When(x => !string.IsNullOrEmpty(x.OldPassword) && !string.IsNullOrEmpty(x.Password), () =>
        {
            RuleFor(x => x.OldPassword)
                .MustAsync(async (command, oldPassword, cancellation) =>
                {
                    var scope = CreateScope();
                    var userManager = scope.Resolve<UserManager<User>>();
                    var user = await userManager.FindByIdAsync(command.Id.ToString());
                    return user != null && await userManager.CheckPasswordAsync(user, oldPassword);
                }).WithMessage("La contraseña antigua es incorrecta.");
        });
    }

    private bool IsStrongPassword(string password)
    {
        return password.Any(char.IsDigit) && password.Any(char.IsUpper) && password.Any(char.IsLower);
    }
}