using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.Facility.Create;

public record CreateFacilityCommand : ICommand<GenericResponse>
{
    public string Name { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string UsagePolicy { get; init; } = string.Empty;
    public int MaximumCapacity { get; init; } 
}