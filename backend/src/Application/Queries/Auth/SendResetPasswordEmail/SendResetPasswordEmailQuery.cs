using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Queries.Auth.SendResetPasswordEmail;

public record SendResetPasswordEmailQuery(string Identifier) : ICommand<UserActionResponse>;