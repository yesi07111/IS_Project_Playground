using Playground.Application.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Domain.Specifications;
using Playground.Application.Responses;

namespace Playground.Application.Commands.User.Update;

public class UpdateUserCommandHandler(UserManager<Domain.Entities.Auth.User> userManager, IEmailSenderService emailSender, IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork) : CommandHandler<UpdateUserCommand, UpdateUserResponse>
{
    public override async Task<UpdateUserResponse> ExecuteAsync(UpdateUserCommand command, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(command.Id);
        if (user == null)
        {
            ThrowError("Usuario no encontrado.");
        }

        // Actualizar solo si los campos no son nulos o vacíos
        if (!string.IsNullOrEmpty(command.FirstName))
        {
            user.FirstName = command.FirstName;
        }

        if (!string.IsNullOrEmpty(command.LastName))
        {
            user.LastName = command.LastName;
        }

        if (!string.IsNullOrEmpty(command.Username))
        {
            user.UserName = command.Username;
        }

        if (!string.IsNullOrEmpty(command.Email) && command.Email != user.Email)
        {
            user.Email = command.Email;
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            user.FullCode = code;
            await emailSender.SendConfirmationLinkAsync(user, user.Email, code);
        }

        if (!string.IsNullOrEmpty(command.Password))
        {
            var passwordChangeResult = await userManager.ChangePasswordAsync(user, command.OldPassword, command.Password);
            if (!passwordChangeResult.Succeeded)
            {
                ThrowError("No se pudo cambiar la contraseña.");
            }
        }

        var userRepository = repositoryFactory.CreateRepository<Domain.Entities.Auth.User>();
        userRepository.Update(user);

        await unitOfWork.CommitAsync();

        return new UpdateUserResponse(true);
    }
}