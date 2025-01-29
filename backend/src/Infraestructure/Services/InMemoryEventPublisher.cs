using System.Collections.Concurrent;
using Playground.Application.Services;

namespace Playground.Infrastructure.Services
{
    public class InMemoryEventPublisher : IEventPublisher
    {
        private readonly ConcurrentDictionary<Type, List<Action<object>>> _subscribers = new();

        public Task PublishAsync<TEvent>(TEvent eventMessage, CancellationToken cancellationToken = default)
        {
            if (_subscribers.TryGetValue(typeof(TEvent), out var actions))
            {
                foreach (var action in actions)
                {
                    action(eventMessage!);
                }
            }
            return Task.CompletedTask;
        }

        public void Subscribe<TEvent>(Action<TEvent> handler)
        {
            var eventType = typeof(TEvent);
            var action = new Action<object>(e => handler((TEvent)e));

            _subscribers.AddOrUpdate(eventType,
                new List<Action<object>> { action },
                (type, existingHandlers) =>
                {
                    existingHandlers.Add(action);
                    return existingHandlers;
                });
        }

        public void Unsubscribe<TEvent>()
        {
            _subscribers.TryRemove(typeof(TEvent), out _);
        }
    }
}