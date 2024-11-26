using System.Collections.Concurrent;
using Playground.Application.Factories;
using Playground.Application.Services;
using Playground.Domain.Entities;
using Playground.Domain.Entities.Auth;
using Playground.Domain.Events;

namespace Playground.Infrastructure.Services
{
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
            var admins = await userRepository.GetAllAdminsAsync();

            foreach (var admin in admins)
            {
                var notification = new Notification("System", $"User {roleRequestedEvent.UserId} requested role {roleRequestedEvent.Role}");
                if (!_userNotifications.ContainsKey(admin.Id))
                {
                    _userNotifications[admin.Id] = new List<Notification>();
                }
                _userNotifications[admin.Id].Add(notification);
            }

            await Task.CompletedTask;
        }

        public List<Notification> GetUserNotifications(string userId)
        {
            _userNotifications.TryGetValue(userId, out var notifications);
            return notifications ?? new List<Notification>();
        }
    }
}