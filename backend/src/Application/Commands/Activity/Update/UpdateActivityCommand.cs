using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.Activity.Update;

public record UpdateActivityCommand : ICommand<GenericResponse>
{
    public string UseCase { get; init; } = string.Empty;
    public string ActivityId { get; init; } = string.Empty;
    public string ActivityDateId { get; init; } = string.Empty;
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