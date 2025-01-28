using AutoFixture;
using Moq;
using Playground.Infraestructure.Services;
using Xunit;

namespace Playground.Tests.Services;

public class ConverterServiceUnitTests
{
    private readonly Fixture _fixture;
    private readonly ConverterService _converterService;

    public ConverterServiceUnitTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _converterService = new ConverterService();
    }

    [Theory]
    [InlineData("a,b,c", new[] { "a", "b", "c" })]
    [InlineData("  a , b , c  ", new[] { "a", "b", "c" })]
    [InlineData("", new string[] { })]
    [InlineData("a,,b", new[] { "a", "b" })]
    [InlineData(null, new string[] { })]
    public void SplitStringToStringEnumerable_WithValidInput_ReturnsStringEnumerable(string input, string[] expected)
    {
        // Act
        var result = _converterService.SplitStringToStringEnumerable(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("1,2,3", new[] { 1, 2, 3 })]
    [InlineData("  1 , 2 , 3  ", new[] { 1, 2, 3 })]
    [InlineData("", new int[] { })]
    [InlineData("1,,2", new[] { 1, 2 })]
    [InlineData("1,a,2", new[] { 1, 2 })]
    [InlineData(null, new int[] { })]
    [InlineData("1, 2147483647, -2147483648", new[] { 1, 2147483647, -2147483648 })]
    public void SplitStringToIntEnumerable_WithValidInput_ReturnsIntEnumerable(string input, int[] expected)
    {
        // Act
        var result = _converterService.SplitStringToIntEnumerable(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(new[] { 0, 1, 2 }, new[] { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday })]
    [InlineData(new[] { 5, 6 }, new[] { DayOfWeek.Friday, DayOfWeek.Saturday })]
    [InlineData(new int[] { }, new DayOfWeek[] { })]
    [InlineData(null, new DayOfWeek[] { })]
    [InlineData(new[] { 0, 7 }, new[] { DayOfWeek.Sunday, (DayOfWeek)7 })] // Invalid DayOfWeek value
    public void ConvertIntToDayOfWeek_WithValidInput_ReturnsDayOfWeekEnumerable(int[] input, DayOfWeek[] expected)
    {
        // Act
        var result = _converterService.ConvertIntToDayOfWeek(input);

        // Assert
        Assert.Equal(expected, result);
    }
}