using FastEndpoints;
using Playground.Application.Factories;
using Playground.Application.Dtos;
using Playground.Application.Responses;
using Playground.Domain.Specifications;

namespace Playground.Application.Queries.Reservation.List;

public class ListReservationQueryHandler : CommandHandler<ListReservationQuery, ListReservationResponse>
{
    private readonly IRepositoryFactory _repositoryFactory;

    public ListReservationQueryHandler(IRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public override async Task<ListReservationResponse> ExecuteAsync(ListReservationQuery query, CancellationToken ct = default)
    {
        var reservationRepository = _repositoryFactory.CreateRepository<Domain.Entities.Reservation>();
        var reservationSpecification = ReservationSpecification.ByParent(query.Id);
        var reservations = await reservationRepository.GetBySpecificationAsync(reservationSpecification, r => r.ActivityDate, r => r.ActivityDate.Activity);

        var reservationsDtos = reservations.Select(r => new ReservationDto
        {
            ActivityId = r.ActivityDate.Id.ToString(),
            ActivityName = r.ActivityDate.Activity.Name,
            Amount = r.AmmountOfChildren,
            Comments = r.AdditionalComments,
            State = r.ReservationState
        });

        return new ListReservationResponse(reservationsDtos);
    }
}
