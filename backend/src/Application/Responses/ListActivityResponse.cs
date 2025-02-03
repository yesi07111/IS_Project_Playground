namespace Playground.Application.Responses;

/// <summary>
/// Representa la respuesta para una lista de actividades.
/// </summary>
public record ListActivityResponse(IEnumerable<object> Result);