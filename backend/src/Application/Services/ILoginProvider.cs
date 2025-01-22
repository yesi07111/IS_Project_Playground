using Microsoft.AspNetCore.Identity;
using Playground.Domain.Entities.Auth;
using Playground.Domain.SmartEnum;

namespace Playground.Application.Services
{
    /// <summary>
    /// Define métodos para manejar los proveedores de inicio de sesión de usuarios.
    /// </summary>
    public interface ILoginProvider
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
        string GetProviderId(UserManager<User> users, LoginProviderSmartEnum loginProvider);
    }
}
