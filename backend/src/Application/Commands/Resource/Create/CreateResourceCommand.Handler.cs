using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;

namespace Playground.Application.Commands.Resource.Create;

public class CreateResourceCommandHandler : CommandHandler<CreateResourceCommand, GenericResponse>
{
    private readonly IRepositoryFactory repositoryFactory;
    private readonly IUnitOfWork unitOfWork;

    public CreateResourceCommandHandler(IRepositoryFactory _repositoryFactory, IUnitOfWork _unitOfWork)
    {
        repositoryFactory = _repositoryFactory;
        unitOfWork = _unitOfWork;
    }

    public override async Task<GenericResponse> ExecuteAsync(CreateResourceCommand command, CancellationToken ct)
    {
        var resourceRepository = repositoryFactory.CreateRepository<Domain.Entities.Resource>();
        var facilityRepository = repositoryFactory.CreateRepository<Domain.Entities.Facility>();

        var facility = await facilityRepository.GetByIdAsync(Guid.Parse(command.FacilityId));
        if (facility is null)
        {
            ThrowError("No se encontró la instalación asociada.");
        }
        else
        {
            var resource = new Domain.Entities.Resource
            {
                Name = command.Name,
                Type = command.Type,
                ResourceCondition = command.ResourceCondition,
                Facility = facility,
            };

            await resourceRepository.AddAsync(resource);
            await unitOfWork.CommitAsync();
        }

        return new GenericResponse("Recurso Creado");
    }
}