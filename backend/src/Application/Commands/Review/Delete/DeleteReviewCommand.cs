using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.Review.Delete;

public record DeleteReviewCommand : ICommand<GenericResponse>
{
    public string ReviewId { get; init; } = string.Empty;
    public string UseCase { get; init; } = string.Empty;
}