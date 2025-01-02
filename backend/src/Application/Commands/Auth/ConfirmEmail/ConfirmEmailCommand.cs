using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.Auth.ConfirmEmail;

public record ConfirmEmailCommand(string UserName, string Code) : ICommand<UserActionResponse>;