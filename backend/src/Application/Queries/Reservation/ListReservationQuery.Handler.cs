using FastEndpoints;
using Playground.Application.Factories;
using Playground.Application.Dtos;
using Playground.Application.Responses;
using Playground.Domain.Specifications;
using Playground.Domain.SmartEnum;

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
        IEnumerable<Domain.Entities.Reservation> reservations;

        if (string.IsNullOrEmpty(query.Id))
        {
            reservations = await reservationRepository.GetAllAsync(r => r.ActivityDate, r => r.ActivityDate.Activity, r => r.Parent);
        }
        else
        {
            var reservationSpecification = ReservationSpecification.ByParent(query.Id).AndNot(ReservationSpecification.ByReservationState(ReservationStateSmartEnum.Cancelada.Name));
            reservations = await reservationRepository.GetBySpecificationAsync(reservationSpecification, r => r.ActivityDate, r => r.ActivityDate.Activity, r => r.Parent);
        }

        var reservationsDtos = reservations.Select(r => new ReservationDto
        {
            ReservationId = r.Id.ToString(),
            FirstName = r.Parent.FirstName,
            LastName = r.Parent.LastName,
            UserName = r.Parent.UserName!,
            ActivityId = r.ActivityDate.Id.ToString(),
            ActivityName = r.ActivityDate.Activity.Name,
            ActivityDate = r.ActivityDate.DateTime.ToString(),
            Amount = r.AmmountOfChildren,
            Comments = r.AdditionalComments,
            State = r.ReservationState
        });

        return new ListReservationResponse(reservationsDtos);
    }
}
