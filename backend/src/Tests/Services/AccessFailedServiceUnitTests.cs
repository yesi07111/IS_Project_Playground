using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Moq;
using Playground.Application.Services;
using Playground.Domain.Entities.Auth;

namespace Playground.Tests.Services
{
    public class AccessFailedServiceUnitTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly AccessFailedService _accessFailedService;
        private readonly Fixture _fixture;

        public AccessFailedServiceUnitTests()
        {
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
            _accessFailedService = new AccessFailedService(_userManagerMock.Object);
            _fixture = new Fixture();

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task AccessFailedService_WithValidEmail_IncrementAccessFailedCount()
        {
            // Arrange
            var user = _fixture.Build<User>().With(u => u.Email, "test@example.com").Create();
            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.AccessFailedAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.GetAccessFailedCountAsync(It.IsAny<User>())).ReturnsAsync(1);

            // Act
            await _accessFailedService.IncrementAccessFailedCountAsync("test@example.com");

            // Assert
            _userManagerMock.Verify(um => um.AccessFailedAsync(user), Times.Once);
        }

        [Fact]
        public async Task AccessFailedService_WithValidUsername_IncrementAccessFailedCount()
        {
            // Arrange
            var user = _fixture.Build<User>().With(u => u.UserName, "testuser").Create();
            _userManagerMock.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.AccessFailedAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.GetAccessFailedCountAsync(It.IsAny<User>())).ReturnsAsync(1);

            // Act
            await _accessFailedService.IncrementAccessFailedCountAsync("testuser");

            // Assert
            _userManagerMock.Verify(um => um.AccessFailedAsync(user), Times.Once);
        }

        [Fact]
        public async Task AccessFailedService_WithMaxFailedAttempts_LockoutUser()
        {
            // Arrange
            var user = _fixture.Create<User>();
            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.AccessFailedAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.GetAccessFailedCountAsync(It.IsAny<User>())).ReturnsAsync(5);

            // Act
            await _accessFailedService.IncrementAccessFailedCountAsync("test@example.com");

            // Assert
            _userManagerMock.Verify(um => um.SetLockoutEndDateAsync(user, It.IsAny<DateTimeOffset>()), Times.Once);
        }

        [Fact]
        public async Task AccessFailedService_WithValidUser_GetLockoutTimeRemaining()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var lockoutEnd = DateTimeOffset.UtcNow.AddMinutes(10);
            _userManagerMock.Setup(um => um.GetLockoutEndDateAsync(It.IsAny<User>())).ReturnsAsync(lockoutEnd);

            // Act
            var result = await _accessFailedService.GetLockoutTimeRemainingAsync(user);

            // Assert
            Assert.NotNull(result);
            Assert.InRange(result.Value, 9, 10); // Allowing a small margin of error
        }

        [Fact]
        public async Task AccessFailedService_WithNoLockout_GetLockoutTimeRemaining()
        {
            // Arrange
            var user = _fixture.Create<User>();
            _userManagerMock.Setup(um => um.GetLockoutEndDateAsync(It.IsAny<User>())).ReturnsAsync((DateTimeOffset?)null);

            // Act
            var result = await _accessFailedService.GetLockoutTimeRemainingAsync(user);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AccessFailedService_WithInvalidEmail_DoesNotIncrementAccessFailedCount()
        {
            // Arrange
            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);

            // Act
            await _accessFailedService.IncrementAccessFailedCountAsync("invalid@example.com");

            // Assert
            _userManagerMock.Verify(um => um.AccessFailedAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task AccessFailedService_WithInvalidUsername_DoesNotIncrementAccessFailedCount()
        {
            // Arrange
            _userManagerMock.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((User)null);

            // Act
            await _accessFailedService.IncrementAccessFailedCountAsync("invaliduser");

            // Assert
            _userManagerMock.Verify(um => um.AccessFailedAsync(It.IsAny<User>()), Times.Never);
        }

    }
}