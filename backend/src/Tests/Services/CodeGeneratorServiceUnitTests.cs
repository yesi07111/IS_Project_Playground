using AutoFixture;
using Playground.Infraestructure.Services;

namespace Playground.Tests.Services;

public class CodeGeneratorServiceUnitTests
{
    private readonly Fixture _fixture;
    private readonly CodeGenerator _codeGenerator;

    public CodeGeneratorServiceUnitTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _codeGenerator = new CodeGenerator();
    }

    [Theory]
    [InlineData("test-input", 6)]
    [InlineData("another-test-input", 8)]
    [InlineData("short", 4)]
    public void CodeGenerator_WithValidInput_ReturnsReducedCode(string input, int length)
    {
        // Act
        var result = _codeGenerator.GenerateReducedCode(input, length);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(length, result.Length);
    }

    [Theory]
    [InlineData("", 6)]
    [InlineData(null, 6)]
    public void CodeGenerator_WithInvalidInput_ThrowsArgumentNullException(string input, int length)
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _codeGenerator.GenerateReducedCode(input, length));
    }

    [Fact]
    public void CodeGenerator_WithDefaultLength_ReturnsCodeOfDefaultLength()
    {
        // Arrange
        var input = _fixture.Create<string>();

        // Act
        var result = _codeGenerator.GenerateReducedCode(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(6, result.Length);
    }

    [Theory]
    [InlineData("test-input", 0)]
    [InlineData("test-input", -1)]
    [InlineData("test-input", 100)]
    public void CodeGenerator_WithInvalidLength_ThrowsArgumentOutOfRangeException(string input, int length)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => _codeGenerator.GenerateReducedCode(input, length));
    }

    [Fact]
    public void CodeGenerator_WithEmptyString_ReturnsEmptyCode()
    {
        // Act && Assert
        Assert.Throws<ArgumentNullException>(() => _codeGenerator.GenerateReducedCode(string.Empty, 6));
    }

    [Fact]
    public void CodeGenerator_WithLongInput_ReturnsReducedCode()
    {
        // Arrange
        var input = new string('a', 1000);

        // Act
        var result = _codeGenerator.GenerateReducedCode(input, 6);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(6, result.Length);
    }
}