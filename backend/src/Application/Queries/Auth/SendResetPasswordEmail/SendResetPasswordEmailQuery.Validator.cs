using FastEndpoints;
using FluentValidation;
using Playground.Application.Queries.Auth.SendResetPasswordEmail;

namespace Playground.Application.Validators.Auth;

public class SendResetPasswordEmailQueryValidator : Validator<SendResetPasswordEmailQuery>
{
    public SendResetPasswordEmailQueryValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage("El identificador no puede estar vacío.")
            .NotNull().WithMessage("El identificador no puede ser nulo.")
            .Must(IsValidIdentifier).WithMessage("El identificador debe ser un nombre de usuario o un correo electrónico válido.");
    }

    private bool IsValidIdentifier(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
        {
            return false;
        }

        return IsValidUsername(identifier) || IsValidEmail(identifier);
    }

    private bool IsValidUsername(string username)
    {
        return username.Length >= 3 && username.Length <= 15 &&
               System.Text.RegularExpressions.Regex.IsMatch(username, "^(?=[a-zA-Z0-9._]{3,15}$)(?!.*[_.]{2})[^_.].*[^_.]$");
    }

    private bool IsValidEmail(string email)
    {
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
}