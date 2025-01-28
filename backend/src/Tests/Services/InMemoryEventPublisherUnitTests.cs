using AutoFixture;
using Playground.Infrastructure.Services;

namespace Playground.Tests.Services;

public class InMemoryEventPublisherUnitTests
{
    private readonly Fixture _fixture;
    private readonly InMemoryEventPublisher _eventPublisher;

    public InMemoryEventPublisherUnitTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _eventPublisher = new InMemoryEventPublisher();
    }

    [Fact]
    public async Task InMemoryEventPublisher_SubscribeAndPublishEvent_EventIsHandled()
    {
        // Arrange
        var eventHandled = false;
        var testEvent = _fixture.Create<TestEvent>();
        _eventPublisher.Subscribe<TestEvent>(e => eventHandled = true);

        // Act
        await _eventPublisher.PublishAsync(testEvent);

        // Assert
        Assert.True(eventHandled);
    }

    [Fact]
    public async Task InMemoryEventPublisher_PublishEventWithoutSubscribers_NoExceptionThrown()
    {
        // Arrange
        var testEvent = _fixture.Create<TestEvent>();

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => _eventPublisher.PublishAsync(testEvent));
        Assert.Null(exception);
    }

    [Fact]
    public async Task InMemoryEventPublisher_SubscribeMultipleHandlers_AllHandlersAreCalled()
    {
        // Arrange
        var handler1Called = false;
        var handler2Called = false;
        var testEvent = _fixture.Create<TestEvent>();

        _eventPublisher.Subscribe<TestEvent>(e => handler1Called = true);
        _eventPublisher.Subscribe<TestEvent>(e => handler2Called = true);

        // Act
        await _eventPublisher.PublishAsync(testEvent);

        // Assert
        Assert.True(handler1Called);
        Assert.True(handler2Called);
    }

    [Fact]
    public async Task InMemoryEventPublisher_SubscribeDifferentEventTypes_OnlyRelevantHandlerIsCalled()
    {
        // Arrange
        var testEvent1Handled = false;
        var testEvent2Handled = false;
        var testEvent1 = _fixture.Create<TestEvent>();
        var testEvent2 = _fixture.Create<AnotherTestEvent>();

        _eventPublisher.Subscribe<TestEvent>(e => testEvent1Handled = true);
        _eventPublisher.Subscribe<AnotherTestEvent>(e => testEvent2Handled = true);

        // Act
        await _eventPublisher.PublishAsync(testEvent1);
        await _eventPublisher.PublishAsync(testEvent2);

        // Assert
        Assert.True(testEvent1Handled);
        Assert.True(testEvent2Handled);
    }

    [Fact]
    public async Task InMemoryEventPublisher_UnsubscribeHandler_HandlerIsNotCalled()
    {
        // Arrange
        var handlerCalled = false;
        var testEvent = _fixture.Create<TestEvent>();
        Action<TestEvent> handler = e => handlerCalled = true;

        _eventPublisher.Subscribe(handler);
        _eventPublisher.Unsubscribe<TestEvent>();

        // Act
        await _eventPublisher.PublishAsync(testEvent);

        // Assert
        Assert.False(handlerCalled);
    }

    private class TestEvent { }
    private class AnotherTestEvent { }
}