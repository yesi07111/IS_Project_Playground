using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Application.Services;
using Playground.Domain.Entities.Auth;
using Playground.Domain.Specifications;

namespace Playground.Application.Commands.User.Create;

public class CreateUserCommandHandler : CommandHandler<CreateUserCommand, GenericResponse>
{
    private readonly IRepositoryFactory repositoryFactory;
    private readonly IUnitOfWork unitOfWork;

    private readonly UserManager<Domain.Entities.Auth.User> userManager;

    public CreateUserCommandHandler(IRepositoryFactory _repositoryFactory, IUnitOfWork _unitOfWork, UserManager<Domain.Entities.Auth.User> _userManager)
    {
        repositoryFactory = _repositoryFactory;
        unitOfWork = _unitOfWork;
        userManager = _userManager;
    }

    public override async Task<GenericResponse> ExecuteAsync(CreateUserCommand command, CancellationToken ct)
    {
        // Buscar el rol existente por nombre
        var rolRepository = repositoryFactory.CreateRepository<Rol>();
        var nameRolSpecification = RolSpecification.ByName(command.Role);

        var existingRol = (await rolRepository.GetBySpecificationAsync(nameRolSpecification)).FirstOrDefault();

        if (existingRol == null)
        {
            ThrowError("El rol especificado no existe.");
        }

        var user = new Domain.Entities.Auth.User
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            UserName = command.UserName,
            Email = command.Email,
            RolId = existingRol.Id,
        };

        var result = await userManager.CreateAsync(user, command.Password);

        if (!result.Succeeded)
        {
            ThrowError("No se pudo crear al usuario.");
        }  

        await unitOfWork.CommitAsync();

        return new GenericResponse("Usuario creado satisfactoriamente.");
    }
}