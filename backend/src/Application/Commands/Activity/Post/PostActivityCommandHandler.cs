using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Domain.Entities;
using Playground.Domain.Entities.Auth;
using Playground.Domain.Specifications;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Application.Commands.Activity.Post;

public class PostActivityCommandHandler : CommandHandler<PostActivityCommand, GenericResponse>
{
    private readonly IRepositoryFactory repositoryFactory;
    private readonly IUnitOfWork unitOfWork;

    public PostActivityCommandHandler(IRepositoryFactory _repositoryFactory, IUnitOfWork _unitOfWork)
    {
        repositoryFactory = _repositoryFactory;
        unitOfWork = _unitOfWork;
    }

    public override async Task<GenericResponse> ExecuteAsync(PostActivityCommand command, CancellationToken ct)
    {
        var activityDateRepository = repositoryFactory.CreateRepository<ActivityDate>();
        var activityRepository = repositoryFactory.CreateRepository<Domain.Entities.Activity>();
        var userRepository = repositoryFactory.CreateRepository<Domain.Entities.Auth.User>();
        var facilityRepository = repositoryFactory.CreateRepository<Domain.Entities.Facility>();

        ISpecification<Domain.Entities.Activity> activitySpecification = new ActivitySpecification(activity => true);

        if (!Guid.TryParse(command.Educator, out var educatorId))
        {
            throw new ArgumentException("El ID del educador no es un GUID válido.");
        }

        if (!Guid.TryParse(command.Facility, out var facilityId))
        {
            throw new ArgumentException("El ID de la instalación no es un GUID válido.");
        }

        if (!DateTime.TryParse(command.Date, out DateTime parsedDate))
        {
            throw new ArgumentException("La fecha no es válida.");
        }

        if (!DateTime.TryParse(command.Time, out DateTime parsedTime))
        {
            throw new ArgumentException("La hora no es válida.");
        }

        DateTime dateTime = parsedDate.Date + parsedTime.TimeOfDay;

        //buscar educadir e instalaion correspondientes
        var educator = await userRepository.GetByIdAsync(command.Educator);
        var facility = await facilityRepository.GetByIdAsync(facilityId);

        //verificar si la actividad ya existe 
        activitySpecification = activitySpecification.And(ActivitySpecification.ByName(command.Name));
        activitySpecification = activitySpecification.And(ActivitySpecification.ByEducator(command.Educator));
        activitySpecification = activitySpecification.And(ActivitySpecification.ByType(command.Type));
        activitySpecification = activitySpecification.And(ActivitySpecification.ByRecommendedAgeEqual(command.RecommendedAge));
        activitySpecification = activitySpecification.And(ActivitySpecification.ByFacility(facilityId));
        activitySpecification = activitySpecification.And(ActivitySpecification.ByDescription(command.Description));
        activitySpecification = activitySpecification.And(ActivitySpecification.ByItsPrivate(command.Private));

        var searchActivity = await activityRepository.GetBySpecificationAsync(activitySpecification);
        foreach (var x in searchActivity)
        {
            Console.WriteLine("!!!!!!!!!!!!!!!!!!" + x.Name + "!");
        }

        Domain.Entities.Activity activity;
        ActivityDate activityDate;

        //si no existe crear actividad nueva y nueva instancia de activity date
        if (!searchActivity.Any())
        {
            activity = new Domain.Entities.Activity
            {
                Name = command.Name,
                Description = command.Description,
                Educator = educator!,
                Type = command.Type,
                RecommendedAge = command.RecommendedAge,
                ItsPrivate = command.Private,
                Facility = facility!
            };

            await activityRepository.AddAsync(activity);
        }
        //si existe crear una nueva instancia de activity date con esa actividad
        else
        {
            activity = searchActivity.FirstOrDefault()!;
        }

        activityDate = new ActivityDate
        {
            Activity = activity,
            DateTime = dateTime.ToUniversalTime(),
            Pending = command.Pending,
        };

        await activityDateRepository.AddAsync(activityDate);

        //commit 
        await unitOfWork.CommitAsync();

        return new GenericResponse("Actividad Creada");
    }
}