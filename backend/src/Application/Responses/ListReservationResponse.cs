using Playground.Application.Dtos;

namespace Playground.Application.Responses;

/// <summary>
/// Representa la respuesta para una lista de reservas.
/// </summary>
public record ListReservationResponse(IEnumerable<ReservationDto> Result);