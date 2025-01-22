using Playground.Application.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Responses;

namespace Playground.Application.Commands.Auth.ResetPassword;

public class ResetPasswordCommandHandler(UserManager<Domain.Entities.Auth.User> userManager, ICodeGenerator codeGenerator) : CommandHandler<ResetPasswordCommand, UserCreationResponse>
{
    public override async Task<UserCreationResponse> ExecuteAsync(ResetPasswordCommand command, CancellationToken ct = default)
    {
        var user = await userManager.FindByNameAsync(command.Identifier) ?? await userManager.FindByEmailAsync(command.Identifier);

        if (user is null)
        {
            ThrowError($"Usuario con Identificador: {command.Identifier} no encontrado.");
        }

        var code = codeGenerator.GenerateReducedCode(command.FullCode);

        if (code != command.ReducedCode)
        {
            ThrowError("El código es incorrecto.");
        }

        var result = await userManager.ResetPasswordAsync(user, command.FullCode, command.NewPassword);

        if (!result.Succeeded)
        {
            ThrowError("Error al actualizar la contraseña.");
        }
        return new UserCreationResponse(Guid.Parse(user.Id), user.UserName!);
    }
}