using System.Text;
using Playground.Application.Commands.Dtos;
using Playground.Application.Services;
using Playground.Domain.Entities.Auth;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Playground.Application.Commands.Auth.Register;

public class RegisterCommandHandler(UserManager<User> userManager, IEmailSenderService emailSender, IActiveSession activeSession) : CommandHandler<RegisterCommand, UserCreationResponse>
{
    public override async Task<UserCreationResponse> ExecuteAsync(RegisterCommand command, CancellationToken ct = default)
    {
        var user = new User
        {
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
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var confirmUrl = $"{activeSession.BaseUrl()}/api/auth/confirm-email?username={user.UserName}$code={code}";

        await emailSender.SendConfirmationLinkAsync(user, user.Email!, confirmUrl);

        return new UserCreationResponse(Guid.Parse(user.Id), user.UserName);
    }
}