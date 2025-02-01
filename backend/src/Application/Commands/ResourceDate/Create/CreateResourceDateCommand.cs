using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.ResourceDate.Create;

public record CreateResourceDateCommand : ICommand<GenericResponse>
{
    public string ResourceId { get; init; } = string.Empty;
    public string? Date { get; init; }
    public int UsageFrequency { get; init; }
}