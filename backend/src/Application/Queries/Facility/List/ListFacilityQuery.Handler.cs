using Ardalis.SmartEnum;
using FastEndpoints;
using Playground.Application.Factories;
using Playground.Application.Dtos;
using Playground.Application.Responses;
using Playground.Application.Repositories;
using Playground.Domain.Specifications;
using Playground.Domain.Specifications.BaseSpecifications;
using Playground.Domain.SmartEnum;

namespace Playground.Application.Queries.Facility.List;

public class ListFacilityQueryHandler : CommandHandler<ListFacilityQuery, ListFacilityResponse>
{
    private readonly IRepositoryFactory _repositoryFactory;

    public ListFacilityQueryHandler(IRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public override async Task<ListFacilityResponse> ExecuteAsync(ListFacilityQuery query, CancellationToken ct = default)
    {
        var facilityRepository = _repositoryFactory.CreateRepository<Domain.Entities.Facility>();
        IEnumerable<Domain.Entities.Facility> facilities = [];
        IEnumerable<string> allTypes = [];
        IEnumerable<string> allLocations = [];

        var isUseCase = SmartEnum<UseCaseSmartEnum>.TryFromName(query.UseCase, out UseCaseSmartEnum useCase);
        if (isUseCase)
        {
            if (useCase == UseCaseSmartEnum.AllTypes)
            {
                allTypes = await GetAllFacilityTypesAsync(facilityRepository);
                return new ListFacilityResponse(allTypes);
            }
            else if(useCase == UseCaseSmartEnum.AllLocations)
            {
                allLocations = await GetAllFacilityLocationsAsync(facilityRepository);
                return new ListFacilityResponse(allLocations);
            }
            else if (useCase == UseCaseSmartEnum.AdminEducatorView)
            {
                if (AreAllPropertiesNull(query))
                {
                    facilities = await facilityRepository.GetAllAsync();
                }
                else
                {
                    var facilitySpecification = BuildFacilitySpecification(query);
                    facilities = await facilityRepository.GetBySpecificationAsync(facilitySpecification);
                }

                var facilityDtos = MapFacilitiesToDtos(facilities);
                return new ListFacilityResponse(facilityDtos);
            }
        }

        throw new ArgumentException("No se especifico un caso de uso válido.");
    }

    private ISpecification<Domain.Entities.Facility> BuildFacilitySpecification(ListFacilityQuery query)
    {
        ISpecification<Domain.Entities.Facility> specification = new FacilitySpecification(facility => true);

        if (!string.IsNullOrEmpty(query.Name))
        {
            specification = specification.And(FacilitySpecification.ByName(query.Name));
        }

        if (!string.IsNullOrEmpty(query.Location))
        {
            specification = specification.And(FacilitySpecification.ByLocation(query.Location));
        }

        if (!string.IsNullOrEmpty(query.Type))
        {
            specification = specification.And(FacilitySpecification.ByType(query.Type));
        }

        if (query.MaximumCapacity.HasValue)
        {
            specification = specification.And(FacilitySpecification.ByMaximumCapacity(query.MaximumCapacity.Value));
        }

        if (!string.IsNullOrEmpty(query.UsagePolicy))
        {
            specification = specification.And(FacilitySpecification.ByUsagePolicy(query.UsagePolicy));
        }

        return specification;
    }

    private static bool AreAllPropertiesNull(ListFacilityQuery query)
    {
        return string.IsNullOrEmpty(query.Name) &&
               string.IsNullOrEmpty(query.Location) &&
               string.IsNullOrEmpty(query.Type) &&
               query.MaximumCapacity is null &&
               string.IsNullOrEmpty(query.UsagePolicy);
    }

    private static IEnumerable<FacilityDto> MapFacilitiesToDtos(IEnumerable<Domain.Entities.Facility> facilities)
    {
        return facilities.Select(facility => new FacilityDto
        {
            Id = facility.Id.ToString(),
            Name = facility.Name,
            Location = facility.Location,
            Type = facility.Type,
            UsagePolicy = facility.UsagePolicy,
            MaximumCapacity = facility.MaximumCapacity
        }).ToList();
    }

    private async Task<IEnumerable<string>> GetAllFacilityTypesAsync(IRepository<Domain.Entities.Facility> facilityRepository)
    {
        // Usar LINQ para obtener los tipos distintos de instalaciones
        var facilities = await facilityRepository.GetAllAsync();
        return facilities.Select(facility => facility.Type)
                         .Distinct()
                         .ToList();
    }

    private async Task<IEnumerable<string>> GetAllFacilityLocationsAsync(IRepository<Domain.Entities.Facility> facilityRepository)
    {
        // Usar LINQ para obtener los tipos distintos de instalaciones
        var facilities = await facilityRepository.GetAllAsync();
        return facilities.Select(facility => facility.Location)
                         .Distinct()
                         .ToList();
    }
}
