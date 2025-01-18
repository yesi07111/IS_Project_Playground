using System.Data;
using FastEndpoints;
using FluentValidation;

namespace Playground.Application.Commands.Activity.Post;

public class PostActivityCommandValidator : Validator<PostActivityCommand>
{
    public PostActivityCommandValidator()
    {
        RuleFor(x => x.RecommendedAge)
            .GreaterThan(1)
            .WithMessage("La edad recomendada debe ser mayor que 1.");
    }
}