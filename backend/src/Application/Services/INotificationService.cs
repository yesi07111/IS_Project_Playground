using Playground.Domain.Entities;
using Playground.Domain.Events;

namespace Playground.Application.Services
{
    /// <summary>
    /// Interfaz para un servicio de notificaciones que gestiona el envío y recuperación de notificaciones.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Notifica a los administradores cuando un usuario solicita un rol.
        /// </summary>
        /// <param name="roleRequestedEvent">Evento que contiene la información de la solicitud de rol.</param>
        /// <param name="cancellationToken">Token de cancelación para la operación.</param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        Task NotifyAdminsAsync(RoleRequestedEvent roleRequestedEvent, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene las notificaciones de un usuario específico.
        /// </summary>
        /// <param name="userId">El identificador del usuario.</param>
        /// <returns>Una lista de notificaciones para el usuario.</returns>
        List<Notification> GetUserNotifications(string userId);
    }
}