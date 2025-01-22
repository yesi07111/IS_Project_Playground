using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Commands.Auth.RefreshToken;

public record RefreshTokenCommand(string Token, string UserId) : ICommand<UserActionResponse>;