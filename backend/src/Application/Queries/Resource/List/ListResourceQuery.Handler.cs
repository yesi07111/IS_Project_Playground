using FastEndpoints;
using Playground.Application.Factories;
using Playground.Application.Queries.Dtos;
using Playground.Application.Queries.Responses;
using Playground.Application.Repositories;
using Playground.Application.Services;
using Playground.Domain.Specifications;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Application.Queries.Resource.List;

public class ListResourceQueryHandler : CommandHandler<ListResourceQuery, ListResourceResponse>
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IConverterService _converterService;

    public ListResourceQueryHandler(IRepositoryFactory repositoryFactory, IConverterService converterService)
    {
        _repositoryFactory = repositoryFactory;
        _converterService = converterService;
    }
    public override async Task<ListResourceResponse> ExecuteAsync(ListResourceQuery query, CancellationToken ct = default)
    {
        var resourceRepository = _repositoryFactory.CreateRepository<Domain.Entities.Resource>();

        IEnumerable<object> resources = [];
        var resourceDtos = new List<object>();

        //si se especifica caso de uso para solo obtener los tipos de recursos
        if (query.UseCase == "AllTypes")
        {
            resources = await GetAllResourceTypesAsync(resourceRepository);
        }
        else if (query.UseCase == "NameFreq")
        {
            resources = await GetResourcesNameFreq(resourceRepository);
        }
        else if(query.UseCase == "ConditionFreq")
        {
            resources = await GetResourcesConditionFreq(resourceRepository);
        }
        else if (AreAllPropertiesNull(query))
        {
            // Obtener todos los recursos si no hay filtros
            resources = await resourceRepository.GetAllAsync(r => r.Facility);
        }
        else
        {
            // Aplicar filtros usando especificaciones predefinidas
            var resourceSpecification = BuildResourceSpecification(query);
            resources = await resourceRepository.GetBySpecificationAsync(resourceSpecification, r => r.Facility);
        }

        if (!(query.UseCase == "AllTypes") && !(query.UseCase == "NameFreq") && !(query.UseCase == "ConditionFreq"))
        {
            foreach (Domain.Entities.Resource resource in resources)
            {
                var resourceDto = new ResourceDto
                {
                    Id = resource.Id,
                    Name = resource.Name,
                    Type = resource.Type,
                    UseFrequency = resource.UseFrequency,
                    Condition = resource.ResourceCondition,
                    FacilityName = resource.Facility.Name,
                    FacilityLocation = resource.Facility.Location,
                    FacilityType = resource.Facility.Type,
                };
                resourceDtos.Add(resourceDto);
            }
            resources = resourceDtos;
        }

        return new ListResourceResponse(resources);
    }

    private static bool AreAllPropertiesNull(ListResourceQuery query)
    {
        return query.Name is null &&
               query.MinUseFrequency is null &&
               query.MaxUseFrequency is null &&
               query.Condition is null &&
               query.FacilityTypes is null &&
               query.ResourceTypes is null;
    }

    private async Task<IEnumerable<string>> GetAllResourceTypesAsync(IRepository<Domain.Entities.Resource> resourceRepository)
    {
        var resources = await resourceRepository.GetAllAsync();
        return resources.Select(resource => resource.Type)
                         .Distinct()
                         .ToList();
    }

    private async Task<IEnumerable<object>> GetResourcesNameFreq(IRepository<Domain.Entities.Resource> resourceRepository)
    {
        var resources = await resourceRepository.GetAllAsync();

        var result = resources
            .GroupBy(r => r.Name)
            .Select(g => new
            {
                Name = g.Key,
                Frequency = g.First().UseFrequency
            })
            .ToList<object>();

        return result;
    }

    private async Task<IEnumerable<object>> GetResourcesConditionFreq(IRepository<Domain.Entities.Resource> resourceRepository)
    {
        var resources = await resourceRepository.GetAllAsync();

        var result = resources
            .GroupBy(r => r.ResourceCondition)
            .Select(g => new
            {
                Condition = g.Key,
                Frequency = g.Count()
            })
            .ToList<object>();

        return result;
    }

    private ISpecification<Domain.Entities.Resource> BuildResourceSpecification(ListResourceQuery query)
    {
        ISpecification<Domain.Entities.Resource> AndSpecification = new ResourceSpecification(resource => true);

        if (!string.IsNullOrEmpty(query.Name))
        {
            AndSpecification = AndSpecification.And(ResourceSpecification.ByName(query.Name));
        }

        if (!string.IsNullOrEmpty(query.Condition))
        {
            AndSpecification = AndSpecification.And(ResourceSpecification.ByResourceCondition(query.Condition));
        }

        if (query.MinUseFrequency.HasValue)
        {
            AndSpecification = AndSpecification.And(ResourceSpecification.ByUseFrequencyMoreOrEqual(query.MinUseFrequency.Value));
        }

        if (query.MaxUseFrequency.HasValue)
        {
            AndSpecification = AndSpecification.And(ResourceSpecification.ByUseFrequencyLessOrEqual(query.MaxUseFrequency.Value));
        }

        ISpecification<Domain.Entities.Resource> OrSpecification = null!;

        if (query.ResourceTypes is not null && query.ResourceTypes.Length > 0)
        {
            var resourceTypes = _converterService.SplitStringToStringEnumerable(query.ResourceTypes);
            foreach (var type in resourceTypes)
            {
                OrSpecification = OrSpecification == null ? ResourceSpecification.ByType(type) : OrSpecification.Or(ResourceSpecification.ByType(type));
            }
        }

        if (OrSpecification is not null)
        {
            AndSpecification = AndSpecification.And(OrSpecification);
        }

        OrSpecification = null!;

        if (query.FacilityTypes is not null && query.FacilityTypes.Length > 0)
        {
            var facilityTypes = _converterService.SplitStringToStringEnumerable(query.FacilityTypes);
            foreach (var type in facilityTypes)
            {
                OrSpecification = OrSpecification == null ? ResourceSpecification.ByFacilityType(type) : OrSpecification.Or(ResourceSpecification.ByFacilityType(type));
            }
        }

        if (OrSpecification is not null)
        {
            AndSpecification = AndSpecification.And(OrSpecification);
        }

        return AndSpecification;
    }

}