using Playground.Application.Dtos;

namespace Playground.Application.Responses;

public record ListReservationResponse(IEnumerable<ReservationDto> Result);