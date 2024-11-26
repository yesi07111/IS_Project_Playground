using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Playground.Application.Commands.CleanUp;
using Playground.Application.Services;

namespace Playground.Infraestructure.Services
{
    public class CleanUpService : ICleanUpService, IHostedService, IDisposable
    {
        private readonly ILogger<CleanUpService> _logger;
        private Timer? _timer;
        private readonly IServiceProvider _serviceProvider;

        public CleanUpService(ILogger<CleanUpService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CleanUp Service is starting.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(24)); // Ejecuta cada 24 horas
            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            _logger.LogInformation("CleanUp Service is working.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var command = new CleanUpUnverifiedUsersCommand();
                var handler = scope.ServiceProvider.GetRequiredService<CleanUpUnverifiedUsersCommandHandler>();
                handler.ExecuteAsync(command).Wait();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CleanUp Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}