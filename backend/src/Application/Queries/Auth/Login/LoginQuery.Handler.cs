using Playground.Application.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Responses;
using Playground.Domain.SmartEnum;
using Playground.Application.Factories;
using Playground.Domain.Specifications;

namespace Playground.Application.Queries.Auth.Login
{
    public class LoginQueryHandler(IRepositoryFactory repositoryFactory, UserManager<Domain.Entities.Auth.User> userManager, IJwtGenerator jwtGenerator, ILoginProvider loginProviderId, IAccessFailedService accessFailedService) : CommandHandler<LoginQuery, UserActionResponse>
    {
        public override async Task<UserActionResponse> ExecuteAsync(LoginQuery query, CancellationToken ct = default)
        {
            var userRepository = repositoryFactory.CreateRepository<Domain.Entities.Auth.User>();
            var userNameSpecification = UserSpecification.ByUserName(query.Identifier);
            var emailSpecification = UserSpecification.ByEmail(query.Identifier);
            var user = (await userRepository.GetBySpecificationAsync(userNameSpecification, u => u.Rol)).FirstOrDefault() ??
                        (await userRepository.GetBySpecificationAsync(emailSpecification, u => u.Rol)).FirstOrDefault();

            if (user is null)
            {
                await accessFailedService.IncrementAccessFailedCountAsync(query.Identifier);
                ThrowError($"El usuario con identificador '{query.Identifier}' no se encuentra registrado.");
            }

            if (await userManager.IsLockedOutAsync(user))
            {
                var remainingMinutes = await accessFailedService.GetLockoutTimeRemainingAsync(user);
                ThrowError($"La cuenta está bloqueada. Intente nuevamente después de {remainingMinutes ?? accessFailedService.LockoutDurationInMinutes} minutos.");
            }

            if (!await userManager.CheckPasswordAsync(user, query.Password))
            {
                await userManager.AccessFailedAsync(user);
                if (await userManager.GetAccessFailedCountAsync(user) >= accessFailedService.MaxFailedAccessAttempts)
                {
                    await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(accessFailedService.LockoutDurationInMinutes));
                    ThrowError($"Ha alcanzado el máximo de intentos fallidos. La cuenta está bloqueada por {accessFailedService.LockoutDurationInMinutes} minutos.");
                }
                ThrowError("Contraseña equivocada.");
            }

            var token = jwtGenerator.GetToken(user);
            var loginProvider = LoginProviderSmartEnum.Default;
            await userManager.SetAuthenticationTokenAsync(user, loginProvider.Name, TokenTypeSmartEnum.AccessToken.Name, token);

            var userLoginInfo = new UserLoginInfo(loginProvider.Name, loginProviderId.GetProviderId(userManager, loginProvider), user.UserName);
            await userManager.AddLoginAsync(user, userLoginInfo);

            return new UserActionResponse(Guid.Parse(user.Id),
                                          user.UserName!,
                                          token,
                                          user.Rol!.Name);
        }
    }
}