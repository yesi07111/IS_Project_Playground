using Playground.Application.Queries.Dtos;

namespace Playground.Application.Queries.Responses;

public record ListActivityResponse(IEnumerable<object> Result);