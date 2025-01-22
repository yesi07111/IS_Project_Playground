using Playground.Application.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Responses;

namespace Playground.Application.Queries.Auth.SendResetPasswordEmail;

public class SendResetPasswordEmailQueryHandler(UserManager<Domain.Entities.Auth.User> userManager, IEmailSenderService emailSender) : CommandHandler<SendResetPasswordEmailQuery, UserActionResponse>
{
    public override async Task<UserActionResponse> ExecuteAsync(SendResetPasswordEmailQuery query, CancellationToken ct = default)
    {
        var user = await userManager.FindByNameAsync(query.Identifier) ?? await userManager.FindByEmailAsync(query.Identifier);
        if (user == null)
        {
            ThrowError($"Usuario con identificador: {query.Identifier} no encontrado.");
            return null;
        }
        var code = await userManager.GeneratePasswordResetTokenAsync(user);

        await emailSender.SendPasswordResetCodeAsync(user, user.Email!, code);

        return new UserActionResponse(Guid.Parse(user.Id), user.UserName!, code, "Parent");
    }
}