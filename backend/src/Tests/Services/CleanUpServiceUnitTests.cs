using Moq;
using Playground.Infraestructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Playground.Application.Commands.CleanUp;
using Playground.Application.Repositories;
using Playground.Application.Factories;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Tests.Services
{
    public class CleanUpServiceUnitTests
    {
        private readonly Mock<ILogger<CleanUpService>> _loggerMock;
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
        private readonly Mock<IServiceScope> _serviceScopeMock;
        private readonly Mock<IRepositoryFactory> _repositoryFactoryMock;
        private readonly Mock<IRepository<Domain.Entities.Auth.User>> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CleanUpUnverifiedUsersCommandHandler _cleanUpHandler;

        public CleanUpServiceUnitTests()
        {
            _loggerMock = new Mock<ILogger<CleanUpService>>();
            _serviceProviderMock = new Mock<IServiceProvider>();
            _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            _serviceScopeMock = new Mock<IServiceScope>();
            _repositoryFactoryMock = new Mock<IRepositoryFactory>();
            _userRepositoryMock = new Mock<IRepository<Domain.Entities.Auth.User>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _repositoryFactoryMock.Setup(x => x.CreateRepository<Domain.Entities.Auth.User>())
                .Returns(_userRepositoryMock.Object);

            _cleanUpHandler = new CleanUpUnverifiedUsersCommandHandler(_repositoryFactoryMock.Object, _unitOfWorkMock.Object);

            _serviceProviderMock.Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(_serviceScopeFactoryMock.Object);
            _serviceScopeFactoryMock.Setup(x => x.CreateScope())
                .Returns(_serviceScopeMock.Object);
            _serviceScopeMock.Setup(x => x.ServiceProvider)
                .Returns(_serviceProviderMock.Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(CleanUpUnverifiedUsersCommandHandler)))
                .Returns(_cleanUpHandler);
        }

        [Fact]
        public async Task CleanUpService_StartAsync_LogsStartingMessage()
        {
            // Arrange
            var cleanUpService = new CleanUpService(_loggerMock.Object, _serviceProviderMock.Object);

            // Act
            await cleanUpService.StartAsync(CancellationToken.None);

            // Assert
            _loggerMock.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("CleanUp Service is starting.")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        }

        [Fact]
        public async Task CleanUpService_StopAsync_LogsStoppingMessage()
        {
            // Arrange
            var cleanUpService = new CleanUpService(_loggerMock.Object, _serviceProviderMock.Object);

            // Act
            await cleanUpService.StopAsync(CancellationToken.None);

            // Assert
            _loggerMock.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("CleanUp Service is stopping.")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        }

        [Fact]
        public void CleanUpService_DoWork_ExecutesCleanUpHandler()
        {
            // Arrange
            var cleanUpService = new CleanUpService(_loggerMock.Object, _serviceProviderMock.Object);

            // Act
            cleanUpService.DoWork(null);

            // Assert
            _userRepositoryMock.Verify(x => x.GetBySpecificationAsync(It.IsAny<ISpecification<Domain.Entities.Auth.User>>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task CleanUpService_Dispose_DisposesTimer()
        {
            // Arrange
            var cleanUpService = new CleanUpService(_loggerMock.Object, _serviceProviderMock.Object);
            await cleanUpService.StartAsync(CancellationToken.None);

            // Act
            cleanUpService.Dispose();

            // Assert
            var timerField = cleanUpService.GetType().GetField("_timer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var timerValue = timerField?.GetValue(cleanUpService);
            Assert.Null(timerValue);
        }
    }
}