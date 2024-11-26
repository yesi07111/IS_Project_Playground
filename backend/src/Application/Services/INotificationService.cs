using Playground.Domain.Entities;
using Playground.Domain.Events;

namespace Playground.Application.Services
{
    public interface INotificationService
    {
        Task NotifyAdminsAsync(RoleRequestedEvent roleRequestedEvent, CancellationToken cancellationToken = default);
        List<Notification> GetUserNotifications(string userId);
    }
}