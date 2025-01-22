using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Queries.User.List;

public record ListUserQuery : ICommand<ListUserResponse>
{
    public string? Username { get; init; }
    public string? Email { get; init; }
    public string? EmailConfirmed { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Rol { get; init; }
    public bool? MarkDeleted { get; init; }
    public string UseCase { get; init; } = string.Empty;
}