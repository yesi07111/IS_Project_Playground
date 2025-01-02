
using Ardalis.SmartEnum;
using FastEndpoints;
using Playground.Application.Factories;
using Playground.Application.Queries.Dtos;
using Playground.Application.Queries.Responses;
using Playground.Application.Repositories;
using Playground.Application.Services;
using Playground.Domain.Entities;
using Playground.Domain.SmartEnum;
using Playground.Domain.Specifications;

namespace Playground.Application.Queries.Activity.Get;

/// <summary>
/// Manejador para la consulta de obtener una actividad.
/// </summary>
public class GetActivityQueryHandler : CommandHandler<GetActivityQuery, GetActivityResponse>
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IRatingService _ratingService;
    private readonly ICommentsService _commentsService;

    /// <summary>
    /// Constructor que inicializa el manejador con un factory de repositorios.
    /// </summary>
    /// <param name="repositoryFactory">Factory para crear repositorios.</param>
    public GetActivityQueryHandler(IRepositoryFactory repositoryFactory, IRatingService ratingService, ICommentsService commentsService)
    {
        _repositoryFactory = repositoryFactory;
        _ratingService = ratingService;
        _commentsService = commentsService;
    }

    /// <summary>
    /// Ejecuta la consulta para Getar actividades según los filtros proporcionados.
    /// </summary>
    /// <param name="query">Consulta que contiene los filtros.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Una respuesta con la Geta de actividades y tipos de actividades.</returns>
    public override async Task<GetActivityResponse> ExecuteAsync(GetActivityQuery query, CancellationToken ct = default)
    {
        // Crear repositorios usando el factory
        var activityDateRepository = _repositoryFactory.CreateRepository<ActivityDate>();
        var resourceRepository = _repositoryFactory.CreateRepository<Resource>();

        var isUseCase = SmartEnum<UseCaseSmartEnum>.TryFromName(query.UseCase, out UseCaseSmartEnum useCase);
        var activityDetailDto = new ActivityDetailDto();

        var activityDate = await activityDateRepository.GetByIdAsync(Guid.Parse(query.Id), ad => ad.Activity, ad => ad.Activity.Facility, ad => ad.Activity.Educator) ?? throw new KeyNotFoundException("La actividad no fue encontrada.");
        var resourceSpecification = ResourceSpecification.ByFacility(activityDate.Activity.Facility.Id);
        var resources = await resourceRepository.GetBySpecificationAsync(resourceSpecification, r => r.Facility);

        if (isUseCase)
        {
            if (useCase == UseCaseSmartEnum.ActivityView)
            {

                activityDetailDto = new ActivityDetailDto
                {
                    Id = activityDate.Activity.Id,
                    Name = activityDate.Activity.Name,
                    Description = activityDate.Activity.Description,
                    Image = "",
                    Rating = 0,
                    Color = "white",
                    MaximumCapacity = activityDate.Activity.Facility.MaximumCapacity,
                    CurrentCapacity = activityDate.ReservedPlaces,
                    EducatorFullName = $"{activityDate.Activity.Educator.FirstName} {activityDate.Activity.Educator.LastName}",
                    EducatorUsername = activityDate.Activity.Educator.UserName ?? "",
                    FacilityName = activityDate.Activity.Facility.Name,
                    FacilityLocation = activityDate.Activity.Facility.Location,
                    FacilityType = activityDate.Activity.Facility.Type,
                    ActivityType = activityDate.Activity.Type,
                    UsagePolicy = activityDate.Activity.Facility.UsagePolicy,
                    RecommendedAge = activityDate.Activity.RecommendedAge.ToString(),
                    Comments = [],
                    Resources = resources.Select(r => r.Name),
                    Date = activityDate.DateTime
                };
            }
            else if (useCase == UseCaseSmartEnum.ReviewView)
            {
                var _reviewRepository = _repositoryFactory.CreateRepository<Review>();
                activityDetailDto = new ActivityDetailDto
                {
                    Id = activityDate.Activity.Id,
                    Name = activityDate.Activity.Name,
                    Description = activityDate.Activity.Description,
                    Image = "",
                    Rating = _ratingService.CalculateAverageRating(activityDate.Activity, _reviewRepository),
                    Color = "white",
                    MaximumCapacity = activityDate.Activity.Facility.MaximumCapacity,
                    CurrentCapacity = activityDate.ReservedPlaces,
                    EducatorFullName = $"{activityDate.Activity.Educator.FirstName} {activityDate.Activity.Educator.LastName}",
                    EducatorUsername = activityDate.Activity.Educator.UserName ?? "",
                    FacilityName = activityDate.Activity.Facility.Name,
                    FacilityLocation = activityDate.Activity.Facility.Location,
                    FacilityType = activityDate.Activity.Facility.Type,
                    ActivityType = activityDate.Activity.Type,
                    UsagePolicy = activityDate.Activity.Facility.UsagePolicy,
                    RecommendedAge = activityDate.Activity.RecommendedAge.ToString(),
                    Comments = await _commentsService.GetCommentsAsync(activityDate.Activity.Id, _reviewRepository),
                    Resources = resources.Select(r => r.Name),
                    Date = activityDate.DateTime
                };
            }

            return new GetActivityResponse(activityDetailDto);
        }

        throw new ArgumentException("No se especifico un caso de uso válido.");
    }

}