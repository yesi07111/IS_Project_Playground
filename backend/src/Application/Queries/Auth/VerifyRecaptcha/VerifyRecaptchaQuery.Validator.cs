using FastEndpoints;
using FluentValidation;

namespace Playground.Application.Queries.Auth.VerifyRecaptcha;

public class VerifyRecaptchaQueryValidator : Validator<VerifyRecaptchaQuery>
{
    public VerifyRecaptchaQueryValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("El token de CAPTCHA no debe ser vacío.")
            .NotNull().WithMessage("El token de CAPTCHA no debe ser nulo.");
    }
}