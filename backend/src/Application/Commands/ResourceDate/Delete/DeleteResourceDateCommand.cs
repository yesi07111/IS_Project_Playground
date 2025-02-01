using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.ResourceDate.Delete;

public record DeleteResourceDateCommand : ICommand<GenericResponse>
{
    public string ResourceId { get; init; } = string.Empty;
    public string Date { get; init; } = string.Empty;
}