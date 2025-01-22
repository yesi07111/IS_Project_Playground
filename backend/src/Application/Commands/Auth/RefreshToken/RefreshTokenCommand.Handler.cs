using Playground.Application.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Responses;

namespace Playground.Application.Commands.Auth.RefreshToken;

public class RefreshTokenCommandHandler(UserManager<Domain.Entities.Auth.User> userManager, IJwtGenerator jwtGenerator) : CommandHandler<RefreshTokenCommand, UserActionResponse>
{
    public override async Task<UserActionResponse> ExecuteAsync(RefreshTokenCommand command, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(command.UserId);

        if (user is null)
        {
            ThrowError($"Usuario con Identificador: {command.UserId} no encontrado.");
        }

        return new UserActionResponse(Guid.Parse(user.Id), user.UserName!, jwtGenerator.GetToken(user), "Parent");
    }
}
