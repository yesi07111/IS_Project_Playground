using Playground.Application.Factories;
using Playground.Application.Responses;
using Playground.Domain.Entities;
using Playground.Domain.Specifications;

namespace Playground.Application.Queries.HomePage;

public class GetHomePageInfoQueryHandler(IRepositoryFactory repositoryFactory)
{
    public async Task<GetHomePageInfoResponse> ExecuteAsync(CancellationToken ct = default)
    {
        var userRepository = repositoryFactory.CreateRepository<Domain.Entities.Auth.User>();
        var reservationRepository = repositoryFactory.CreateRepository<Domain.Entities.Reservation>();
        var activityDateRepository = repositoryFactory.CreateRepository<ActivityDate>();
        var reviewRepository = repositoryFactory.CreateRepository<Domain.Entities.Review>();

        var firstDayOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        var userSpecification = UserSpecification.ByCreatedAt(firstDayOfMonth, "greater-or-equal")
            .And(UserSpecification.ByCreatedAt(lastDayOfMonth, "less-or-equal"));
        var visitors = (await userRepository.GetBySpecificationAsync(userSpecification)).Count();

        var reservationSpecification = ReservationSpecification.ByCreatedAt(firstDayOfMonth, "greater-or-equal")
            .And(ReservationSpecification.ByCreatedAt(DateTime.UtcNow, "less-or-equal"));
        var reservations = await reservationRepository.GetBySpecificationAsync(reservationSpecification);

        foreach (var reservation in reservations)
        {
            visitors += reservation.AmmountOfChildren;
        }

        var activityDateSpecification = ActivityDateSpecification.ByDateMoreOrEqual(DateTime.UtcNow).And(ActivityDateSpecification.ByPending(false));
        var activeActivities = (await activityDateRepository.GetBySpecificationAsync(activityDateSpecification)).Count();

        var score = reviewRepository.GetAllAsync().Result.Average(r => r.Score);

        return new GetHomePageInfoResponse(visitors, activeActivities, score);
    }
}