using Playground.Application.Queries.Dtos;

namespace Playground.Application.Queries.Responses;

public record ListResourceResponse(IEnumerable<object> Result);