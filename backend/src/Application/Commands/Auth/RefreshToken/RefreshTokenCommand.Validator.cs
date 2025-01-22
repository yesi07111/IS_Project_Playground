using FastEndpoints;
using FluentValidation;
using System.IdentityModel.Tokens.Jwt;

namespace Playground.Application.Commands.Auth.RefreshToken;

public class RefreshTokenCommandValidator : Validator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
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