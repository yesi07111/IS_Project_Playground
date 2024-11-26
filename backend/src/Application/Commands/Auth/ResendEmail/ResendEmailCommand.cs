using Playground.Application.Commands.Dtos;
using FastEndpoints;

namespace Playground.Application.Commands.Auth.ResendEmail;

public record ResendEmailCommand(string Username) : ICommand<UserCreationResponse>;