using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Playground.Application.Commands.CleanUp;

namespace Playground.Infraestructure.Services
{
    /// <summary>
    /// Servicio que se ejecuta en segundo plano para realizar tareas de limpieza periódicas.
    /// Implementa IHostedService para integrarse con el ciclo de vida del host de la aplicación.
    /// </summary>
    public class CleanUpService : IHostedService, IDisposable
    {
        private readonly ILogger<CleanUpService> _logger;
        private Timer? _timer;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CleanUpService"/>.
        /// </summary>
        /// <param name="logger">Instancia de <see cref="ILogger"/> para registrar información de depuración.</param>
        /// <param name="serviceProvider">Proveedor de servicios para crear alcances de servicio.</param>
        public CleanUpService(ILogger<CleanUpService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Inicia el servicio de limpieza, configurando un temporizador para ejecutar tareas periódicas.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para detener el servicio.</param>
        /// <returns>Una tarea completada.</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CleanUp Service is starting.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(24)); // Ejecuta cada 24 horas
            return Task.CompletedTask;
        }

        /// <summary>
        /// Realiza las tareas de limpieza, como eliminar usuarios no verificados.
        /// </summary>
        /// <param name="state">Estado del temporizador (no utilizado).</param>
        private void DoWork(object? state)
        {
            _logger.LogInformation("CleanUp Service is working.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetRequiredService<CleanUpUnverifiedUsersCommandHandler>();
                handler.ExecuteAsync().Wait();
            }
        }

        /// <summary>
        /// Detiene el servicio de limpieza.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para detener el servicio.</param>
        /// <returns>Una tarea completada.</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CleanUp Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Libera los recursos utilizados por el servicio.
        /// </summary>
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}