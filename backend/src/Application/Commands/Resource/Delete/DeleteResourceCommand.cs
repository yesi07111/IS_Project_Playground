using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.Resource.Delete;

public record DeleteResourceCommand : ICommand<GenericResponse>
{
    public string Id { get; init; } = string.Empty;
}