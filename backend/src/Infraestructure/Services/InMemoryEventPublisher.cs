using System.Collections.Concurrent;
using Playground.Application.Services;

namespace Playground.Infrastructure.Services;

/// <summary>
/// Publicador de eventos en memoria que permite la suscripción y publicación de eventos.
/// Utiliza un diccionario concurrente para gestionar los suscriptores.
/// </summary>
public class InMemoryEventPublisher : IEventPublisher
{
    private readonly ConcurrentDictionary<Type, Action<object>> _subscribers = new();

    /// <summary>
    /// Publica un evento a todos los suscriptores registrados para el tipo de evento.
    /// </summary>
    /// <typeparam name="TEvent">El tipo de evento a publicar.</typeparam>
    /// <param name="eventMessage">El mensaje del evento.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación.</param>
    /// <returns>Una tarea completada.</returns>
    public Task PublishAsync<TEvent>(TEvent eventMessage, CancellationToken cancellationToken = default)
    {
        if (_subscribers.TryGetValue(typeof(TEvent), out var action))
        {
            action(eventMessage!);
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Suscribe un manejador a un tipo de evento específico.
    /// </summary>
    /// <typeparam name="TEvent">El tipo de evento al que suscribirse.</typeparam>
    /// <param name="handler">El manejador que se ejecutará cuando se publique el evento.</param>
    public void Subscribe<TEvent>(Action<TEvent> handler)
    {
        _subscribers[typeof(TEvent)] = (e) => handler((TEvent)e);
    }
}