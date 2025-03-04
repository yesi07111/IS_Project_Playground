using FastEndpoints;
using Playground.Application.Factories;
using Playground.Application.Dtos;
using Playground.Application.Responses;
using Playground.Domain.Specifications;

namespace Playground.Application.Queries.Reservation.Get;

public class GetReservationQueryHandler(IRepositoryFactory _repositoryFactory) : CommandHandler<GetReservationQuery, GetReservationResponse>
{
    public override async Task<GetReservationResponse> ExecuteAsync(GetReservationQuery query, CancellationToken ct = default)
    {
        var reservationRepository = _repositoryFactory.CreateRepository<Domain.Entities.Reservation>();

        if (string.IsNullOrEmpty(query.Id))
        {
            return new GetReservationResponse(null!);
        }

        var reservationSpecification = ReservationSpecification.ByParent(query.UserId).And(ReservationSpecification.ByActivityDate(Guid.Parse(query.Id)));
        var reservations = await reservationRepository.GetBySpecificationAsync(reservationSpecification, r => r.Parent, r => r.ActivityDate, r => r.ActivityDate.Activity);

        if (reservations is null)
        {
            return new GetReservationResponse(null!);
        }

        var reservation = reservations.FirstOrDefault(r => r.ReservationState != "Cancelada") ?? reservations.OrderByDescending(r => r.CreatedAt).FirstOrDefault();
        if (reservation is null)
        {
            return new GetReservationResponse(null!);
        }

        var reservationDto = new ReservationDto
        {
            ReservationId = reservation.Id.ToString(),
            FirstName = reservation.Parent.FirstName,
            LastName = reservation.Parent.LastName,
            UserName = reservation.Parent.UserName!,
            ActivityId = reservation.ActivityDate.Id.ToString(),
            ActivityName = reservation.ActivityDate.Activity.Name,
            ActivityDate = reservation.ActivityDate.DateTime.ToString(),
            Amount = reservation.AmmountOfChildren,
            Comments = reservation.AdditionalComments,
            State = reservation.ReservationState,
            ActivityRecommendedAge = reservation.ActivityDate.Activity.RecommendedAge,
        };

        return new GetReservationResponse(reservationDto);
    }
}
