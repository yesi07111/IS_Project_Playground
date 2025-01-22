using Microsoft.AspNetCore.Identity;
using Playground.Application.Repositories;
using Playground.Application.Services;
using Playground.Domain.Entities.Auth;
using Playground.Domain.SmartEnum;

namespace Playground.Infraestructure.Services
{
    /// <summary>
    /// Implementación de ILoginProvider que devuelve un providerID en dependencia del loginProvider.
    /// </summary>
    public class LoginProvider : ILoginProvider
    {
        /// <summary>
        /// Obtiene el identificador del proveedor de inicio de sesión para un usuario específico.
        /// </summary>
        /// <param name="users">El repositorio de usuarios que contiene la información de los usuarios registrados.</param>
        /// <param name="loginProvider">
        /// El proveedor de inicio de sesión del cual se desea obtener el identificador.
        /// Debe ser un valor de <see cref="LoginProviderSmartEnum"/>.
        /// </param>
        /// <returns>El identificador del proveedor de inicio de sesión asociado al usuario.</returns>
        public string GetProviderId(UserManager<User> users, LoginProviderSmartEnum loginProvider)
        {
            if (loginProvider == LoginProviderSmartEnum.Default)
            {
                return "None";
            }

            return "";
        }
    }
}