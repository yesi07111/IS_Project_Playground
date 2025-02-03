using FastEndpoints;
using Playground.Application.Factories;
using Playground.Application.Queries.Dtos;
using Playground.Application.Queries.Responses;

namespace Playground.Application.Queries.OnlyActivity;

public class ListOnlyActivityQueryHandler : CommandHandler<ListOnlyActivityQuery, ListOnlyActivityResponse>
{
    private readonly IRepositoryFactory _repositoryFactory;

    public ListOnlyActivityQueryHandler(IRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public override async Task<ListOnlyActivityResponse> ExecuteAsync(ListOnlyActivityQuery query, CancellationToken ct = default)
    {
        var activityRepository = _repositoryFactory.CreateRepository<Domain.Entities.Activity>();

        IEnumerable<object> activities = await activityRepository.GetAllAsync(r => r.Educator, r => r.Facility);
        var activityDtos = new List<object>();

        foreach (Domain.Entities.Activity activity in activities)
        {
            var activityDto = new OnlyActivityDto
            {
                Id = activity.Id,
                Name = activity.Name,
                Description = activity.Description,
                EducatorFirstName = activity.Educator.FirstName,
                EducatorLastName = activity.Educator.LastName,
                EducatorUserName = activity.Educator.UserName!,
                Type = activity.Type,
                RecommendedAge = activity.RecommendedAge,
                ItsPrivate = activity.ItsPrivate,
                FacilityName = activity.Facility.Name,
            };
            activityDtos.Add(activityDto);
        }
        activities = activityDtos;
        return new ListOnlyActivityResponse(activities);
    }
}