using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Commands.Auth.Register;

public record RegisterCommand(string FirstName, string LastName, string Username, string Password, string Email, string Rol) : ICommand<UserCreationResponse>;