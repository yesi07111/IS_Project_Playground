namespace Playground.Application.Services;

/// <summary>
/// Interfaz para un publicador de eventos que gestiona la publicación y suscripción de eventos.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publica un evento de manera asincrónica a todos los suscriptores registrados.
    /// </summary>
    /// <typeparam name="TEvent">El tipo de evento a publicar.</typeparam>
    /// <param name="eventMessage">El mensaje del evento.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    Task PublishAsync<TEvent>(TEvent eventMessage, CancellationToken cancellationToken = default);

    /// <summary>
    /// Suscribe un manejador a un tipo de evento específico.
    /// </summary>
    /// <typeparam name="TEvent">El tipo de evento al que suscribirse.</typeparam>
    /// <param name="handler">El manejador que se ejecutará cuando se publique el evento.</param>
    void Subscribe<TEvent>(Action<TEvent> handler);
}
