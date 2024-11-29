using FastEndpoints;
using Playground.Application.Queries.Dtos;

namespace Playground.Application.Queries.CheckEmail;

public record CheckEmailQuery : ICommand<CheckEmailResponse>
{
    public string Token { get; init; } = string.Empty;
    public string Id { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
}