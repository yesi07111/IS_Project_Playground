using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Commands.Auth.GoogleAccess;

public record GoogleAccessCommand(string FirstName, string LastName, string Username, string ImageUrl, string Email, string IsConfirmed, string Rol, string Action) : ICommand<UserActionResponse>;