using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;

namespace Playground.Application.Commands.Resource.Update;

public class UpdateResourceCommandHandler : CommandHandler<UpdateResourceCommand, GenericResponse>
{
    private readonly IRepositoryFactory repositoryFactory;
    private readonly IUnitOfWork unitOfWork;

    public UpdateResourceCommandHandler(IRepositoryFactory _repositoryFactory, IUnitOfWork _unitOfWork)
    {
        repositoryFactory = _repositoryFactory;
        unitOfWork = _unitOfWork;
    }

    public override async Task<GenericResponse> ExecuteAsync(UpdateResourceCommand command, CancellationToken ct)
    {
        var resourceRepository = repositoryFactory.CreateRepository<Domain.Entities.Resource>();
        var facilityRepository = repositoryFactory.CreateRepository<Domain.Entities.Facility>();

        var resource = await resourceRepository.GetByIdAsync(Guid.Parse(command.Id));
        if (resource is null)
        {
            ThrowError("No se encontró el recurso.");
        }
        else
        {
            if (!string.IsNullOrEmpty(command.Name))
            {
                resource.Name = command.Name;
            }
            if(!string.IsNullOrEmpty(command.Type))
            {
                resource.Type = command.Type;
            }
            if(!string.IsNullOrEmpty(command.ResourceCondition))
            {
                resource.ResourceCondition = command.ResourceCondition;
            }
            if(!string.IsNullOrEmpty(command.FacilityId))
            {
                var facility = await facilityRepository.GetByIdAsync(Guid.Parse(command.FacilityId));
                if(facility is null)
                {
                    ThrowError("No se encontró la instalación que se desea poner.");
                }
                else
                {
                    resource.Facility = facility;
                }
            }

            resourceRepository.Update(resource);
            await unitOfWork.CommitAsync();
        }

        return new GenericResponse("Recurso Modificado");
    }
}