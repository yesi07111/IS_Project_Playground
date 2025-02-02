
using Ardalis.SmartEnum;
using FastEndpoints;
using Playground.Application.Factories;
using Playground.Application.Dtos;
using Playground.Application.Responses;
using Playground.Application.Services;
using Playground.Domain.Entities;
using Playground.Domain.SmartEnum;
using Playground.Domain.Specifications;

namespace Playground.Application.Queries.Activity.Get;

/// <summary>
/// Manejador para la consulta de obtener una actividad.
/// </summary>
public class GetActivityQueryHandler(IRepositoryFactory repositoryFactory, IRatingService ratingService, ICommentsService commentsService) : CommandHandler<GetActivityQuery, GetActivityResponse>
{
    /// <summary>
    /// Ejecuta la consulta para Getar actividades según los filtros proporcionados.
    /// </summary>
    /// <param name="query">Consulta que contiene los filtros.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Una respuesta con la Geta de actividades y tipos de actividades.</returns>
    public override async Task<GetActivityResponse> ExecuteAsync(GetActivityQuery query, CancellationToken ct = default)
    {
        Console.WriteLine("!!!!!!!!!!!!!!!!!\n\n\n!!!!!!!!!!!!!!!!!");
        Console.WriteLine("Inicio de ExecuteAsync en GetActivityQueryHandler");
        Console.WriteLine("Query ID: " + query.Id);
        Console.WriteLine("UseCase: " + query.UseCase);
        Console.WriteLine("!!!!!!!!!!!!!!!!!\n\n\n!!!!!!!!!!!!!!!!!");

        // Crear repositorios usando el factory
        var activityDateRepository = repositoryFactory.CreateRepository<ActivityDate>();
        var resourceRepository = repositoryFactory.CreateRepository<Domain.Entities.Resource>();

        Console.WriteLine("Repositorios creados.");

        var isUseCase = SmartEnum<UseCaseSmartEnum>.TryFromName(query.UseCase, out UseCaseSmartEnum useCase);
        var activityDetailDto = new ActivityDetailDto();

        Console.WriteLine("UseCase encontrado: " + isUseCase);

        var activityDate = await activityDateRepository.GetByIdAsync(Guid.Parse(query.Id), ad => ad.Activity,
                                                                     ad => ad.Activity.Facility,
                                                                     ad => ad.Activity.Educator)
        ?? throw new KeyNotFoundException("La actividad no fue encontrada.");

        Console.WriteLine("Actividad obtenida: " + activityDate.Activity.Name);

        var resourceSpecification = ResourceSpecification.ByFacility(activityDate.Activity.Facility.Id);
        var resources = await resourceRepository.GetBySpecificationAsync(resourceSpecification, r => r.Facility);

        Console.WriteLine("Recursos obtenidos: " + resources.Count());

        // Filtrar recursos cuyo estado sea "Bueno"
        var filteredResources = resources.Where(r => r.ResourceCondition == ResourceStateSmartEnum.Bueno.Name);

        Console.WriteLine("Recursos filtrados: " + filteredResources.Count());

        if (isUseCase)
        {
            if (useCase == UseCaseSmartEnum.ActivityView)
            {
                Console.WriteLine("Caso de uso: ActivityView");
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
                    EducatorId = activityDate.Activity.Educator.Id,
                    EducatorFullName = $"{activityDate.Activity.Educator.FirstName} {activityDate.Activity.Educator.LastName}",
                    EducatorUsername = activityDate.Activity.Educator.UserName ?? "",
                    FacilityName = activityDate.Activity.Facility.Name,
                    FacilityLocation = activityDate.Activity.Facility.Location,
                    FacilityType = activityDate.Activity.Facility.Type,
                    ActivityType = activityDate.Activity.Type,
                    UsagePolicy = activityDate.Activity.Facility.UsagePolicy,
                    RecommendedAge = activityDate.Activity.RecommendedAge.ToString(),
                    Comments = [],
                    Resources = filteredResources.Select(r => r.Name),
                    Date = activityDate.DateTime,
                    IsPublic = activityDate.Activity.ItsPrivate ? "Privada" : "Pública"
                };
            }
            else if (useCase == UseCaseSmartEnum.ReviewView)
            {
                Console.WriteLine("Caso de uso: ReviewView");
                var _reviewRepository = repositoryFactory.CreateRepository<Domain.Entities.Review>();
                activityDetailDto = new ActivityDetailDto
                {
                    Id = activityDate.Activity.Id,
                    Name = activityDate.Activity.Name,
                    Description = activityDate.Activity.Description,
                    Image = "",
                    Rating = ratingService.CalculateAverageRating(activityDate, _reviewRepository),
                    Color = "white",
                    MaximumCapacity = activityDate.Activity.Facility.MaximumCapacity,
                    CurrentCapacity = activityDate.ReservedPlaces,
                    EducatorId = activityDate.Activity.Educator.Id,
                    EducatorFullName = $"{activityDate.Activity.Educator.FirstName} {activityDate.Activity.Educator.LastName}",
                    EducatorUsername = activityDate.Activity.Educator.UserName ?? "",
                    FacilityName = activityDate.Activity.Facility.Name,
                    FacilityLocation = activityDate.Activity.Facility.Location,
                    FacilityType = activityDate.Activity.Facility.Type,
                    ActivityType = activityDate.Activity.Type,
                    UsagePolicy = activityDate.Activity.Facility.UsagePolicy,
                    RecommendedAge = activityDate.Activity.RecommendedAge.ToString(),
                    Comments = await commentsService.GetCommentsAsync(activityDate.Id, _reviewRepository),
                    Resources = filteredResources.Select(r => r.Name),
                    Date = activityDate.DateTime,
                    IsPublic = activityDate.Activity.ItsPrivate ? "Privada" : "Pública"
                };
            }

            Console.WriteLine("Actividad detallada creada.");
            return new GetActivityResponse(activityDetailDto);
        }

        Console.WriteLine("No se especificó un caso de uso válido.");
        throw new ArgumentException("No se especifico un caso de uso válido.");
    }
}