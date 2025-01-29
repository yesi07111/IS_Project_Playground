using System.Diagnostics;
using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;

namespace Playground.Application.Commands.Facility.Delete;

public class DeleteFacilityCommandHandler : CommandHandler<DeleteFacilityCommand, GenericResponse>
{
    private readonly IRepositoryFactory repositoryFactory;
    private readonly IUnitOfWork unitOfWork;

    public DeleteFacilityCommandHandler(IRepositoryFactory _repositoryFactory, IUnitOfWork _unitOfWork)
    {
        repositoryFactory = _repositoryFactory;
        unitOfWork = _unitOfWork;
    }

    public override async Task<GenericResponse> ExecuteAsync(DeleteFacilityCommand command, CancellationToken ct)
    {
        var facilityRepository = repositoryFactory.CreateRepository<Domain.Entities.Facility>();
        var facility = await facilityRepository.GetByIdAsync(Guid.Parse(command.Id)) ?? throw new ArgumentException("La instalación no existe.");

        facilityRepository.Delete(facility);
        unitOfWork.Commit();

        return new GenericResponse("Instalación Eliminada Permanentemente");
    }
}