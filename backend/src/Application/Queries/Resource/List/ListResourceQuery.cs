using FastEndpoints;
using Playground.Application.Queries.Responses;

namespace Playground.Application.Queries.Resource.List;

public record ListResourceQuery : ICommand<ListResourceResponse>
{
    public string? UseCase { get; init; }
    public string? Name { get; init; }
    public int? MinUseFrequency { get; init; }
    public int? MaxUseFrequency { get; init; }
    public string? Condition { get; init; }
    public string? FacilityTypes { get; init; }
    public string? ResourceTypes { get; init; }
}