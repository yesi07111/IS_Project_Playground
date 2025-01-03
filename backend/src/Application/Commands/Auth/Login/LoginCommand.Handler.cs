using Playground.Domain.Entities.Auth;
using Playground.Application.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using Playground.Application.Commands.Responses;
using Microsoft.EntityFrameworkCore;
using Playground.Domain.SmartEnum;

namespace Playground.Application.Commands.Auth.Login;

public class LoginCommandHandler(UserManager<User> userManager, IJwtGenerator jwtGenerator, ILoginProvider loginProviderId) : CommandHandler<LoginCommand, UserActionResponse>
{
    public override async Task<UserActionResponse> ExecuteAsync(LoginCommand command, CancellationToken ct = default)
    {
        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        User? user;

        if (emailRegex.IsMatch(command.Identifier))
        {
            user = await userManager.Users.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Email == command.Identifier);
            if (user is null)
                ThrowError($"'{command.Identifier}' no es un correo electrónico registrado.");
        }
        else
        {
            user = await userManager.Users.Include(u => u.Rol).FirstOrDefaultAsync(u => u.UserName == command.Identifier);
            if (user is null)
                ThrowError($"'{command.Identifier}' no es un nombre de usuario registrado.");
        }

        if (!await userManager.CheckPasswordAsync(user, command.Password))
            ThrowError("Contraseña equivocada.");

        var token = await jwtGenerator.GetToken(user);
        var loginProvider = LoginProviderSmartEnum.Default;
        await userManager.SetAuthenticationTokenAsync(user, loginProvider.Name, TokenTypeSmartEnum.AccessToken.Name, token);

        var userLoginInfo = new UserLoginInfo(loginProvider.Name, loginProviderId.GetProviderId(userManager, loginProvider), user.UserName);
        await userManager.AddLoginAsync(user, userLoginInfo);

        return new UserActionResponse(Guid.Parse(user.Id),
                                      user.UserName!,
                                      token,
                                      user.Rol.Name);
    }
}