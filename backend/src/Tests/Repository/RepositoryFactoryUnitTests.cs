using AutoFixture;
using Moq;
using Playground.Infraestructure.Data.DbContexts;
using Playground.Infraestructure.Factories;
using Playground.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Playground.Tests.Repository;

public class RepositoryFactoryUniTests
{
    private readonly Fixture _fixture;
    private readonly Mock<DefaultDbContext> _contextMock;
    private readonly RepositoryFactory _repositoryFactory;

    public RepositoryFactoryUniTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        // Crear opciones de DbContext simuladas
        var options = new DbContextOptionsBuilder<DefaultDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        // Crear un Mock de DefaultDbContext usando las opciones simuladas
        _contextMock = new Mock<DefaultDbContext>(options);
        _repositoryFactory = new RepositoryFactory(_contextMock.Object);
    }

    [Fact]
    public void RepositoryFactory_WithValidContext_CreatesRepository()
    {
        // Act
        var repository = _repositoryFactory.CreateRepository<TestEntity>();

        // Assert
        Assert.NotNull(repository);
        Assert.IsType<Repository<TestEntity>>(repository);
    }

    [Fact]
    public void RepositoryFactory_WithNullContext_ThrowsArgumentNullException()
    {
        // Arrange
        DefaultDbContext? nullContext = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new RepositoryFactory(nullContext!));
    }

    [Fact]
    public void RepositoryFactory_CreateRepository_ReturnsNewInstanceEachTime()
    {
        // Act
        var repository1 = _repositoryFactory.CreateRepository<TestEntity>();
        var repository2 = _repositoryFactory.CreateRepository<TestEntity>();

        // Assert
        Assert.NotSame(repository1, repository2);
    }

    private class TestEntity { }
    private class AnotherEntity { }
}