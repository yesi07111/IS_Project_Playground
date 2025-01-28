using AutoFixture;
using Moq;
using Playground.Domain.Specifications.BaseSpecifications;
using Playground.Infraestructure.Data.DbContexts;
using Playground.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Playground.Domain.Entities.Common;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Playground.Tests.Repositories;

public class RepositoryUnitTests
{
    private readonly Fixture _fixture;
    private readonly Mock<DefaultDbContext> _contextMock;
    private readonly Repository<Entity> _repository;
    private readonly UnitOfWork _unitOfWork;

    public RepositoryUnitTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var options = new DbContextOptionsBuilder<DefaultDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _contextMock = new Mock<DefaultDbContext>(options);
        _repository = new Repository<Entity>(_contextMock.Object);
        _unitOfWork = new UnitOfWork(_contextMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidGuidId_ReturnsEntity()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = _fixture.Build<Entity>().With(e => e.Id, id).Create();
        var dbSetMock = CreateDbSetMock([entity]);
        _contextMock.Setup(x => x.Set<Entity>()).Returns(dbSetMock.Object);

        // Act
        var result = await _repository.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }
    [Fact]
    public async Task GetBySpecificationAsync_WithValidSpecification_ReturnsEntities()
    {
        // Arrange
        var entities = _fixture.CreateMany<Entity>(5).ToList();
        var specificationMock = new Mock<ISpecification<Entity>>();
        specificationMock.Setup(x => x.ToExpression()).Returns(e => true);
        var dbSetMock = CreateDbSetMock(entities);
        _contextMock.Setup(x => x.Set<Entity>()).Returns(dbSetMock.Object);

        // Act
        var result = await _repository.GetBySpecificationAsync(specificationMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllEntities()
    {
        // Arrange
        var entities = _fixture.CreateMany<Entity>(5).ToList();
        var dbSetMock = CreateDbSetMock(entities);
        _contextMock.Setup(x => x.Set<Entity>()).Returns(dbSetMock.Object);

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count());
    }

    [Fact]
    public async Task AddAsync_WithValidEntity_AddsEntity()
    {
        // Arrange
        var entity = _fixture.Create<Entity>();
        var dbSetMock = new Mock<DbSet<Entity>>();
        _contextMock.Setup(x => x.Set<Entity>()).Returns(dbSetMock.Object);

        // Act
        await _repository.AddAsync(entity);
        await _unitOfWork.CommitAsync();

        // Assert
        dbSetMock.Verify(x => x.AddAsync(entity, default), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    private Mock<DbSet<T>> CreateDbSetMock<T>(List<T> entities) where T : class
    {
        var queryable = entities.AsQueryable();
        var dbSetMock = new Mock<DbSet<T>>();

        dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

        dbSetMock.As<IAsyncEnumerable<T>>().Setup(m => m.GetAsyncEnumerator(default))
            .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

        dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<T>(queryable.Provider));

        dbSetMock.Setup(d => d.FindAsync(It.IsAny<object[]>())).ReturnsAsync((object[] ids) => entities.FirstOrDefault(e => ((Entity)(object)e).Id == (Guid)ids[0]));

        return dbSetMock;
    }

    [Fact]
    public async Task Update_WithValidEntity_UpdatesEntity()
    {
        // Arrange
        var entity = _fixture.Create<Entity>();
        var dbSetMock = CreateDbSetMock(new List<Entity> { entity });
        _contextMock.Setup(x => x.Set<Entity>()).Returns(dbSetMock.Object);

        // Act
        _repository.Update(entity);
        await _unitOfWork.CommitAsync();

        // Assert
        dbSetMock.Verify(x => x.Update(entity), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Delete_WithValidEntity_DeletesEntity()
    {
        // Arrange
        var entity = _fixture.Create<Entity>();
        var dbSetMock = CreateDbSetMock(new List<Entity> { entity });
        _contextMock.Setup(x => x.Set<Entity>()).Returns(dbSetMock.Object);

        // Act
        _repository.Delete(entity);
        await _unitOfWork.CommitAsync();

        // Assert
        dbSetMock.Verify(x => x.Remove(entity), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task MarkDeleted_WithEntityEntity_MarksAsDeleted()
    {
        // Arrange
        var Entity = _fixture.Create<Entity>();
        var dbSetMock = CreateDbSetMock(new List<Entity> { Entity });
        _contextMock.Setup(x => x.Set<Entity>()).Returns(dbSetMock.Object);

        // Act
        _repository.MarkDeleted(Entity);
        await _unitOfWork.CommitAsync();

        // Assert
        Assert.NotNull(Entity.DeletedAt);
        dbSetMock.Verify(x => x.Update(Entity), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task GetBySpecificationAsync_WithIncludes_ReturnsEntities()
    {
        // Arrange
        var entities = _fixture.CreateMany<Entity>(5).ToList();
        var specificationMock = new Mock<ISpecification<Entity>>();
        specificationMock.Setup(x => x.ToExpression()).Returns(e => true);
        var dbSetMock = CreateDbSetMock(entities);
        _contextMock.Setup(x => x.Set<Entity>()).Returns(dbSetMock.Object);

        // Act
        var result = await _repository.GetBySpecificationAsync(specificationMock.Object, e => e.CreatedAt);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_WithIncludes_ReturnsAllEntities()
    {
        // Arrange
        var entities = _fixture.CreateMany<Entity>(5).ToList();
        var dbSetMock = CreateDbSetMock(entities);
        _contextMock.Setup(x => x.Set<Entity>()).Returns(dbSetMock.Object);

        // Act
        var result = await _repository.GetAllAsync(e => e.CreatedAt);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count());
    }

}
public class Entity : IBaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdateAt { get; set; } = DateTime.Now;
    public DateTime? DeletedAt { get; set; } = null;
}

// Helper classes for async queryable
internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    internal TestAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(Expression expression)
    {
        return new TestAsyncEnumerable<TEntity>(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new TestAsyncEnumerable<TElement>(expression);
    }

    public object Execute(Expression expression)
    {
        return _inner.Execute(expression)!;
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return _inner.Execute<TResult>(expression);
    }

    public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
    {
        return new TestAsyncEnumerable<TResult>(expression);
    }

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        return Execute<TResult>(expression);
    }
}

public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable)
    { }

    public TestAsyncEnumerable(Expression expression) : base(expression)
    { }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }

    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);

}

internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }

    public ValueTask DisposeAsync()
    {
        _inner.Dispose();
        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> MoveNextAsync()
    {
        return new ValueTask<bool>(_inner.MoveNext());
    }

    public T Current => _inner.Current;
}