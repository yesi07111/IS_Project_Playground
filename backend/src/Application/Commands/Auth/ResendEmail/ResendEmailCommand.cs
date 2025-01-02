using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.Auth.ResendEmail;

public record ResendEmailCommand(string Username) : ICommand<UserCreationResponse>;