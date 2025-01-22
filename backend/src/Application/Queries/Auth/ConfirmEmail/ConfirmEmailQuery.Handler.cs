using Playground.Application.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Responses;


namespace Playground.Application.Queries.Auth.ConfirmEmail;

public class ConfirmEmailQueryHandler(UserManager<Domain.Entities.Auth.User> userManager, IJwtGenerator jwtGenerator, ICodeGenerator codeGenerator) : CommandHandler<ConfirmEmailQuery, UserActionResponse>
{
    public override async Task<UserActionResponse> ExecuteAsync(ConfirmEmailQuery query, CancellationToken ct = default)
    {
        var user = await userManager.FindByNameAsync(query.Username);
        if (user == null)
        {
            ThrowError($"Usuario con nombre de usuario: {query.Username} no encontrado.");
            return null;
        }

        string expectedCode = codeGenerator.GenerateReducedCode(user.FullCode);

        if (query.Code != expectedCode)
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

        return new UserActionResponse(Guid.Parse(user.Id), user.UserName!, jwtGenerator.GetToken(user));
    }
}
