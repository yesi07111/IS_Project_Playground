using FastEndpoints;
using Playground.Application.Queries.Responses;

namespace Playground.Application.Queries.Facility.List;

public record ListFacilityQuery : ICommand<ListFacilityResponse>
{
    public string? Name { get; init; }
    public string? Location { get; init; }
    public string? Type { get; init; }
    public int? MaximumCapacity { get; init; }
    public string? UsagePolicy { get; init; }
    public string UseCase { get; init; } = string.Empty;
}