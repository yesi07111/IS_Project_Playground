namespace Playground.Application.Responses;

/// <summary>
/// Representa la respuesta para una lista de instalaciones.
/// </summary>
public record ListFacilityResponse(IEnumerable<object> Result);