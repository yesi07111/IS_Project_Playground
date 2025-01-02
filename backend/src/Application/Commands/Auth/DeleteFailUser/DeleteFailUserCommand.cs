using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.DeleteFailUser;

public record DeleteFailUserCommand(string Id, string FirstName, string LastName, string UserName, string Email, string UserType) : ICommand<DeleteFailUserResponse>;