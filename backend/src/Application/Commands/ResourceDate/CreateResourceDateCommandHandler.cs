using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Domain.Entities;
using Playground.Domain.Specifications;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Application.Commands.ResourceDate;

public class CreateResourceDateCommandHandler : CommandHandler<CreateResourceDateCommand, GenericResponse>
{
    private readonly IRepositoryFactory repositoryFactory;
    private readonly IUnitOfWork unitOfWork;

    public CreateResourceDateCommandHandler(IRepositoryFactory _repositoryFactory, IUnitOfWork _unitOfWork)
    {
        repositoryFactory = _repositoryFactory;
        unitOfWork = _unitOfWork;
    }

    public override async Task<GenericResponse> ExecuteAsync(CreateResourceDateCommand command, CancellationToken ct)
    {
        var isNew = true;

        if (!Guid.TryParse(command.ResourceId, out var resourceId))
        {
            throw new ArgumentException("El ID del recurso no es un GUID válido.");
        }

        var resourceDateRepository = repositoryFactory.CreateRepository<Domain.Entities.ResourceDate>();
        var resourceRepository = repositoryFactory.CreateRepository<Resource>();

        //nuscar recurso actual
        var resource = await resourceRepository.GetByIdAsync(resourceId);
        if (resource == null)
        {
            return new GenericResponse("Recurso no encontrado.");
        }

        ISpecification<Domain.Entities.ResourceDate> resourceDateSpecification = new ResourceDateSpecification(resourceDate => true);

        //buscar todas las frecuencias de uso en fechas del recuro actual
        resourceDateSpecification = resourceDateSpecification.And(ResourceDateSpecification.ByResource(command.ResourceId));
        var resourceDates = await resourceDateRepository.GetBySpecificationAsync(resourceDateSpecification);

        if (DateTime.TryParse(command.Date, out DateTime parsedDate))
        {
            //buscar si existe ya una declaracion de frecuencia de uso de ese recurso en esa fecha
            resourceDateSpecification = resourceDateSpecification.And(ResourceDateSpecification.ByDate(parsedDate));
            var resourceDateList = await resourceDateRepository.GetBySpecificationAsync(resourceDateSpecification);

            //si no existe crearla
            if (resourceDateList.Count() == 0)
            {
                //crear entidad nueva
                var resourceDate = new Domain.Entities.ResourceDate
                {
                    Resource = resource!,
                    Date = DateOnly.FromDateTime(parsedDate),
                    UseFrequency = command.UsageFrequency
                };

                await resourceDateRepository.AddAsync(resourceDate);
            } //si existe modificarla sumando con la nueva
            else
            {
                isNew = false;
                //modificar entidad existente sumando frecuencia 
                var resourceDate = resourceDateList.FirstOrDefault();
                resourceDate!.UseFrequency = resourceDate!.UseFrequency + command.UsageFrequency;
                resourceDateRepository.Update(resourceDate);
            }

            //sumar total de frecuencia de uso del recurso
            var total = 0;
            foreach (var x in resourceDates)
            {
                total += x.UseFrequency;
            }
            if (isNew)
            {
                total += command.UsageFrequency;
            }

            resource!.UseFrequency = total;
            resourceRepository.Update(resource);

            await unitOfWork.CommitAsync();
        }

        return new GenericResponse("Operación realizada con éxito.");
    }
}