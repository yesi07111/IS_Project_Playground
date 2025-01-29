using AutoFixture;
using Moq;
using Playground.Domain.Entities.Auth;
using Playground.Domain.SmartEnum;
using Playground.Infraestructure.Services;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Playground.Tests.Services
{
    public class LoginProviderServiceUnitTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<UserManager<User>> _userManagerMock;

        public LoginProviderServiceUnitTests()
        {
            _fixture = new Fixture();

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        }

        [Fact]
        public void LoginProvider_WithDefaultProvider_ReturnsNone()
        {
            // Arrange
            var loginProviderService = new LoginProvider();
            var loginProvider = LoginProviderSmartEnum.Default;

            // Act
            var result = loginProviderService.GetProviderId(_userManagerMock.Object, loginProvider);

            // Assert
            Assert.Equal("None", result);
        }

        [Fact]
        public void LoginProvider_WithUnknownProvider_ReturnsEmptyString()
        {
            // Arrange
            var loginProviderService = new LoginProvider();
            var loginProvider = (LoginProviderSmartEnum)null!;

            // Act
            var result = loginProviderService.GetProviderId(_userManagerMock.Object, loginProvider);

            // Assert
            Assert.Equal("", result);
        }
    }
}