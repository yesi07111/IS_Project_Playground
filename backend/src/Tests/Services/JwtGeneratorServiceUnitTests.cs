using AutoFixture;
using Moq;
using Playground.Domain.Entities.Auth;
using Playground.Infraestructure.Configurations;
using Playground.Infraestructure.Services;
using Playground.Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace Playground.Tests.Services
{
    public class JwtGeneratorServiceUnitTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IOptions<JwtConfiguration>> _optionsMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IDateTimeService> _dateTimeServiceMock;
        private readonly JwtGenerator _jwtGenerator;

        public JwtGeneratorServiceUnitTests()
        {
            _fixture = new Fixture();

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _optionsMock = new Mock<IOptions<JwtConfiguration>>();
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
            _dateTimeServiceMock = new Mock<IDateTimeService>();

            var jwtConfig = new JwtConfiguration
            {
                SecretKey = "thisIsA32CharacterLongSecretKey!", // Asegúrate de que la clave tenga al menos 32 caracteres
                Issuer = "testIssuer",
                Audience = "testAudience",
                LifetimeMinutes = 60
            };

            _optionsMock.Setup(o => o.Value).Returns(jwtConfig);
            _jwtGenerator = new JwtGenerator(_optionsMock.Object, _userManagerMock.Object, _dateTimeServiceMock.Object);
        }

        [Fact]
        public void JwtGenerator_WithValidUser_ReturnsToken()
        {
            // Arrange
            var user = _fixture.Build<User>()
                .With(u => u.UserName, "testuser")
                .With(u => u.Id, "test-user-id")
                .With(u => u.Email, "testuser@example.com")
                .Create();

            _dateTimeServiceMock.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);

            // Act
            var token = _jwtGenerator.GetToken(user);

            // Assert
            Assert.NotNull(token);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            Assert.Equal("testuser", jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
            Assert.Equal("test-user-id", jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            Assert.Equal("testuser@example.com", jwtToken.Claims.First(c => c.Type == ClaimTypes.Email).Value);
        }

        [Fact]
        public void JwtGenerator_WithNullUser_ReturnsTokenWithEmptyClaims()
        {
            // Arrange
            var user = _fixture.Build<User>()
                .With(u => u.UserName, (string?)null)
                .With(u => u.Id, (string?)null)
                .With(u => u.Email, (string?)null)
                .Create();

            _dateTimeServiceMock.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);

            // Act
            var token = _jwtGenerator.GetToken(user);

            // Assert
            Assert.NotNull(token);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            Assert.Equal(string.Empty, jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
            Assert.Equal(string.Empty, jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            Assert.Equal(string.Empty, jwtToken.Claims.First(c => c.Type == ClaimTypes.Email).Value);
        }

        [Theory]
        [InlineData("user1", "user1-id", "user1@example.com")]
        [InlineData("user2", "user2-id", "user2@example.com")]
        public void JwtGenerator_WithDifferentUsers_ReturnsCorrectToken(string userName, string userId, string email)
        {
            // Arrange
            var user = _fixture.Build<User>()
                .With(u => u.UserName, userName)
                .With(u => u.Id, userId)
                .With(u => u.Email, email)
                .Create();

            _dateTimeServiceMock.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);

            // Act
            var token = _jwtGenerator.GetToken(user);

            // Assert
            Assert.NotNull(token);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            Assert.Equal(userName, jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
            Assert.Equal(userId, jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            Assert.Equal(email, jwtToken.Claims.First(c => c.Type == ClaimTypes.Email).Value);
        }

        [Fact]
        public void JwtGenerator_WithExpiredToken_ReturnsExpiredToken()
        {
            // Arrange
            var user = _fixture.Build<User>()
                .With(u => u.UserName, "testuser")
                .With(u => u.Id, "test-user-id")
                .With(u => u.Email, "testuser@example.com")
                .Create();

            // Simula que la fecha actual es 70 minutos en el futuro para que el token expire
            _dateTimeServiceMock.Setup(d => d.UtcNow).Returns(DateTime.UtcNow.AddMinutes(-70));

            // Act
            var token = _jwtGenerator.GetToken(user);

            // Assert
            Assert.NotNull(token);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            Assert.True(jwtToken.ValidTo < DateTime.UtcNow, "The token should be expired.");
        }
    }
}