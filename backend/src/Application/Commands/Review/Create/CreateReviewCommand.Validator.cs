using FastEndpoints;
using FluentValidation;

namespace Playground.Application.Commands.Review.Create;

public class CreateReviewCommandValidator : Validator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.Rating)
            .GreaterThan(0)
            .WithMessage("La calificación debe ser mayor que 0.")
            .LessThanOrEqualTo(5)
            .WithMessage("La calificación debe ser menor o igual a 5.");

        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage("El comentario no puede estar vacío.")
            .NotNull()
            .WithMessage("El comentario no puede ser nulo.")
            .MaximumLength(500)
            .WithMessage("El comentario no puede tener más de 500 caracteres.");
    }
}