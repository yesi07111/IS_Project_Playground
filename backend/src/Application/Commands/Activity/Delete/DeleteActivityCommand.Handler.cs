using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Domain.Entities;
using Playground.Domain.SmartEnum;

namespace Playground.Application.Commands.Activity.Delete;

public class DeleteActivityCommandHandler : CommandHandler<DeleteActivityCommand, GenericResponse>
{
    private readonly IRepositoryFactory repositoryFactory;
    private readonly IUnitOfWork unitOfWork;

    public DeleteActivityCommandHandler(IRepositoryFactory _repositoryFactory, IUnitOfWork _unitOfWork)
    {
        repositoryFactory = _repositoryFactory;
        unitOfWork = _unitOfWork;
    }

    public override async Task<GenericResponse> ExecuteAsync(DeleteActivityCommand command, CancellationToken ct)
    { 
        var activityDateRepository = repositoryFactory.CreateRepository<ActivityDate>();
        var activityRepository = repositoryFactory.CreateRepository<Domain.Entities.Activity>();

        if(command.UseCase == UseCaseSmartEnum.DeleteActivity.Name)
        {
            var activity = await activityRepository.GetByIdAsync(Guid.Parse(command.ActivityId)) ?? throw new ArgumentException("La actividad no existe.");
            activityRepository.Delete(activity);
        }
        else if(command.UseCase == UseCaseSmartEnum.DeleteActivityDate.Name)
        {
            var activityDate = await activityDateRepository.GetByIdAsync(Guid.Parse(command.ActivityDateId)) ?? throw new ArgumentException("La actividad con fecha no existe.");
            activityDateRepository.Delete(activityDate);
        }
        else
        {
            ThrowError("Caso de uso no válido");
        }

        unitOfWork.Commit();
        return new GenericResponse("Actividad Eliminada Permanentemente");
    }
}