using Playground.Domain.Entities.Auth;

namespace Playground.Application.Services
{
    /// <summary>
    /// Define los métodos y propiedades para gestionar el acceso fallido de un usuario, incluyendo el incremento del contador de intentos fallidos y la obtención del tiempo restante de bloqueo.
    /// </summary>
    public interface IAccessFailedService
    {
        /// <summary>
        /// Incrementa el contador de intentos fallidos para un usuario basado en su identificador.
        /// Si el número máximo de intentos fallidos se alcanza, bloquea al usuario temporalmente.
        /// </summary>
        /// <param name="identifier">El identificador del usuario (correo electrónico o nombre de usuario).</param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        Task IncrementAccessFailedCountAsync(string identifier);

        /// <summary>
        /// Obtiene el tiempo restante de bloqueo para un usuario, si está bloqueado.
        /// </summary>
        /// <param name="user">El usuario cuyo tiempo de bloqueo se desea obtener.</param>
        /// <returns>Una tarea que devuelve el tiempo restante de bloqueo en minutos o null si el usuario no está bloqueado.</returns>
        Task<int?> GetLockoutTimeRemainingAsync(User user);

        /// <summary>
        /// Obtiene el número máximo de intentos fallidos permitidos antes de que se bloquee al usuario.
        /// </summary>
        int MaxFailedAccessAttempts { get; }

        /// <summary>
        /// Obtiene la duración en minutos del bloqueo del usuario después de alcanzar el número máximo de intentos fallidos.
        /// </summary>
        int LockoutDurationInMinutes { get; }
    }
}
