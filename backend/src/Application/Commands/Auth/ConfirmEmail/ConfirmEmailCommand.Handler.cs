using Playground.Application.Commands.Dtos;
using Playground.Domain.Entities.Auth;
using Playground.Application.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;


namespace Playground.Application.Commands.Auth.ConfirmEmail
{
    public class ConfirmEmailCommandHandler(UserManager<User> userManager, IJwtGenerator jwtGenerator, ICodeGenerator codeGenerator) : CommandHandler<ConfirmEmailCommand, UserActionResponse>
    {
        public override async Task<UserActionResponse> ExecuteAsync(ConfirmEmailCommand command, CancellationToken ct = default)
        {
            Console.WriteLine("Paso por aqui con " + command.Code);
            var user = await userManager.FindByNameAsync(command.UserName);
            if (user == null)
            {
                ThrowError("User not found");
                return null;
            }

            string expectedCode = codeGenerator.GenerateReducedCode(user.FullCode);
            Console.WriteLine("Aqui esta el code: {0} y el expectedCode {1}", user.FullCode, command.Code);

            if (command.Code != expectedCode)
            {
                ThrowError("Invalid verification code");
                return null;
            }

            var result = await userManager.ConfirmEmailAsync(user, user.FullCode);

            Console.WriteLine("El result de ConfirmEmailAsync es {0}", result.Succeeded);
            if (!result.Succeeded)
            {
                ThrowError("Could not confirm user's email");
                return null;
            }

            return new UserActionResponse(Guid.Parse(user.Id), user.UserName!, await jwtGenerator.GetToken(user));
        }
    }
}