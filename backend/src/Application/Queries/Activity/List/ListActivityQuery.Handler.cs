using Ardalis.SmartEnum;
using FastEndpoints;
using Playground.Application.Factories;
using Playground.Application.Dtos;
using Playground.Application.Responses;
using Playground.Application.Repositories;
using Playground.Application.Services;
using Playground.Domain.Entities;
using Playground.Domain.SmartEnum;
using Playground.Domain.Specifications;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Application.Queries.Activity.List;

/// <summary>
/// Manejador para la consulta de listado de actividades.
/// </summary>
public class ListActivityQueryHandler : CommandHandler<ListActivityQuery, ListActivityResponse>
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IConverterService _converterService;
    private readonly IImageService _imageService;
    private readonly IRatingService _ratingService;

    /// <summary>
    /// Constructor que inicializa el manejador con un factory de repositorios.
    /// </summary>
    /// <param name="repositoryFactory">Factory para crear repositorios.</param>
    public ListActivityQueryHandler(IRepositoryFactory repositoryFactory, IConverterService converterService, IImageService imageService, IRatingService ratingService)
    {
        _repositoryFactory = repositoryFactory;
        _converterService = converterService;
        _imageService = imageService;
        _ratingService = ratingService;
    }

    /// <summary>
    /// Ejecuta la consulta para listar actividades según los filtros proporcionados.
    /// </summary>
    /// <param name="query">Consulta que contiene los filtros.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Una respuesta con la lista de actividades y tipos de actividades.</returns>
    public override async Task<ListActivityResponse> ExecuteAsync(ListActivityQuery query, CancellationToken ct = default)
    {
        // Crear repositorios usando el factory
        var activityRepository = _repositoryFactory.CreateRepository<Domain.Entities.Activity>();
        var activityDateRepository = _repositoryFactory.CreateRepository<ActivityDate>();
        var reviewRepository = _repositoryFactory.CreateRepository<Domain.Entities.Review>();
        var isUseCase = SmartEnum<UseCaseSmartEnum>.TryFromName(query.UseCase, out UseCaseSmartEnum useCase);

        IEnumerable<object> Result = [];

        // Si se especifica un caso de uso
        if (isUseCase)
        {
            // Si este caso de uso es tomar todos los tipos de actividades
            if (useCase == UseCaseSmartEnum.AllTypes)
            {
                // Obtener todos los tipos de actividades
                Result = await GetAllActivityTypesAsync(activityRepository);
            }
            // Ver si se piden las actividades para la vista de Inicio
            else if (useCase == UseCaseSmartEnum.HomeView)
            {
                // Obtener todas las actividades no activas
                ISpecification<ActivityDate> activityDateSpecification = ActivityDateSpecification.ByDateLessOrEqual(DateTime.UtcNow.AddDays(-1)).And(ActivityDateSpecification.ByPending(false));

                var activities = await activityDateRepository.GetBySpecificationAsync(
                    activityDateSpecification,
                    ad => ad.Activity,
                    ad => ad.Activity.Facility
                );

                // Mapear las actividades resultantes al DTO calculando el rating
                var activityDtos = activities.Select(ad => ad)
                    .Select(ad => new ActivityDto
                    {
                        Id = ad!.Id,
                        Name = ad.Activity.Name,
                        Date = ad.DateTime,
                        Image = _imageService.GetImageForActivity(ad.Activity),
                        Rating = _ratingService.CalculateAverageRating(ad, reviewRepository),
                        Color = "white",
                        MaximumCapacity = ad.Activity.Facility.MaximumCapacity,
                        CurrentCapacity = ad.ReservedPlaces,
                        IsPublic = ad.Activity.ItsPrivate ? "false" : "true",
                    })
                    .OrderByDescending(dto => dto.Rating)
                    .Take(3)
                    .ToList();

                Result = activityDtos;
            }
            // Sino ver si se solicita el resultado para la vista de Actividades o Reseñas
            else if (useCase == UseCaseSmartEnum.ActivityView || useCase == UseCaseSmartEnum.ReviewView)
            {
                // Si no hay filtros
                if (AreAllFiltersNull(query))
                {

                    // Obtener todas las actividades activas si no hay filtros
                    ISpecification<ActivityDate> activityDateSpecification =
                        useCase == UseCaseSmartEnum.ActivityView
                        ? new ActivityDateSpecification(activityDate => true)
                            .And(ActivityDateSpecification.ByDateMoreOrEqual(DateTime.UtcNow))
                        : new ActivityDateSpecification(activityDate => true)
                            .And(ActivityDateSpecification.ByDateLessOrEqual(DateTime.UtcNow.AddDays(-1)));

                    Result = await activityDateRepository.GetBySpecificationAsync(activityDateSpecification, ad => ad.Activity, ad => ad.Activity.Facility);
                }
                else
                {

                    // Si hay filtros para ACtivity que esta dentro de ActivityDate entonces construir especificaciones para ambos
                    if (!AreAllFiltersNull(query, excludeDateAndCapacity: true))
                    {
                        var activityDateSpecification = BuildActivityDateSpecification(query, useCase);
                        var activitySpecification = BuildActivitySpecification(query);


                        // Usar la nueva sobrecarga para aplicar la especificación a la propiedad de navegación
                        Result = await activityDateRepository.GetBySpecificationAsync(
                            activityDateSpecification,
                            activitySpecification,
                            ad => ad.Activity, // Propiedad de navegación
                            ad => ad.Activity, // Include para la propiedad de navegación
                            ad => ad.Activity.Facility // Include adicional si es necesario
                        );
                    }
                    // Sino solo hay filtros para ActivityDate, construirlos y aplicarlos
                    else
                    {
                        // Construir especificaciones para ActivityDate
                        var activityDateSpecification = BuildActivityDateSpecification(query, useCase);
                        Result = await activityDateRepository.GetBySpecificationAsync(
                            activityDateSpecification,
                            ad => ad.Activity,
                            ad => ad.Activity.Facility
                        );
                    }
                }

                IEnumerable<ActivityDto> activityDtos;

                if (useCase == UseCaseSmartEnum.ActivityView)
                {
                    // Mapear las actividades resultantes al DTO
                    activityDtos = await MapActivitiesToDtosAsync(Result.Select(ad => ad as ActivityDate)!, activityDateRepository, query.IsNew);
                }
                else
                {
                    // Mapear las actividades resultantes al DTO calculando el rating
                    activityDtos = await MapAndFilterActivitiesAsync(
                        Result.Select(ad => ad as ActivityDate)!,
                        activityDateRepository,
                        reviewRepository,
                        _imageService,
                        _ratingService,
                        query.Rating
                    );
                }

                Result = activityDtos;
            }
            return new ListActivityResponse(Result);
        }

        throw new ArgumentException("No se especifico un caso de uso válido.");
    }

    private ISpecification<ActivityDate> BuildActivityDateSpecification(ListActivityQuery query, UseCaseSmartEnum useCase)
    {
        ISpecification<ActivityDate> specification =
            useCase == UseCaseSmartEnum.ActivityView
            ? new ActivityDateSpecification(ad => ad.DateTime > DateTime.UtcNow)
            : new ActivityDateSpecification(ad => ad.DateTime <= DateTime.UtcNow.AddDays(-1));

        if (DateTime.TryParse(query.StartDateTime, out DateTime _parsedStartDateTime)
            && DateTime.TryParse(query.EndDateTime, out DateTime _parsedEndDateTime)
            && _parsedStartDateTime == _parsedEndDateTime)
        {
            specification = specification.And(ActivityDateSpecification.ByDateEqual(_parsedStartDateTime.ToUniversalTime()));
        }

        else
        {
            if (DateTime.TryParse(query.StartDateTime, out DateTime parsedStartDateTime))
            {
                specification = specification.And(ActivityDateSpecification.ByDateMoreOrEqual(parsedStartDateTime.ToUniversalTime()));
            }

            if (DateTime.TryParse(query.EndDateTime, out DateTime parsedEndDateTime))
            {
                specification = specification.And(ActivityDateSpecification.ByDateLessOrEqual(parsedEndDateTime.ToUniversalTime()));
            }
        }


        if (TimeSpan.TryParse(query.StartTime, out TimeSpan _parsedStartTime)
        && TimeSpan.TryParse(query.EndTime, out TimeSpan _parsedEndTime)
        && _parsedStartTime == _parsedEndTime)
        {
            specification = specification.And(ActivityDateSpecification.ByTimeEqual(_parsedStartTime));
        }

        else
        {
            if (TimeSpan.TryParse(query.StartTime, out TimeSpan parsedStartTime))
            {
                specification = specification.And(ActivityDateSpecification.ByTimeMoreOrEqual(parsedStartTime));
            }


            if (TimeSpan.TryParse(query.EndTime, out TimeSpan parsedEndTime))
            {
                specification = specification.And(ActivityDateSpecification.ByTimeLessOrEqual(parsedEndTime));
            }
        }

        if (query.Capacity.HasValue)
        {
            specification = specification.And(ActivityDateSpecification.ByAvailableCapacity(query.Capacity.Value));
        }

        ISpecification<ActivityDate> orSpecification1 = null!;
        if (!string.IsNullOrEmpty(query.Today) && bool.TryParse(query.Today, out bool today) && today)
        {
            orSpecification1 = orSpecification1 == null ? ActivityDateSpecification.ByToday() : orSpecification1.Or(ActivityDateSpecification.ByToday());
        }

        if (!string.IsNullOrEmpty(query.Tomorrow) && bool.TryParse(query.Tomorrow, out bool tomorrow) && tomorrow)
        {
            orSpecification1 = orSpecification1 == null ? ActivityDateSpecification.ByTomorrow() : orSpecification1.Or(ActivityDateSpecification.ByTomorrow());
        }

        if (!string.IsNullOrEmpty(query.ThisWeek) && bool.TryParse(query.ThisWeek, out bool thisWeek) && thisWeek)
        {
            orSpecification1 = orSpecification1 == null ? ActivityDateSpecification.ByThisWeek() : orSpecification1.Or(ActivityDateSpecification.ByThisWeek());
        }

        if (query.DaysOfWeek is not null && query.DaysOfWeek.Length > 0)
        {
            var intDaysOfWeek = _converterService.SplitStringToIntEnumerable(query.DaysOfWeek);
            var daysOfWeek = _converterService.ConvertIntToDayOfWeek(intDaysOfWeek);
            foreach (var day in daysOfWeek)
            {
                orSpecification1 = orSpecification1 == null ? ActivityDateSpecification.ByDayOfWeek(day) : orSpecification1.Or(ActivityDateSpecification.ByDayOfWeek(day));
            }
        }

        if (orSpecification1 is not null)
        {
            specification = specification.And(orSpecification1);
        }

        return specification;
    }

    private ISpecification<Domain.Entities.Activity> BuildActivitySpecification(ListActivityQuery query)
    {
        ISpecification<Domain.Entities.Activity> andSpecification = new ActivitySpecification(activity => true);

        if (query.MinAge.HasValue)
        {
            andSpecification = andSpecification.And(ActivitySpecification.ByRecommendedAgeMoreOrEqual(query.MinAge.Value));
        }

        if (query.MaxAge.HasValue)
        {
            andSpecification = andSpecification.And(ActivitySpecification.ByRecommendedAgeLessOrEqual(query.MaxAge.Value));
        }

        if (bool.TryParse(query.Availability, out bool availabilityValue))
        {
            andSpecification = andSpecification.And(ActivitySpecification.ByItsPrivate(!availabilityValue));
        }

        ISpecification<Domain.Entities.Activity> orSpecification1 = null!;
        if (query.ActivityTypes is not null && query.ActivityTypes.Length > 0)
        {
            var activityTypes = _converterService.SplitStringToStringEnumerable(query.ActivityTypes);
            foreach (var type in activityTypes)
            {
                orSpecification1 = orSpecification1 == null ? ActivitySpecification.ByType(type) : orSpecification1.Or(ActivitySpecification.ByType(type));
            }
        }

        if (query.FacilityTypes is not null && query.FacilityTypes.Length > 0)
        {
            var facilityTypes = _converterService.SplitStringToStringEnumerable(query.FacilityTypes);
            foreach (var type in facilityTypes)
            {
                orSpecification1 = orSpecification1 == null ? ActivitySpecification.ByFacilityType(type) : orSpecification1.Or(ActivitySpecification.ByFacilityType(type));
            }
        }

        ISpecification<Domain.Entities.Activity> orSpecification2 = null!;
        if (query.Educators is not null && query.Educators.Length > 0)
        {
            var educators = _converterService.SplitStringToStringEnumerable(query.Educators);
            foreach (var educator in educators)
            {
                if (Guid.TryParse(educator, out Guid educatorId))
                {
                    orSpecification2 = orSpecification2 == null ? ActivitySpecification.ByEducator(educatorId.ToString()) : orSpecification2.Or(ActivitySpecification.ByEducator(educatorId.ToString()));
                }
            }
        }

        if (orSpecification1 is not null)
        {
            andSpecification = andSpecification.And(orSpecification1);
        }

        if (orSpecification2 is not null)
        {
            andSpecification = andSpecification.And(orSpecification2);
        }

        return andSpecification;
    }

    private static bool AreAllFiltersNull(ListActivityQuery query, bool excludeDateAndCapacity = false)
    {
        return (query.Rating is null) &&
               string.IsNullOrEmpty(query.StartTime) &&
               string.IsNullOrEmpty(query.EndTime) &&
               (excludeDateAndCapacity || query.StartDateTime is null) &&
               (excludeDateAndCapacity || query.EndDateTime is null) &&
               ((query.Educators is null) || (query.Educators!.Length == 0)) &&
               (query.ActivityTypes is null) &&
               (query.FacilityTypes is null) &&
               ((query.MinAge is null) || (query.MinAge == 2)) &&
               ((query.MaxAge is null) || (query.MaxAge == 17)) &&
               (excludeDateAndCapacity || query.Capacity is null) &&
               (excludeDateAndCapacity || query.Today is null) &&
               (excludeDateAndCapacity || query.Tomorrow is null) &&
               (excludeDateAndCapacity || query.ThisWeek is null) &&
               (excludeDateAndCapacity || query.DaysOfWeek is null) &&
               query.Availability is null;
    }

    private async Task<IEnumerable<string>> GetAllActivityTypesAsync(IRepository<Domain.Entities.Activity> activityRepository)
    {
        var activities = await activityRepository.GetAllAsync();
        return activities.Select(activity => activity.Type)
                         .Distinct()
                         .ToList();
    }

    private async Task<string> SeeIfActivityIsNew(IRepository<ActivityDate> activityDateRepository, Guid id)
    {
        var activityDates = await activityDateRepository.GetBySpecificationAsync(
            new ActivityDateSpecification(ad => ad.Activity.Id == id)
        );

        var isNew = activityDates.Count() == 1 && activityDates.First().DateTime > DateTime.UtcNow;
        return isNew ? "true" : string.Empty;
    }

    private async Task<List<ActivityDto>> MapActivitiesToDtosAsync(
        IEnumerable<ActivityDate> activityDates,
        IRepository<ActivityDate> activityDateRepository,
        string? isNewQuery)
    {
        var activityDtos = new List<ActivityDto>();

        // Intentamos parsear el valor de isNewQuery a un bool
        var filterIsNew = bool.TryParse(isNewQuery, out var parsedIsNew);

        foreach (var ad in activityDates)
        {
            var isNew = await SeeIfActivityIsNew(activityDateRepository, ad.Activity.Id);

            // Aplicamos el filtro solo si isNewFilter tiene un valor
            if (filterIsNew && (parsedIsNew && isNew == "" || !parsedIsNew && isNew == "true"))
            {
                continue; // Omitimos actividades que no cumplen con el filtro
            }

            var activityDto = new ActivityDto
            {
                Id = ad.Id,
                Name = ad.Activity.Name,
                Date = ad.DateTime,
                Image = _imageService.GetImageForActivity(ad.Activity),
                Rating = 0,
                Color = "white",
                MaximumCapacity = ad.Activity.Facility.MaximumCapacity,
                CurrentCapacity = ad.ReservedPlaces,
                IsPublic = ad.Activity.ItsPrivate ? "false" : "true",
                IsNew = isNew,
                Pending = ad.Pending,
            };

            activityDtos.Add(activityDto);
        }
        ;

        return activityDtos;
    }


    private async Task<List<ActivityDto>> MapAndFilterActivitiesAsync(
    IEnumerable<ActivityDate> activityDates,
    IRepository<ActivityDate> activityDateRepository,
    IRepository<Domain.Entities.Review> reviewRepository,
    IImageService imageService,
    IRatingService ratingService,
    string? ratingFilter)
    {
        var activityDtos = new List<ActivityDto>();

        foreach (var ad in activityDates)
        {
            var isNew = await SeeIfActivityIsNew(activityDateRepository, ad.Activity.Id);
            var rating = ratingService.CalculateAverageRating(ad, reviewRepository);

            var activityDto = new ActivityDto
            {
                Id = ad.Id,
                Name = ad.Activity.Name,
                Date = ad.DateTime,
                Image = imageService.GetImageForActivity(ad.Activity),
                Rating = rating,
                Color = "white",
                MaximumCapacity = ad.Activity.Facility.MaximumCapacity,
                CurrentCapacity = ad.ReservedPlaces,
                IsPublic = ad.Activity.ItsPrivate ? "false" : "true",
                IsNew = isNew,
                Pending = ad.Pending,
            };

            activityDtos.Add(activityDto);
        }

        // Aplicar el filtro de calificación si es necesario
        if (double.TryParse(ratingFilter, out double ratingValue))
        {
            activityDtos = [.. activityDtos.Where(dto => dto.Rating >= ratingValue)];
        }

        return activityDtos;
    }
}

