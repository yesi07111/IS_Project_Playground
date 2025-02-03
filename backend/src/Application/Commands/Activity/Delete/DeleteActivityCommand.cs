using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.Activity.Delete;

public record DeleteActivityCommand : ICommand<GenericResponse>
{
    public string UseCase {get;init;} = string.Empty;
    public string ActivityId { get; init; } = string.Empty;
    public string ActivityDateId { get; init; } = string.Empty;
}