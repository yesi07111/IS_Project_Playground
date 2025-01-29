using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Commands.Reservation.Cancel;

public record CancelReservationCommand(string ActivityId, string UserId) : ICommand<UserActionResponse>;