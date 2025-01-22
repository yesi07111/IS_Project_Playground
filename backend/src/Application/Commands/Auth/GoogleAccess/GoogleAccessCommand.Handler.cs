using Playground.Application.Services;
using Playground.Domain.Entities.Auth;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Factories;
using Playground.Domain.Specifications;
using Playground.Application.Responses;
using Playground.Domain.SmartEnum;

namespace Playground.Application.Commands.Auth.GoogleAccess
{
    public class GoogleAccessCommandHandler(UserManager<Domain.Entities.Auth.User> userManager,
                                            IJwtGenerator jwtGenerator,
                                            IRepositoryFactory repositoryFactory,
                                            IAccessFailedService accessFailedService) : CommandHandler<GoogleAccessCommand, UserActionResponse>
    {
        public override async Task<UserActionResponse> ExecuteAsync(GoogleAccessCommand command, CancellationToken ct = default)
        {
            GoogleActionSmartEnum.TryFromName(command.Action, true, out var actionEnum);

            if (actionEnum == GoogleActionSmartEnum.Register)
            {
                // Lógica de registro
                var rolRepository = repositoryFactory.CreateRepository<Rol>();
                var nameRolSpecification = RolSpecification.ByName(command.Rol);

                var existingRol = (await rolRepository.GetBySpecificationAsync(nameRolSpecification)).FirstOrDefault();

                if (existingRol == null)
                {
                    ThrowError("El rol especificado no existe.");
                }

                var user = new Domain.Entities.Auth.User
                {
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    UserName = command.Username,
                    Email = command.Email,
                    RolId = existingRol.Id,
                    EmailConfirmed = command.IsConfirmed == "true"
                };

                var result = await userManager.CreateAsync(user);
                if (!result.Succeeded)
                    ThrowError("No se pudo crear al usuario.");

                return new UserActionResponse(Guid.Parse(user.Id), user.UserName!, jwtGenerator.GetToken(user));
            }
            else
            {
                // Lógica de inicio de sesión
                var userInBD = await userManager.FindByEmailAsync(command.Email) ?? await userManager.FindByNameAsync(command.Username);

                if (userInBD == null)
                {
                    await accessFailedService.IncrementAccessFailedCountAsync(command.Email);
                    ThrowError("Su usuario de Google no está registrado.");
                }

                return new UserActionResponse(Guid.Parse(userInBD.Id), userInBD.UserName!, jwtGenerator.GetToken(userInBD));
            }
        }
    }
}