using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Infraestructure.Data.DbContexts;
using Playground.Infraestructure.Repositories;

namespace Playground.Infraestructure.Factories
{
    /// <summary>
    /// Fábrica para crear instancias de repositorios.
    /// Proporciona un método para crear repositorios genéricos utilizando el contexto de base de datos predeterminado.
    /// </summary>
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly DefaultDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RepositoryFactory"/>.
        /// </summary>
        /// <param name="context">El contexto de base de datos utilizado para los repositorios.</param>
        public RepositoryFactory(DefaultDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "Context cannot be null.");
        }

        /// <summary>
        /// Crea un repositorio para una entidad específica.
        /// </summary>
        /// <typeparam name="T">El tipo de entidad para el repositorio.</typeparam>
        /// <returns>Una instancia de <see cref="IRepository{T}"/>.</returns>
        public IRepository<T> CreateRepository<T>() where T : class
        {
            return new Repository<T>(_context);
        }
    }
}