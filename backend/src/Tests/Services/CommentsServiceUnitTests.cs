using AutoFixture;
using Moq;
using Playground.Application.Repositories;
using Playground.Domain.Entities;
using Playground.Domain.Entities.Auth;
using Playground.Domain.Specifications.BaseSpecifications;
using Playground.Infraestructure.Services;
using System.Linq.Expressions;

namespace Playground.Tests.Services;

public class CommentsServiceUnitTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IRepository<Review>> _reviewRepositoryMock;
    private readonly CommentsService _commentsService;

    public CommentsServiceUnitTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _reviewRepositoryMock = new Mock<IRepository<Review>>();
        _commentsService = new CommentsService();
    }

    [Fact]
    public async Task CommentsService_WithValidActivityId_ReturnsComments()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var reviews = _fixture.Build<Review>()
            .With(r => r.Parent, _fixture.Build<User>().Create())
            .CreateMany(3)
            .ToList();

        _reviewRepositoryMock.Setup(repo => repo.GetBySpecificationAsync(It.IsAny<ISpecification<Review>>(), It.IsAny<Expression<Func<Review, object>>>()))
            .ReturnsAsync(reviews);

        // Act
        var comments = await _commentsService.GetCommentsAsync(activityId, _reviewRepositoryMock.Object);

        // Assert
        Assert.NotNull(comments);
        Assert.Equal(3, comments.Count());
        foreach (var review in reviews)
        {
            var expectedComment = $"{review.Parent.UserName}:{review.Score}:{review.Comments}";
            Assert.Contains(expectedComment, comments);
        }
    }

    [Fact]
    public async Task CommentsService_WithNoReviews_ReturnsEmptyList()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var reviews = new List<Review>();

        _reviewRepositoryMock.Setup(repo => repo.GetBySpecificationAsync(It.IsAny<ISpecification<Review>>(), It.IsAny<Expression<Func<Review, object>>>()))
            .ReturnsAsync(reviews);

        // Act
        var comments = await _commentsService.GetCommentsAsync(activityId, _reviewRepositoryMock.Object);

        // Assert
        Assert.NotNull(comments);
        Assert.Empty(comments);
    }

    [Fact]
    public async Task CommentsService_WithNullReviews_ReturnsEmptyList()
    {
        // Arrange
        var activityId = Guid.NewGuid();

        _reviewRepositoryMock.Setup(repo => repo.GetBySpecificationAsync(It.IsAny<ISpecification<Review>>(), It.IsAny<Expression<Func<Review, object>>[]>()))
            .ReturnsAsync((IEnumerable<Review>?)null!);

        // Act
        var comments = await _commentsService.GetCommentsAsync(activityId, _reviewRepositoryMock.Object);

        // Assert
        Assert.NotNull(comments);
        Assert.Empty(comments);
    }

    [Theory]
    [InlineData("user1", 5, "Great activity!")]
    [InlineData("user2", 3, "It was okay.")]
    [InlineData("user3", 1, "Not good at all.")]
    public async Task CommentsService_WithVariousReviews_ReturnsFormattedComments(string userName, int score, string comment)
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var review = _fixture.Build<Review>()
            .With(r => r.Parent, _fixture.Build<User>().With(p => p.UserName, userName).Create())
            .With(r => r.Score, score)
            .With(r => r.Comments, comment)
            .Create();

        var reviews = new List<Review> { review };

        _reviewRepositoryMock.Setup(repo => repo.GetBySpecificationAsync(It.IsAny<ISpecification<Review>>(), It.IsAny<Expression<Func<Review, object>>>()))
            .ReturnsAsync(reviews);

        // Act
        var comments = await _commentsService.GetCommentsAsync(activityId, _reviewRepositoryMock.Object);

        // Assert
        Assert.NotNull(comments);
        Assert.Single(comments);
        var expectedComment = $"{userName}:{score}:{comment}";
        Assert.Contains(expectedComment, comments);
    }

    [Fact]
    public async Task CommentsService_WithMixedScores_ReturnsFormattedComments()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var reviews = _fixture.Build<Review>()
            .With(r => r.Parent, _fixture.Build<User>().Create())
            .With(r => r.Score, 5)
            .CreateMany(2)
            .ToList();

        var lowScoreReview = _fixture.Build<Review>()
            .With(r => r.Parent, _fixture.Build<User>().Create())
            .With(r => r.Score, 1)
            .Create();

        reviews.Add(lowScoreReview);

        _reviewRepositoryMock.Setup(repo => repo.GetBySpecificationAsync(It.IsAny<ISpecification<Review>>(), It.IsAny<Expression<Func<Review, object>>>()))
            .ReturnsAsync(reviews);

        // Act
        var comments = await _commentsService.GetCommentsAsync(activityId, _reviewRepositoryMock.Object);

        // Assert
        Assert.NotNull(comments);
        Assert.Equal(3, comments.Count());
        foreach (var review in reviews)
        {
            var expectedComment = $"{review.Parent.UserName}:{review.Score}:{review.Comments}";
            Assert.Contains(expectedComment, comments);
        }
    }

    [Fact]
    public async Task CommentsService_WithEmptyComments_ReturnsFormattedComments()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var reviews = _fixture.Build<Review>()
            .With(r => r.Parent, _fixture.Build<User>().Create())
            .With(r => r.Comments, string.Empty)
            .CreateMany(2)
            .ToList();

        _reviewRepositoryMock.Setup(repo => repo.GetBySpecificationAsync(It.IsAny<ISpecification<Review>>(), It.IsAny<Expression<Func<Review, object>>>()))
            .ReturnsAsync(reviews);

        // Act
        var comments = await _commentsService.GetCommentsAsync(activityId, _reviewRepositoryMock.Object);

        // Assert
        Assert.NotNull(comments);
        Assert.Equal(2, comments.Count());
        foreach (var review in reviews)
        {
            var expectedComment = $"{review.Parent.UserName}:{review.Score}:{review.Comments}";
            Assert.Contains(expectedComment, comments);
        }
    }

    [Fact]
    public async Task CommentsService_WithMultipleReviews_ReturnsAllFormattedComments()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var reviews = _fixture.Build<Review>()
            .With(r => r.Parent, _fixture.Build<User>().Create())
            .CreateMany(5)
            .ToList();

        _reviewRepositoryMock.Setup(repo => repo.GetBySpecificationAsync(It.IsAny<ISpecification<Review>>(), It.IsAny<Expression<Func<Review, object>>>()))
            .ReturnsAsync(reviews);

        // Act
        var comments = await _commentsService.GetCommentsAsync(activityId, _reviewRepositoryMock.Object);

        // Assert
        Assert.NotNull(comments);
        Assert.Equal(5, comments.Count());
        foreach (var review in reviews)
        {
            var expectedComment = $"{review.Parent.UserName}:{review.Score}:{review.Comments}";
            Assert.Contains(expectedComment, comments);
        }
    }
}