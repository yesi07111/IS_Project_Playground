using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.User.Delete;

public class DeleteUserCommand : ICommand<GenericResponse>
{
    public string Id { get; init; } = string.Empty;
}