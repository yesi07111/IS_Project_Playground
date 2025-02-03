using Microsoft.AspNetCore.Identity;
using Playground.Domain.Entities.Auth;

namespace Playground.Application.Services
{
    /// <summary>
    /// Servicio encargado de gestionar los intentos fallidos de acceso a la cuenta de usuario.
    /// </summary>
    public class AccessFailedService : IAccessFailedService
    {
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Inicializa una nueva instancia del servicio de acceso fallido.
        /// </summary>
        /// <param name="userManager">El gestor de usuarios para interactuar con la base de datos de usuarios.</param>
        public AccessFailedService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Número máximo de intentos fallidos antes de bloquear la cuenta.
        /// </summary>
        public int MaxFailedAccessAttempts => 5;

        /// <summary>
        /// Duración del bloqueo de la cuenta en minutos después de alcanzar el número máximo de intentos fallidos.
        /// </summary>
        public int LockoutDurationInMinutes => 15;

        /// <summary>
        /// Incrementa el contador de intentos fallidos de acceso para el usuario especificado.
        /// Si el número de intentos fallidos alcanza el límite, la cuenta se bloquea temporalmente.
        /// </summary>
        /// <param name="identifier">El identificador del usuario (correo electrónico o nombre de usuario).</param>
        /// <returns>Tarea asincrónica que representa la operación.</returns>
        public async Task IncrementAccessFailedCountAsync(string identifier)
        {
            var user = await _userManager.FindByEmailAsync(identifier) ?? await _userManager.FindByNameAsync(identifier);
            if (user != null)
            {
                await _userManager.AccessFailedAsync(user);
                if (await _userManager.GetAccessFailedCountAsync(user) >= MaxFailedAccessAttempts)
                {
                    await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(LockoutDurationInMinutes));
                }
            }
        }

        /// <summary>
        /// Obtiene el tiempo restante de bloqueo de la cuenta si está bloqueada, de lo contrario retorna null.
        /// </summary>
        /// <param name="user">El usuario cuyo tiempo de bloqueo se desea obtener.</param>
        /// <returns>El tiempo restante de bloqueo en minutos, o null si no está bloqueado.</returns>
        public async Task<int?> GetLockoutTimeRemainingAsync(User user)
        {
            var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
            if (lockoutEnd.HasValue && lockoutEnd.Value > DateTimeOffset.UtcNow)
            {
                return (int)(lockoutEnd.Value - DateTimeOffset.UtcNow).TotalMinutes;
            }
            return null;
        }
    }
}
