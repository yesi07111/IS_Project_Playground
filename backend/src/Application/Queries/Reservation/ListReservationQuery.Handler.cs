using FastEndpoints;
using Playground.Application.Factories;
using Playground.Application.Dtos;
using Playground.Application.Responses;
using Playground.Domain.Specifications;
using Playground.Domain.SmartEnum;
using Playground.Domain.Specifications.BaseSpecifications;

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
        var activityDateRepository = _repositoryFactory.CreateRepository<Domain.Entities.ActivityDate>();
        IEnumerable<Domain.Entities.Reservation> reservations;

        if (string.IsNullOrEmpty(query.Id))
        {
            reservations = await reservationRepository.GetAllAsync(r => r.ActivityDate, r => r.ActivityDate.Activity, r => r.Parent);
        }
        else
        {
            var reservationSpecification = ReservationSpecification.ByParent(query.Id);
            reservations = await reservationRepository.GetBySpecificationAsync(reservationSpecification, r => r.ActivityDate, r => r.ActivityDate.Activity, r => r.Parent);
        }

        ISpecification<Domain.Entities.ActivityDate> activityDateSpecification = new ActivityDateSpecification(r => true);
        foreach (var reservation in reservations)
        {
            activityDateSpecification = activityDateSpecification.Or(ActivityDateSpecification.ById(reservation.ActivityDate.Id));
        }

        var activityDates = await activityDateRepository.GetBySpecificationAsync(activityDateSpecification, r => r.Activity, r => r.Activity.Facility);

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
            State = r.ReservationState,
            ActivityRecommendedAge = r.ActivityDate.Activity.RecommendedAge,
            UsedCapacity = r.ActivityDate.ReservedPlaces,
            Capacity = activityDates.First(ad => ad.Id == r.ActivityDate.Id).Activity.Facility.MaximumCapacity,
        });

        return new ListReservationResponse(reservationsDtos);
    }
}
