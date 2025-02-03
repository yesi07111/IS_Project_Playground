namespace Playground.Application.Responses;

 /// <summary>
/// Representa la respuesta para una lista de reseñas.
/// </summary>
public record ListReviewResponse(IEnumerable<object> Result);