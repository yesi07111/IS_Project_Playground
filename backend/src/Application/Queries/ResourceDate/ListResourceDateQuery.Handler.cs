using FastEndpoints;
using Playground.Application.Factories;
using Playground.Application.Queries.Dtos;
using Playground.Application.Queries.Responses;
using Playground.Application.Services;

namespace Playground.Application.Queries.ResourceDate;

public class ListResourceDateQueryHandler : CommandHandler<ListResourceDateQuery, ListResourceDateResponse>
{
    private readonly IRepositoryFactory _repositoryFactory;

    public ListResourceDateQueryHandler(IRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }
    public override async Task<ListResourceDateResponse> ExecuteAsync(ListResourceDateQuery query, CancellationToken ct = default)
    {
        var resourceDateRepository = _repositoryFactory.CreateRepository<Domain.Entities.ResourceDate>();

        IEnumerable<object> resourceDates = await resourceDateRepository.GetAllAsync(r => r.Resource);
        var resourceDateDtos = new List<object>();

        foreach (Domain.Entities.ResourceDate resource in resourceDates)
        {
            var resourceDto = new ResourceDateDto
            {
                Id = resource.Resource.Id,
                Name = resource.Resource.Name,
                Date = resource.Date.ToDateTime(TimeOnly.MinValue),
                UseFrequency = resource.UseFrequency,
            };
            resourceDateDtos.Add(resourceDto);
        }
        resourceDates = resourceDateDtos;
        return new ListResourceDateResponse(resourceDates);
    }
}