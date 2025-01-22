using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Queries.Auth.ConfirmEmail;

public record ConfirmEmailQuery(string Username, string Code) : ICommand<UserActionResponse>;