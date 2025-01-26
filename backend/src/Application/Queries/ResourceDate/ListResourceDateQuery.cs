using FastEndpoints;
using Playground.Application.Queries.Responses;

namespace Playground.Application.Queries.ResourceDate;

public record ListResourceDateQuery : ICommand<ListResourceDateResponse>
{
    public string? GenericProperty { get; init; }
}