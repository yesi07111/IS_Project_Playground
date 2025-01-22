using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Commands.DeleteFailUser;

public record DeleteFailUserCommand(string Id, string FirstName, string LastName, string Username, string Email, string UserType) : ICommand<DeleteFailUserResponse>;