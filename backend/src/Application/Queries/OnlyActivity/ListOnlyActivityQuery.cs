using FastEndpoints;
using Playground.Application.Queries.Responses;

namespace Playground.Application.Queries.OnlyActivity;

public record ListOnlyActivityQuery : ICommand<ListOnlyActivityResponse>
{
    public string? GenericProperty { get; init; }
};