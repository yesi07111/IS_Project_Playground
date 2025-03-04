using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Domain.Entities;
using Playground.Domain.SmartEnum;

namespace Playground.Application.Commands.Activity.Update;

public class UpdateActivityCommandHandler : CommandHandler<UpdateActivityCommand, GenericResponse>
{
    private readonly IRepositoryFactory repositoryFactory;
    private readonly IUnitOfWork unitOfWork;

    public UpdateActivityCommandHandler(IRepositoryFactory _repositoryFactory, IUnitOfWork _unitOfWork)
    {
        repositoryFactory = _repositoryFactory;
        unitOfWork = _unitOfWork;
    }

    public override async Task<GenericResponse> ExecuteAsync(UpdateActivityCommand command, CancellationToken ct)
    {
        var activityDateRepository = repositoryFactory.CreateRepository<ActivityDate>();
        var activityRepository = repositoryFactory.CreateRepository<Domain.Entities.Activity>();
        var userRepository = repositoryFactory.CreateRepository<Domain.Entities.Auth.User>();
        var facilityRepository = repositoryFactory.CreateRepository<Domain.Entities.Facility>();


        if (command.UseCase == UseCaseSmartEnum.UpdateActivity.Name)
        {
            var activity = await activityRepository.GetByIdAsync(Guid.Parse(command.ActivityId));
            if (activity is null)
            {
                ThrowError("No se encontró la actividad");
            }
            if (!string.IsNullOrEmpty(command.Name))
            {
                activity.Name = command.Name;
            }
            if (!string.IsNullOrEmpty(command.Description))
            {
                activity.Description = command.Description;
            }
            if (!string.IsNullOrEmpty(command.Educator))
            {
                var educator = await userRepository.GetByIdAsync(command.Educator);
                if (educator is null)
                {
                    ThrowError("Educador no encontrado");
                }
                activity.Educator = educator;
            }
            if (!string.IsNullOrEmpty(command.Type))
            {
                activity.Type = command.Type;
            }
            if (!string.IsNullOrEmpty(command.Facility))
            {
                var facility = await facilityRepository.GetByIdAsync(Guid.Parse(command.Facility));
                if (facility is null)
                {
                    ThrowError("Instalación no encontrada");
                }
                activity.Facility = facility;
            }
            activity.RecommendedAge = command.RecommendedAge;
            activity.ItsPrivate = command.Private;

            activityRepository.Update(activity);
        }
        else if (command.UseCase == UseCaseSmartEnum.UpdateActivityDate.Name)
        {
            DateTime dateTime;
            DateTime parsedDate;
            DateTime parsedTime;
            var activityDate = await activityDateRepository.GetByIdAsync(Guid.Parse(command.ActivityDateId));
            if (activityDate is null)
            {
                ThrowError("Fecha no encontrada");
            }

            if(!string.IsNullOrEmpty(command.Date) || !string.IsNullOrEmpty(command.Time))
            {
                if(!string.IsNullOrEmpty(command.Date) && string.IsNullOrEmpty(command.Time))
                {
                    parsedDate = DateTime.Parse(command.Date);
                    parsedTime = DateTime.MinValue;
                }
                else if(string.IsNullOrEmpty(command.Date) && !string.IsNullOrEmpty(command.Time))
                {
                    parsedDate = DateTime.MinValue;
                    parsedTime = DateTime.Parse(command.Time);
                }
                else 
                {
                    parsedDate = DateTime.Parse(command.Date!);
                    parsedTime = DateTime.Parse(command.Time!);
                }

                dateTime = parsedDate.Date + parsedTime.TimeOfDay;
                activityDate.DateTime = dateTime.ToUniversalTime();
            }
            activityDate.Pending = command.Pending;

            activityDateRepository.Update(activityDate);
        }
        else
        {
            ThrowError("Caso de uso no válido.");
        }

        await unitOfWork.CommitAsync();

        return new GenericResponse("Actividad modificada");
    }
}