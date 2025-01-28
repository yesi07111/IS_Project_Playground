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

    public NotificationService(IEventPublisher eventPublisher, IRepositoryFactory repositoryFactory)
    {
        _eventPublisher = eventPublisher;
        _repositoryFactory = repositoryFactory;
        _userNotifications = new ConcurrentDictionary<string, List<Notification>>();
        _eventPublisher.Subscribe<RoleRequestedEvent>(async (e) => await NotifyAdminsAsync(e));
    }

    public async Task NotifyAdminsAsync(RoleRequestedEvent roleRequestedEvent, CancellationToken cancellationToken = default)
    {
        var userRepository = _repositoryFactory.CreateRepository<User>();

        var adminSpecification = UserSpecification.ByRol("Admin");
        var admins = await userRepository.GetBySpecificationAsync(adminSpecification);

        foreach (var admin in admins)
        {
            var notification = new Notification("System", $"User {roleRequestedEvent.UserId} requested role {roleRequestedEvent.Role}");
            AddNotification(admin.Id, notification);
        }

        await Task.CompletedTask;
    }

    public List<Notification> GetUserNotifications(string userId)
    {
        _userNotifications.TryGetValue(userId, out var notifications);
        return notifications ?? new List<Notification>();
    }

    // Nuevo método para agregar notificaciones
    public void AddNotification(string userId, Notification notification)
    {
        if (!_userNotifications.ContainsKey(userId))
        {
            _userNotifications[userId] = new List<Notification>();
        }
        _userNotifications[userId].Add(notification);
    }
}