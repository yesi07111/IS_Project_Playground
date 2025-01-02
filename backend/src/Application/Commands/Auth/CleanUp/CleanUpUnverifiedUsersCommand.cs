using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.CleanUp;

public record CleanUpUnverifiedUsersCommand : ICommand<CleanUpUnverifiedUsersResponse>
{
    public string PublicProperty { get; init; } = string.Empty;
}