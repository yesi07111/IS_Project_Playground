using Playground.Application.Services;
using Playground.Domain.Entities.Auth;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Domain.Specifications;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.Auth.Register;

public class RegisterCommandHandler(UserManager<User> userManager, IEmailSenderService emailSender, IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork) : CommandHandler<RegisterCommand, UserCreationResponse>
{
    public override async Task<UserCreationResponse> ExecuteAsync(RegisterCommand command, CancellationToken ct = default)
    {
        System.Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
        System.Console.WriteLine("HERE COMES THE GENERAL! RISE UP!");
        System.Console.WriteLine("Rol: {0}", command.Rol);
        // Buscar el rol existente por nombre
        var rolRepository = repositoryFactory.CreateRepository<Rol>();
        var nameRolSpecification = RolSpecification.ByName(command.Rol);

        System.Console.WriteLine("nameRolSpecification: {0}", nameRolSpecification);

        var existingRol = (await rolRepository.GetBySpecificationAsync(nameRolSpecification)).FirstOrDefault();

        System.Console.WriteLine("existingRol: {0}", existingRol);

        if (existingRol == null)
        {
            ThrowError("El rol especificado no existe.");
        }

        var user = new User
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

        var userRepository = repositoryFactory.CreateRepository<User>();
        userRepository.Update(user);

        await unitOfWork.CommitAsync();

        await emailSender.SendConfirmationLinkAsync(user, user.Email!, code);

        return new UserCreationResponse(Guid.Parse(user.Id), user.UserName);
    }
}