using FastEndpoints;
using Playground.Application.Commands.Dtos;

namespace Playground.Application.Commands.Auth.Login;

public record LoginCommand(string Username, string Password) : ICommand<UserActionResponse>;