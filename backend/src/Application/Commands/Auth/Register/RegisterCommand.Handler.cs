using System.Text;
using Playground.Application.Commands.Dtos;
using Playground.Application.Services;
using Playground.Domain.Entities.Auth;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Factories;
using Playground.Application.Repositories;

namespace Playground.Application.Commands.Auth.Register;

public class RegisterCommandHandler(UserManager<User> userManager, IEmailSenderService emailSender, IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork) : CommandHandler<RegisterCommand, UserCreationResponse>
{
    public override async Task<UserCreationResponse> ExecuteAsync(RegisterCommand command, CancellationToken ct = default)
    {
        var user = new User
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            UserName = command.Username,
            Email = command.Email
        };

        var result = await userManager.CreateAsync(user, command.Password);

        if (!result.Succeeded)
            ThrowError("Could not create user");

        var roleResult = await userManager.AddToRolesAsync(user, command.Roles);

        if (!roleResult.Succeeded)
            ThrowError("Could not add roles to user");

        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

        user.FullCode = code;

        var userRepository = repositoryFactory.CreateRepository<User>();
        userRepository.Update(user);

        await unitOfWork.CommitAsync();

        await emailSender.SendConfirmationLinkAsync(user, user.Email!, code);

        return new UserCreationResponse(Guid.Parse(user.Id), user.UserName);
    }
}