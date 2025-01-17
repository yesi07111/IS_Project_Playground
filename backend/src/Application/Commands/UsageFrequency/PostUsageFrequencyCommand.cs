using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.UsageFrequency;

public record PostUsageFrequencyCommand : ICommand<GenericResponse>
{
    public string ResourceId { get; init; } = string.Empty;
    public string? Date { get; init; }
    public int UsageFrequency { get; init; }
}