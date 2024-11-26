using FastEndpoints;
using Playground.Application.Commands.Dtos;

namespace Playground.Application.Commands.CleanUp;

public record CleanUpUnverifiedUsersCommand : ICommand<CleanUpUnverifiedUsersResponse>
{
    public string PublicProperty { get; init; } = string.Empty;
}