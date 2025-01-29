using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.Activity.Create;

public record CreateActivityCommand : ICommand<GenericResponse>
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Educator { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string? Date { get; init; } 
    public string? Time { get; init; } 
    public int RecommendedAge { get; init; } 
    public string Facility { get; init; } = string.Empty;
    public bool Pending { get; init; } = false;
    public bool Private { get; init; } = false;
}