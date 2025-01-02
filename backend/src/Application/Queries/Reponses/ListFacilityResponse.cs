using Playground.Application.Queries.Dtos;

namespace Playground.Application.Queries.Responses;

public record ListFacilityResponse(IEnumerable<object> Result);