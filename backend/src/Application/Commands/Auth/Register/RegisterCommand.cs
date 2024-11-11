using Playground.Application.Commands.Dtos;
using FastEndpoints;

namespace Playground.Application.Commands.Auth.Register;

public record RegisterCommand(string Username, string Password, string Email, string[] Roles) : ICommand<UserCreationResponse>;