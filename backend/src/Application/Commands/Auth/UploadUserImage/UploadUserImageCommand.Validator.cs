using FastEndpoints;
using FluentValidation;

namespace Playground.Application.Commands.Auth.UploadUserImage;

public class UploadUserImageCommandValidator : Validator<UploadUserImageCommand>
{
    public UploadUserImageCommandValidator()
    {
        RuleFor(x => x.Username)
                    .NotEmpty().WithMessage("El nombre de usuario no puede estar vacío.")
                    .NotNull().WithMessage("El nombre de usuario no puede ser nulo.")
                    .Length(3, 15).WithMessage("El nombre de usuario debe tener entre 3 y 15 caracteres.")
                    .Must(x => x.Trim().Length > 0).WithMessage("El nombre de usuario no puede ser solo espacios en blanco.")
                    .Matches("^(?=[a-zA-Z0-9._]{3,15}$)(?!.*[_.]{2})[^_.].*[^_.]$")
                        .WithMessage("Nombre de usuario no válido.");
    }
}