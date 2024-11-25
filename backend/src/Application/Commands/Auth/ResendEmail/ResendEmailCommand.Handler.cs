using Playground.Application.Commands.Dtos;
using Playground.Domain.Entities.Auth;
using Playground.Application.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Factories;
using Playground.Application.Repositories;

namespace Playground.Application.Commands.Auth.ResendEmail;

public class ResendEmailCommandHandler(UserManager<User> userManager, IEmailSenderService emailSender, IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork) : CommandHandler<ResendEmailCommand, UserCreationResponse>
{
    public override async Task<UserCreationResponse> ExecuteAsync(ResendEmailCommand command, CancellationToken ct = default)
    {
        var user = await userManager.FindByNameAsync(command.Username);
        if (user == null)
        {
            ThrowError("User not found");
            return null;
        }

        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

        user.FullCode = code;

        var userRepository = repositoryFactory.CreateRepository<User>();
        userRepository.Update(user);

        await unitOfWork.CommitAsync();

        await emailSender.SendConfirmationLinkAsync(user, user.Email!, code);

        return new UserCreationResponse(Guid.Parse(user.Id), user.UserName!);
    }
}