using System.ComponentModel.DataAnnotations.Schema;
using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;

namespace Playground.Application.Commands.Resource.Delete;

public class DeleteResourceCommandHandler : CommandHandler<DeleteResourceCommand, GenericResponse>
{
    private readonly IRepositoryFactory repositoryFactory;
    private readonly IUnitOfWork unitOfWork;

    public DeleteResourceCommandHandler(IRepositoryFactory _repositoryFactory, IUnitOfWork _unitOfWork)
    {
        repositoryFactory = _repositoryFactory;
        unitOfWork = _unitOfWork;
    }

    public override async Task<GenericResponse> ExecuteAsync(DeleteResourceCommand command, CancellationToken ct)
    {
        var resourceRepository = repositoryFactory.CreateRepository<Domain.Entities.Resource>();

        var resource = await resourceRepository.GetByIdAsync(Guid.Parse(command.Id));
        if (resource is null)
        {
            ThrowError("No se encontró el recurso.");
        }
        else
        {
            resourceRepository.Delete(resource);
            await unitOfWork.CommitAsync();
        }

        return new GenericResponse("Recurso Borrado");
    }
}