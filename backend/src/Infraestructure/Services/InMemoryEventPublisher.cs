using System.Collections.Concurrent;
using Playground.Application.Services;

namespace Playground.Infrastructure.Services
{
    /// <summary>
    /// Implementación en memoria del publicador de eventos, que permite suscribir, publicar y desuscribir manejadores de eventos.
    /// </summary>
    public class InMemoryEventPublisher : IEventPublisher
    {
        private readonly ConcurrentDictionary<Type, List<Action<object>>> _subscribers = new();

        /// <summary>
        /// Publica un evento de tipo <typeparamref name="TEvent"/> a todos los suscriptores registrados.
        /// </summary>
        /// <typeparam name="TEvent">El tipo del evento que se va a publicar.</typeparam>
        /// <param name="eventMessage">El mensaje del evento que se va a publicar.</param>
        /// <param name="cancellationToken">El token de cancelación para la operación asincrónica (opcional).</param>
        /// <returns>Tarea asincrónica que representa la operación de publicación del evento.</returns>
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

        /// <summary>
        /// Suscribe un manejador de eventos para eventos de tipo <typeparamref name="TEvent"/>.
        /// </summary>
        /// <typeparam name="TEvent">El tipo del evento al que se suscribirá el manejador.</typeparam>
        /// <param name="handler">El manejador que se ejecutará cuando se publique el evento.</param>
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

        /// <summary>
        /// Elimina todos los manejadores de eventos registrados para el tipo de evento <typeparamref name="TEvent"/>.
        /// </summary>
        /// <typeparam name="TEvent">El tipo de evento cuyos manejadores serán eliminados.</typeparam>
        public void Unsubscribe<TEvent>()
        {
            _subscribers.TryRemove(typeof(TEvent), out _);
        }
    }
}
