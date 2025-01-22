using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Queries.Auth.ResendEmail;

public record ResendEmailQuery(string Username) : ICommand<UserCreationResponse>;