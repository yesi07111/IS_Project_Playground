using System.Linq.Expressions;
using AutoFixture;
using Moq;
using Playground.Application.Repositories;
using Playground.Domain.Entities;
using Playground.Domain.Specifications.BaseSpecifications;
using Playground.Infraestructure.Services;
using Xunit;

namespace Playground.Tests.Services;

public class RatingServiceUnitTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IRepository<Review>> _reviewRepositoryMock;
    private readonly RatingService _ratingService;

    public RatingServiceUnitTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _reviewRepositoryMock = new Mock<IRepository<Review>>();
        _ratingService = new RatingService();
    }

    [Fact]
    public void CalculateAverageRating_WithNoReviews_ReturnsZero()
    {
        // Arrange
        var activityDate = _fixture.Create<ActivityDate>();
        _reviewRepositoryMock.Setup(repo => repo.GetBySpecificationAsync(It.IsAny<ISpecification<Review>>(), It.IsAny<Expression<Func<Review, object>>>(), It.IsAny<Expression<Func<Review, object>>>()))
                             .ReturnsAsync(new List<Review>());

        // Act
        var result = _ratingService.CalculateAverageRating(activityDate, _reviewRepositoryMock.Object);

        // Assert
        Assert.Equal(0.0, result);
    }

    [Fact]
    public void CalculateAverageRating_WithReviews_ReturnsAverageScore()
    {
        // Arrange
        var activityDate = _fixture.Create<ActivityDate>();
        var reviews = _fixture.Build<Review>()
                              .With(r => r.Score, 4.0)
                              .CreateMany(5)
                              .ToList();
        _reviewRepositoryMock.Setup(repo => repo.GetBySpecificationAsync(It.IsAny<ISpecification<Review>>(), It.IsAny<Expression<Func<Review, object>>>(), It.IsAny<Expression<Func<Review, object>>>()))
                             .ReturnsAsync(reviews);

        // Act
        var result = _ratingService.CalculateAverageRating(activityDate, _reviewRepositoryMock.Object);

        // Assert
        Assert.Equal(4.0, result);
    }

    [Theory]
    [InlineData(3.5, 4.5, 5.0, 4.3)]
    [InlineData(2.0, 3.0, 4.0, 3.0)]
    [InlineData(1.0, 1.0, 1.0, 1.0)]
    public void CalculateAverageRating_WithVariousScores_ReturnsCorrectAverage(double score1, double score2, double score3, double expectedAverage)
    {
        // Arrange
        var activityDate = _fixture.Create<ActivityDate>();
        var reviews = new List<Review>
        {
            _fixture.Build<Review>().With(r => r.Score, score1).Create(),
            _fixture.Build<Review>().With(r => r.Score, score2).Create(),
            _fixture.Build<Review>().With(r => r.Score, score3).Create()
        };
        _reviewRepositoryMock.Setup(repo => repo.GetBySpecificationAsync(It.IsAny<ISpecification<Review>>(), It.IsAny<Expression<Func<Review, object>>>(), It.IsAny<Expression<Func<Review, object>>>()))
                             .ReturnsAsync(reviews);

        // Act
        var result = _ratingService.CalculateAverageRating(activityDate, _reviewRepositoryMock.Object);

        // Assert
        Assert.Equal(expectedAverage, result);
    }

    [Theory]
    [InlineData(3.5, 4.5, 5.0, 2, 4.33)]
    [InlineData(2.0, 3.0, 4.0, 1, 3.0)]
    [InlineData(1.0, 1.0, 1.0, 0, 1.0)]
    public void CalculateAverageRating_WithVariousScoresAndRounding_ReturnsCorrectAverage(double score1, double score2, double score3, int round, double expectedAverage)
    {
        // Arrange
        var activityDate = _fixture.Create<ActivityDate>();
        var reviews = new List<Review>
        {
            _fixture.Build<Review>().With(r => r.Score, score1).Create(),
            _fixture.Build<Review>().With(r => r.Score, score2).Create(),
            _fixture.Build<Review>().With(r => r.Score, score3).Create()
        };
        _reviewRepositoryMock.Setup(repo => repo.GetBySpecificationAsync(It.IsAny<ISpecification<Review>>(), It.IsAny<Expression<Func<Review, object>>>(), It.IsAny<Expression<Func<Review, object>>>()))
                             .ReturnsAsync(reviews);

        // Act
        var result = _ratingService.CalculateAverageRating(activityDate, _reviewRepositoryMock.Object, round);

        // Assert
        Assert.Equal(expectedAverage, result);
    }
}