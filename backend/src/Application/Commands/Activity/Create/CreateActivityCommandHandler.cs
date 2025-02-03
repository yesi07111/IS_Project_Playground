using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Domain.Entities;
using Playground.Domain.Entities.Auth;
using Playground.Domain.SmartEnum;
using Playground.Domain.Specifications;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Application.Commands.Activity.Create;

public class CreateActivityCommandHandler : CommandHandler<CreateActivityCommand, GenericResponse>
{
    private readonly IRepositoryFactory repositoryFactory;
    private readonly IUnitOfWork unitOfWork;

    public CreateActivityCommandHandler(IRepositoryFactory _repositoryFactory, IUnitOfWork _unitOfWork)
    {
        repositoryFactory = _repositoryFactory;
        unitOfWork = _unitOfWork;
    }

    public override async Task<GenericResponse> ExecuteAsync(CreateActivityCommand command, CancellationToken ct)
    {
        var activityDateRepository = repositoryFactory.CreateRepository<ActivityDate>();
        var activityRepository = repositoryFactory.CreateRepository<Domain.Entities.Activity>();
        var userRepository = repositoryFactory.CreateRepository<Domain.Entities.Auth.User>();
        var facilityRepository = repositoryFactory.CreateRepository<Domain.Entities.Facility>();

        if (command.UseCase == UseCaseSmartEnum.CreateBoth.Name)
        {
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
            if(educator is null)
            {
                ThrowError("Educador no encontrado");
            }
            var facility = await facilityRepository.GetByIdAsync(Guid.Parse(command.Facility));
            if(facility is null)
            {
                ThrowError("Instalación no encontrada");
            }

            //crear la actividad nueva
            var activity = new Domain.Entities.Activity
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

            var activityDate = new ActivityDate
            {
                Activity = activity,
                DateTime = dateTime.ToUniversalTime(),
                Pending = command.Pending,
            };

            await activityDateRepository.AddAsync(activityDate);
        }
        else if (command.UseCase == UseCaseSmartEnum.CreateActivity.Name)
        {
            //buscar educadir e instalaion correspondientes
            var educator = await userRepository.GetByIdAsync(command.Educator);
            if(educator is null)
            {
                ThrowError("Educador no encontrado");
            }
            var facility = await facilityRepository.GetByIdAsync(Guid.Parse(command.Facility));
            if(facility is null)
            {
                ThrowError("Instalación no encontrada");
            }

            var activity = new Domain.Entities.Activity
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
        else if (command.UseCase == UseCaseSmartEnum.CreateActivityDate.Name)
        {
            var activity = await activityRepository.GetByIdAsync(Guid.Parse(command.ActivityId));
            if(activity is null)
            {
                ThrowError("Actividad no encontrada");
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

            var activityDate = new ActivityDate
            {
                Activity = activity,
                DateTime = dateTime.ToUniversalTime(),
                Pending = command.Pending,
            };

            await activityDateRepository.AddAsync(activityDate);
        }
        else
        {
            ThrowError("Caso de uso no válido.");
        }

        await unitOfWork.CommitAsync();

        return new GenericResponse("Actividad Creada");
    }
}