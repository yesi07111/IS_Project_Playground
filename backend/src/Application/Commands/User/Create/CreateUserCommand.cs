using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.User.Create;

public record CreateUserCommand : ICommand<GenericResponse>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}