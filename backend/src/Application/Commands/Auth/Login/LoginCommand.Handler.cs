using Playground.Application.Commands.Dtos;
using Playground.Domain.Entities.Auth;
using Playground.Application.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace Playground.Application.Commands.Auth.Login;

public class LoginCommandHandler(UserManager<User> userManager, IJwtGenerator jwtGenerator) : CommandHandler<LoginCommand, UserActionResponse>
{
    public override async Task<UserActionResponse> ExecuteAsync(LoginCommand command, CancellationToken ct = default)
    {
        var user = await userManager.FindByNameAsync(command.Username);

        if (user is null || !user.EmailConfirmed)
            ThrowError($"User '{command.Username}' does not exists");

        if (!await userManager.CheckPasswordAsync(user, command.Password))
            ThrowError("Wrong Password");

        return new UserActionResponse(Guid.Parse(user.Id),
                                      user.UserName!,
                                      await jwtGenerator.GetToken(user));
    }
}