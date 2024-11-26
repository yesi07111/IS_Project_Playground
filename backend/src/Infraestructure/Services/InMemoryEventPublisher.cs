using System.Collections.Concurrent;
using Playground.Application.Services;

namespace Playground.Infrastructure.Services
{
    public class InMemoryEventPublisher : IEventPublisher
    {
        private readonly ConcurrentDictionary<Type, Action<object>> _subscribers = new();

        public Task PublishAsync<TEvent>(TEvent eventMessage, CancellationToken cancellationToken = default)
        {
            if (_subscribers.TryGetValue(typeof(TEvent), out var action))
            {
                action(eventMessage!);
            }
            return Task.CompletedTask;
        }

        public void Subscribe<TEvent>(Action<TEvent> handler)
        {
            _subscribers[typeof(TEvent)] = (e) => handler((TEvent)e);
        }
    }
}