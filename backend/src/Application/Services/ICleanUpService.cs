namespace Playground.Application.Services
{
    public interface ICleanUpService
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}