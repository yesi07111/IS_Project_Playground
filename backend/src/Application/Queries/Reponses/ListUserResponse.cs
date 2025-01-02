using Playground.Application.Queries.Dtos;

namespace Playground.Application.Queries.Responses;

public record ListUserResponse(IEnumerable<object> Users);