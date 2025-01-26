using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Queries.Review.List;

public record ListReviewQuery : ICommand<ListReviewResponse>
{
    public string? UserId { get; init; }
    public string UseCase { get; init; } = string.Empty;
}