using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;

namespace Playground.Application.Commands.Facility.Update;

public class UpdateFacilityCommandHandler : CommandHandler<UpdateFacilityCommand, GenericResponse>
{
    private readonly IRepositoryFactory repositoryFactory;
    private readonly IUnitOfWork unitOfWork;

    public UpdateFacilityCommandHandler(IRepositoryFactory _repositoryFactory, IUnitOfWork _unitOfWork)
    {
        repositoryFactory = _repositoryFactory;
        unitOfWork = _unitOfWork;
    }

    public override async Task<GenericResponse> ExecuteAsync(UpdateFacilityCommand command, CancellationToken ct)
    {
        var facilityRepository = repositoryFactory.CreateRepository<Domain.Entities.Facility>();

        var facility = await facilityRepository.GetByIdAsync(Guid.Parse(command.Id));
        if (facility == null)
        {
            ThrowError("Instalación no encontrada.");
        }

        if(!string.IsNullOrEmpty(command.Name))
        {
            facility.Name = command.Name;
        }
        if(!string.IsNullOrEmpty(command.Location))
        {
            facility.Location = command.Location;
        }
        if(!string.IsNullOrEmpty(command.Type))
        {
            facility.Type = command.Type;
        }
        if(!string.IsNullOrEmpty(command.UsagePolicy))
        {
            facility.UsagePolicy = command.UsagePolicy;
        }
        facility.MaximumCapacity = command.MaximumCapacity;

        facilityRepository.Update(facility);
        await unitOfWork.CommitAsync();

        return new GenericResponse("Instalación Modificada");
    }
}