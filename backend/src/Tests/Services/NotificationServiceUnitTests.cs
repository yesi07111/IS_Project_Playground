using AutoFixture;
using Moq;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Application.Services;
using Playground.Domain.Entities;
using Playground.Domain.Entities.Auth;
using Playground.Domain.Events;
using Playground.Domain.Specifications;
using Playground.Domain.Specifications.BaseSpecifications;
using Playground.Infrastructure.Services;
using Xunit;

namespace Playground.Tests.Services
{
    public class NotificationServiceUnitTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IEventPublisher> _eventPublisherMock;
        private readonly Mock<IRepositoryFactory> _repositoryFactoryMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly NotificationService _notificationService;

        public NotificationServiceUnitTests()
        {
            _fixture = new Fixture();

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _eventPublisherMock = new Mock<IEventPublisher>();
            _repositoryFactoryMock = new Mock<IRepositoryFactory>();
            _userRepositoryMock = new Mock<IRepository<User>>();

            _repositoryFactoryMock.Setup(x => x.CreateRepository<User>()).Returns(_userRepositoryMock.Object);

            _notificationService = new NotificationService(_eventPublisherMock.Object, _repositoryFactoryMock.Object);
        }

        [Fact]
        public async Task NotificationService_NotifyAdminsAsync_WithValidRoleRequestedEvent_NotifiesAdmins()
        {
            // Arrange
            var roleRequestedEvent = new RoleRequestedEvent("test-user-id", "Admin");

            var adminUsers = new List<User>
            {
                new() { Id = "admin-user-id-1" },
                new() { Id = "admin-user-id-2" }
            };

            _userRepositoryMock.Setup(x => x.GetBySpecificationAsync(It.IsAny<ISpecification<User>>()))
                .ReturnsAsync(adminUsers);

            // Act
            await _notificationService.NotifyAdminsAsync(roleRequestedEvent);

            // Assert
            foreach (var admin in adminUsers)
            {
                var notifications = _notificationService.GetUserNotifications(admin.Id);
                Assert.NotNull(notifications);
                Assert.Single(notifications);
                Assert.Equal("System", notifications[0].Sender);
                Assert.Contains("test-user-id", notifications[0].Message);
                Assert.Contains("Admin", notifications[0].Message);
            }
        }

        [Fact]
        public void NotificationService_GetUserNotifications_WithValidUserId_ReturnsNotifications()
        {
            // Arrange
            var userId = "test-user-id";
            var notifications = new List<Notification>
            {
                new("System", "Test notification 1"),
                new("System", "Test notification 2"),
                new("System", "Test notification 3")
            };

            // Usa el nuevo método para agregar notificaciones
            foreach (var notification in notifications)
            {
                _notificationService.AddNotification(userId, notification);
            }

            // Act
            var result = _notificationService.GetUserNotifications(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.All(result, n => Assert.Equal("System", n.Sender));
            Assert.All(result, n => Assert.Contains("Test notification", n.Message));
        }

        [Fact]
        public void NotificationService_GetUserNotifications_WithInvalidUserId_ReturnsEmptyList()
        {
            // Arrange
            var userId = "invalid-user-id";

            // Act
            var result = _notificationService.GetUserNotifications(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData("Admin")]
        [InlineData("User")]
        public async Task NotificationService_NotifyAdminsAsync_WithDifferentRoles_NotifiesAdmins(string role)
        {
            // Arrange
            var roleRequestedEvent = new RoleRequestedEvent("test-user-id", role);

            var adminUsers = new List<User>
            {
                new() { Id = "admin-user-id-1" },
                new() { Id = "admin-user-id-2" }
            };

            _userRepositoryMock.Setup(x => x.GetBySpecificationAsync(It.IsAny<ISpecification<User>>()))
                .ReturnsAsync(adminUsers);

            // Act
            await _notificationService.NotifyAdminsAsync(roleRequestedEvent);

            // Assert
            foreach (var admin in adminUsers)
            {
                var notifications = _notificationService.GetUserNotifications(admin.Id);
                Assert.NotNull(notifications);
                Assert.Single(notifications);
                Assert.Equal("System", notifications[0].Sender);
                Assert.Contains("test-user-id", notifications[0].Message);
                Assert.Contains(role, notifications[0].Message);
            }
        }
    }
}