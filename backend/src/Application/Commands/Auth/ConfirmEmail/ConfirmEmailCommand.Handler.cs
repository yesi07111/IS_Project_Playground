using Playground.Domain.Entities.Auth;
using Playground.Application.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Commands.Responses;


namespace Playground.Application.Commands.Auth.ConfirmEmail
{
    public class ConfirmEmailCommandHandler(UserManager<User> userManager, IJwtGenerator jwtGenerator, ICodeGenerator codeGenerator) : CommandHandler<ConfirmEmailCommand, UserActionResponse>
    {
        public override async Task<UserActionResponse> ExecuteAsync(ConfirmEmailCommand command, CancellationToken ct = default)
        {
            var user = await userManager.FindByNameAsync(command.UserName);
            if (user == null)
            {
                ThrowError($"Usuario con nombre de usuario: {command.UserName} no encontrado.");
                return null;
            }

            string expectedCode = codeGenerator.GenerateReducedCode(user.FullCode);

            if (command.Code != expectedCode)
            {
                ThrowError("Código de verificación inválido.");
                return null;
            }

            var result = await userManager.ConfirmEmailAsync(user, user.FullCode);

            if (!result.Succeeded)
            {
                ThrowError("No se pudo confirmar el correo electrónico del usuario.");
                return null;
            }

            return new UserActionResponse(Guid.Parse(user.Id), user.UserName!, await jwtGenerator.GetToken(user), user.Rol.Name);
        }
    }
}