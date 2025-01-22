using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Commands.Activity.Reservation;

public record ReserveActivityCommand(int Amount, string Comments, string UserId, string ActivityId) : ICommand<ReservationCreationResponse>;