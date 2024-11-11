using Playground.Application.Commands.Dtos;
using FastEndpoints;

namespace Playground.Application.Commands.Auth.ConfirmEmail;

public record ConfirmEmailCommand(string Username, string Code) : ICommand<UserActionResponse>;