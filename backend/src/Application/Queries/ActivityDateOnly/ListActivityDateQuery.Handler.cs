using FastEndpoints;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Playground.Application.Factories;
using Playground.Application.Queries.Dtos;
using Playground.Application.Queries.OnlyActivity;
using Playground.Application.Queries.Responses;
using Playground.Domain.Entities;
using Playground.Domain.Specifications;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Application.Queries.ActivityDateOnly;

public class ListActivityDateQueryHandler : CommandHandler<ListActivityDateQuery, ListActivityDateResponse>
{
    private readonly IRepositoryFactory _repositoryFactory;

    public ListActivityDateQueryHandler(IRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public override async Task<ListActivityDateResponse> ExecuteAsync(ListActivityDateQuery query, CancellationToken ct = default)
    {
        var activityDateRepository = _repositoryFactory.CreateRepository<Domain.Entities.ActivityDate>();

        IEnumerable<object> activityDates = await activityDateRepository.GetBySpecificationAsync(ActivityDateSpecification.ByActivity(Guid.Parse(query.ActivityId)));
        var activityDateDtos = new List<object>();

        foreach (ActivityDate activityDate in activityDates)
        {
            var activityDateDto = new ActivityDateDto
            {
                Id = activityDate.Id,
                DateTime = activityDate.DateTime,
                Pending = activityDate.Pending,
            };
            activityDateDtos.Add(activityDateDto);
        }
        activityDates = activityDateDtos;
        return new ListActivityDateResponse(activityDates);
    }
}