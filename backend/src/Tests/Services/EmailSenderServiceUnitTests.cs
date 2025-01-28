using AutoFixture;
using Moq;
using Playground.Application.Services;
using Playground.Domain.Entities.Auth;
using Playground.Infraestructure.Configurations;
using Playground.Infraestructure.Services;
using Microsoft.Extensions.Options;

namespace Playground.Tests.Services;

public class EmailSenderServiceUnitTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IOptions<EmailConfiguration>> _optionsMock;
    private readonly Mock<ICodeGenerator> _codeGeneratorMock;
    private readonly EmailSenderService _emailSenderService;

    public EmailSenderServiceUnitTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _optionsMock = new Mock<IOptions<EmailConfiguration>>();
        _codeGeneratorMock = new Mock<ICodeGenerator>();

        var emailConfig = _fixture.Build<EmailConfiguration>()
            .With(ec => ec.DebugMode, true)
            .Create();
        _optionsMock.Setup(o => o.Value).Returns(emailConfig);

        _emailSenderService = new EmailSenderService(_optionsMock.Object, _codeGeneratorMock.Object);
    }

    [Fact]
    public async Task EmailSenderService_SendConfirmationLinkAsync_ValidInput_CallsGenerateReducedCode()
    {
        // Arrange
        var user = _fixture.Build<User>()
            .With(u => u.UserName, "testuser")
            .Create();
        var email = "test@example.com";
        var code = "123456";
        var reducedCode = "1234";
        _codeGeneratorMock.Setup(cg => cg.GenerateReducedCode(It.Is<string>(s => s == code), It.IsAny<int>())).Returns(reducedCode);

        // Act
        await _emailSenderService.SendConfirmationLinkAsync(user, email, code);

        // Assert
        _codeGeneratorMock.Verify(cg => cg.GenerateReducedCode(It.Is<string>(s => s == code), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task EmailSenderService_SendPasswordResetCodeAsync_ValidInput_CallsGenerateReducedCode()
    {
        // Arrange
        var user = _fixture.Build<User>()
            .With(u => u.UserName, "testuser")
            .Create();
        var email = "test@example.com";
        var resetCode = "654321";
        var reducedCode = "6543";
        _codeGeneratorMock.Setup(cg => cg.GenerateReducedCode(It.Is<string>(s => s == resetCode), It.IsAny<int>())).Returns(reducedCode);

        // Act
        await _emailSenderService.SendPasswordResetCodeAsync(user, email, resetCode);

        // Assert
        _codeGeneratorMock.Verify(cg => cg.GenerateReducedCode(It.Is<string>(s => s == resetCode), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task EmailSenderService_QueueSendEmailAsync_DebugMode_ReturnsDebug()
    {
        // Arrange
        var to = "test@example.com";
        var subject = "Test Subject";
        var message = "Test Message";

        // Act
        var result = await _emailSenderService.QueueSendEmailAsync(to, subject, message);

        // Assert
        Assert.Equal("DEBUG", result.Value);
    }
}
