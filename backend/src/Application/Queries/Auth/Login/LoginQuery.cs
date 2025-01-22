using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Queries.Auth.Login;

public record LoginQuery(string Identifier, string Password) : ICommand<UserActionResponse>;