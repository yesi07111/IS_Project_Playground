using Playground.Application.Commands.Dtos;
using FastEndpoints;

namespace Playground.Application.Commands.Auth.ConfirmEmail;

public record ConfirmEmailCommand(string UserName, string Code) : ICommand<UserActionResponse>;