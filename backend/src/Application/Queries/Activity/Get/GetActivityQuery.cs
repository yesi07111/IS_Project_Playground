
using FastEndpoints;
using Playground.Application.Queries.Responses;

namespace Playground.Application.Queries.Activity.Get;

public record GetActivityQuery : ICommand<GetActivityResponse>
{
    public string Id { get; init; } = string.Empty;
    public string UseCase { get; init; } = string.Empty;
}