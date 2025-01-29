using AutoFixture;
using Xunit;
using Playground.Infraestructure.Services;
using System;

namespace Playground.Tests.Services;

public class DateTimeServiceUnitTests
{
    private readonly Fixture _fixture;
    private readonly DateTimeService _dateTimeService;

    public DateTimeServiceUnitTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _dateTimeService = new DateTimeService();
    }

    [Fact]
    public void DateTimeService_Now_ReturnsCurrentLocalDateTime()
    {
        // Act
        var now = _dateTimeService.Now;

        // Assert
        Assert.Equal(DateTime.Now.ToString("yyyy-MM-dd HH:mm"), now.ToString("yyyy-MM-dd HH:mm"));
    }

    [Fact]
    public void DateTimeService_UtcNow_ReturnsCurrentUtcDateTime()
    {
        // Act
        var utcNow = _dateTimeService.UtcNow;

        // Assert
        Assert.Equal(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm"), utcNow.ToString("yyyy-MM-dd HH:mm"));
    }

    [Fact]
    public void DateTimeService_Today_ReturnsCurrentDate()
    {
        // Act
        var today = _dateTimeService.Today;

        // Assert
        Assert.Equal(DateOnly.FromDateTime(DateTime.Now), today);
    }

    [Fact]
    public void DateTimeService_Time_ReturnsCurrentTime()
    {
        // Act
        var time = _dateTimeService.Time;

        // Assert
        Assert.Equal(TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm"), time.ToString("HH:mm"));
    }

    [Fact]
    public void DateTimeService_Now_IsNotInFuture()
    {
        // Act
        var now = _dateTimeService.Now;

        // Assert
        Assert.True(now <= DateTime.Now);
    }

    [Fact]
    public void DateTimeService_UtcNow_IsNotInFuture()
    {
        // Act
        var utcNow = _dateTimeService.UtcNow;

        // Assert
        Assert.True(utcNow <= DateTime.UtcNow);
    }

    [Fact]
    public void DateTimeService_Today_IsNotInFuture()
    {
        // Act
        var today = _dateTimeService.Today;

        // Assert
        Assert.True(today <= DateOnly.FromDateTime(DateTime.Now));
    }

    [Fact]
    public void DateTimeService_Time_IsNotInFuture()
    {
        // Act
        var time = _dateTimeService.Time;

        // Assert
        Assert.True(time <= TimeOnly.FromDateTime(DateTime.Now));
    }
}