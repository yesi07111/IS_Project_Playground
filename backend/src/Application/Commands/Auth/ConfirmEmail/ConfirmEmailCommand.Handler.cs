using System.Text;
using Playground.Application.Commands.Dtos;
using Playground.Domain.Entities.Auth;
using Playground.Application.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Playground.Application.Commands.Auth.ConfirmEmail;

public class ConfirmEmailCommandHandler(UserManager<User> userManager, IJwtGenerator jwtGenerator) : CommandHandler<ConfirmEmailCommand, UserActionResponse>
{
    public override async Task<UserActionResponse> ExecuteAsync(ConfirmEmailCommand command, CancellationToken ct = default)
    {
        var user = (await userManager.FindByNameAsync(command.Username))!;

        byte[] decodedBytes = WebEncoders.Base64UrlDecode(command.Code);
        string code = Encoding.UTF8.GetString(decodedBytes);

        var result = await userManager.ConfirmEmailAsync(user, code);
        if (!result.Succeeded)
            ThrowError("Could not confirm user's email");

        return new UserActionResponse(Guid.Parse(user.Id), user.UserName!, await jwtGenerator.GetToken(user));
    }
}