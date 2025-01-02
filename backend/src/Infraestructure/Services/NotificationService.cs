using System.Collections.Concurrent;
using Playground.Application.Factories;
using Playground.Application.Services;
using Playground.Domain.Entities;
using Playground.Domain.Entities.Auth;
using Playground.Domain.Events;
using Playground.Domain.Specifications;

namespace Playground.Infrastructure.Services;

/// <summary>
/// Servicio de notificaciones que gestiona el envío de notificaciones a los usuarios.
/// Utiliza un publicador de eventos y un repositorio para gestionar las notificaciones.
/// </summary>
public class NotificationService : INotificationService
{
    private readonly IEventPublisher _eventPublisher;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly ConcurrentDictionary<string, List<Notification>> _userNotifications;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="NotificationService"/>.
    /// </summary>
    /// <param name="eventPublisher">Publicador de eventos para gestionar eventos de notificación.</param>
    /// <param name="repositoryFactory">Fábrica de repositorios para acceder a los datos de usuario.</param>
    public NotificationService(IEventPublisher eventPublisher, IRepositoryFactory repositoryFactory)
    {
        _eventPublisher = eventPublisher;
        _repositoryFactory = repositoryFactory;
        _userNotifications = new ConcurrentDictionary<string, List<Notification>>();
        _eventPublisher.Subscribe<RoleRequestedEvent>(async (e) => await NotifyAdminsAsync(e));
    }

    /// <summary>
    /// Notifica a los administradores cuando un usuario solicita un rol.
    /// </summary>
    /// <param name="roleRequestedEvent">Evento que contiene la información de la solicitud de rol.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    public async Task NotifyAdminsAsync(RoleRequestedEvent roleRequestedEvent, CancellationToken cancellationToken = default)
    {
        var userRepository = _repositoryFactory.CreateRepository<User>();

        // Usar la especificación para obtener solo los administradores
        var adminSpecification = UserSpecification.ByRol("Admin");
        var admins = await userRepository.GetBySpecificationAsync(adminSpecification);

        foreach (var admin in admins)
        {
            var notification = new Notification("System", $"User {roleRequestedEvent.UserId} requested role {roleRequestedEvent.Role}");
            if (!_userNotifications.ContainsKey(admin.Id))
            {
                _userNotifications[admin.Id] = [];
            }
            _userNotifications[admin.Id].Add(notification);
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// Obtiene las notificaciones de un usuario específico.
    /// </summary>
    /// <param name="userId">El identificador del usuario.</param>
    /// <returns>Una lista de notificaciones para el usuario.</returns>
    public List<Notification> GetUserNotifications(string userId)
    {
        _userNotifications.TryGetValue(userId, out var notifications);
        return notifications ?? [];
    }
}