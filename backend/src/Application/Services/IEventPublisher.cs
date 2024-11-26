namespace Playground.Application.Services
{
    public interface IEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent eventMessage, CancellationToken cancellationToken = default);
        void Subscribe<TEvent>(Action<TEvent> handler);
    }
}