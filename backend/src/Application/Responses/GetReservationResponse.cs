using Playground.Application.Dtos;

namespace Playground.Application.Responses;

/// <summary>
/// Representa la respuesta para una reserva.
/// </summary>
public record GetReservationResponse(ReservationDto Result);