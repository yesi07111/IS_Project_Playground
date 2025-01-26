using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.Review.Update;

public record UpdateReviewCommand : ICommand<GenericResponse>
{
    public string UserId { get; init; } = string.Empty;
    public string ActivityDateId { get; init; } = string.Empty;
    public string Comment { get; init; } = string.Empty;
    public int Rating { get; init; }
}