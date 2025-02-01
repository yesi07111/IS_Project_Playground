using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Domain.Specifications;

namespace Playground.Application.Commands.ResourceDate.Delete;

public class DeleteResourceDateCommandHandler : CommandHandler<DeleteResourceDateCommand, GenericResponse>
{
    private readonly IRepositoryFactory repositoryFactory;
    private readonly IUnitOfWork unitOfWork;

    public DeleteResourceDateCommandHandler(IRepositoryFactory _repositoryFactory, IUnitOfWork _unitOfWork)
    {
        repositoryFactory = _repositoryFactory;
        unitOfWork = _unitOfWork;
    }

    public override async Task<GenericResponse> ExecuteAsync(DeleteResourceDateCommand command, CancellationToken ct)
    {
        var resourceDateRepository = repositoryFactory.CreateRepository<Domain.Entities.ResourceDate>();
        var resourceRepository = repositoryFactory.CreateRepository<Domain.Entities.Resource>();

        //nuscar recurso actual
        var resource = await resourceRepository.GetByIdAsync(Guid.Parse(command.ResourceId));
        if (resource == null)
        {
            return new GenericResponse("Recurso no encontrado.");
        }

        var parsedDate = DateTime.Parse(command.Date);

        var resourceDates = await resourceDateRepository.GetBySpecificationAsync(ResourceDateSpecification.ByResource(command.ResourceId).And(ResourceDateSpecification.ByDate(parsedDate)));
        if(resourceDates.Count() == 0)
        {
            ThrowError("No existe frecuencia de uso definida para este día.");
        }
        else
        {
            var resourceDate = resourceDates.FirstOrDefault();
            var usageFrequency = resourceDate!.UseFrequency;
            resource.UseFrequency -= usageFrequency;
            resourceDateRepository.Delete(resourceDate!);
            resourceRepository.Update(resource);
            await unitOfWork.CommitAsync();
        }

        return new GenericResponse("Frecuencia de uso eliminada.");
    }
}