using Playground.Application.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Application.Responses;

namespace Playground.Application.Queries.Auth.ResendEmail;

public class ResendEmailQueryHandler(UserManager<Domain.Entities.Auth.User> userManager, IEmailSenderService emailSender, IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork) : CommandHandler<ResendEmailQuery, UserCreationResponse>
{
    public override async Task<UserCreationResponse> ExecuteAsync(ResendEmailQuery query, CancellationToken ct = default)
    {
        var user = await userManager.FindByNameAsync(query.Username);
        if (user == null)
        {
            ThrowError($"Usuario con nombre de usuario: {query.Username} no encontrado.");
            return null;
        }

        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

        user.FullCode = code;

        var userRepository = repositoryFactory.CreateRepository<Domain.Entities.Auth.User>();
        userRepository.Update(user);

        await unitOfWork.CommitAsync();

        await emailSender.SendConfirmationLinkAsync(user, user.Email!, code);

        return new UserCreationResponse(Guid.Parse(user.Id), user.UserName!);
    }
}