
using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Queries.User.Get;

public record GetUserQuery : ICommand<GetUserResponse>
{
    public string Id { get; init; } = string.Empty;
    public string UseCase { get; init; } = string.Empty;
}