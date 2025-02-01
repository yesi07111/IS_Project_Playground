using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Domain.Specifications;

namespace Playground.Application.Commands.Facility.Create;

public class CreateFacilityCommandHandler : CommandHandler<CreateFacilityCommand, GenericResponse>
{
    private readonly IRepositoryFactory repositoryFactory;
    private readonly IUnitOfWork unitOfWork;

    public CreateFacilityCommandHandler(IRepositoryFactory _repositoryFactory, IUnitOfWork _unitOfWork)
    {
        repositoryFactory = _repositoryFactory;
        unitOfWork = _unitOfWork;
    }

    public override async Task<GenericResponse> ExecuteAsync(CreateFacilityCommand command, CancellationToken ct)
    {
        var facilityRepository = repositoryFactory.CreateRepository<Domain.Entities.Facility>();

        var existingName = await facilityRepository.GetBySpecificationAsync(FacilitySpecification.ByName(command.Name));
        if(existingName.Any())
        {
            ThrowError("Ese nombre de instalación ya existe, pruebe con otro.");
        }

        var facility = new Domain.Entities.Facility
        {
            Name = command.Name,
            Location = command.Location,
            Type = command.Type,
            MaximumCapacity = command.MaximumCapacity,
            UsagePolicy = command.UsagePolicy,
        };

        await facilityRepository.AddAsync(facility);
        await unitOfWork.CommitAsync();

        return new GenericResponse("Instalación Creada");
    }
}