namespace Playground.Application.Responses;

public record UserActionResponse(Guid Id, string Username, string Token, string RolName);