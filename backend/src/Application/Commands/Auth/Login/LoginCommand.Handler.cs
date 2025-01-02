using Playground.Domain.Entities.Auth;
using Playground.Application.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.Auth.Login;

public class LoginCommandHandler(UserManager<User> userManager, IJwtGenerator jwtGenerator) : CommandHandler<LoginCommand, UserActionResponse>
{
    public override async Task<UserActionResponse> ExecuteAsync(LoginCommand command, CancellationToken ct = default)
    {
        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        User? user;

        if (emailRegex.IsMatch(command.Identifier))
        {
            user = await userManager.FindByEmailAsync(command.Identifier);
            if (user is null)
                ThrowError($"'{command.Identifier}' no es un correo electrónico registrado.");
        }
        else
        {
            user = await userManager.FindByNameAsync(command.Identifier);
            if (user is null)
                ThrowError($"'{command.Identifier}' no es un nombre de usuario registrado.");
        }

        if (!await userManager.CheckPasswordAsync(user, command.Password))
            ThrowError("Contraseña equivocada.");

        return new UserActionResponse(Guid.Parse(user.Id),
                                      user.UserName!,
                                      await jwtGenerator.GetToken(user));
    }
}