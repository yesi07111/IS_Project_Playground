using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Commands.User.Update;

public record UpdateUserCommand(string Id, string FirstName, string LastName, string Username, string OldPassword, string Email, string Password) : ICommand<UpdateUserResponse>;