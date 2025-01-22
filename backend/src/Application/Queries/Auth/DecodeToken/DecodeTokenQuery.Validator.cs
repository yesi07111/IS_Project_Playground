using FastEndpoints;
using FluentValidation;
using System.IdentityModel.Tokens.Jwt;

namespace Playground.Application.Queries.Auth.DecodeToken;

public class DecodeTokenQueryValidator : Validator<DecodeTokenQuery>
{
    public DecodeTokenQueryValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("El token es obligatorio.")
            .Must(BeAValidJwtToken).WithMessage("Token JWT inválido.");
    }

    private bool BeAValidJwtToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        return handler.CanReadToken(token);
    }
}