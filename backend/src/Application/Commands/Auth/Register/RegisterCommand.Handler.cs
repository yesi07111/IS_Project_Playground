using Playground.Application.Services;
using Playground.Domain.Entities.Auth;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Domain.Specifications;
using Playground.Application.Responses;

namespace Playground.Application.Commands.Auth.Register;

public class RegisterCommandHandler(UserManager<Domain.Entities.Auth.User> userManager, IEmailSenderService emailSender, IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork) : CommandHandler<RegisterCommand, UserCreationResponse>
{
    public override async Task<UserCreationResponse> ExecuteAsync(RegisterCommand command, CancellationToken ct = default)
    {
        // Buscar el rol existente por nombre
        var rolRepository = repositoryFactory.CreateRepository<Rol>();
        var nameRolSpecification = RolSpecification.ByName(command.Rol);

        var existingRol = (await rolRepository.GetBySpecificationAsync(nameRolSpecification)).FirstOrDefault();

        if (existingRol == null)
        {
            ThrowError("El rol especificado no existe.");
        }

        var user = new Domain.Entities.Auth.User
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            UserName = command.Username,
            Email = command.Email,
            RolId = existingRol.Id
        };

        var result = await userManager.CreateAsync(user, command.Password);

        if (!result.Succeeded)
            ThrowError("No se pudo crear al usuario.");

        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

        user.FullCode = code;

        var userRepository = repositoryFactory.CreateRepository<Domain.Entities.Auth.User>();
        userRepository.Update(user);

        await unitOfWork.CommitAsync();

        await emailSender.SendConfirmationLinkAsync(user, user.Email!, code);

        return new UserCreationResponse(Guid.Parse(user.Id), user.UserName);
    }
}