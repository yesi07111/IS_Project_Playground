using FastEndpoints;
using FluentValidation;
using Playground.Application.Commands.Auth.ResetPassword;

public class ResetPasswordCommandValidator : Validator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage("El identificador no puede estar vacío.")
            .NotNull().WithMessage("El identificador no puede ser nulo.")
            .Must(IsValidIdentifier).WithMessage("El identificador debe ser un nombre de usuario o un correo electrónico válido.");

        RuleFor(x => x.NewPassword)
            .NotNull().WithMessage("La contraseña no debe estar vacía.")
            .NotEmpty().WithMessage("La contraseña no debe ser nula.")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.")
            .Must(IsStrongPassword).WithMessage("La contraseña no es fuerte, cámbiala por favor.");
    }

    private bool IsValidIdentifier(string identifier)
    {
        return IsValidUsername(identifier) || IsValidEmail(identifier);
    }

    private bool IsValidUsername(string username)
    {
        return !string.IsNullOrWhiteSpace(username) &&
               username.Length >= 3 && username.Length <= 15 &&
               System.Text.RegularExpressions.Regex.IsMatch(username, "^(?=[a-zA-Z0-9._]{3,15}$)(?!.*[_.]{2})[^_.].*[^_.]$");
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private bool IsStrongPassword(string password)
    {
        return password.Any(c => char.IsDigit(c)) &&
               password.Any(c => char.IsUpper(c)) &&
               password.Any(c => char.IsLower(c));
    }
}