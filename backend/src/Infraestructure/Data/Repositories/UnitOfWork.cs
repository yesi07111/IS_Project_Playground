using Playground.Application.Repositories;
using Playground.Infraestructure.Data.DbContexts;

namespace Playground.Infraestructure.Repositories
{
    /// <summary>
    /// Unidad de trabajo que gestiona las transacciones en el contexto de base de datos.
    /// Proporciona métodos para confirmar cambios en el contexto.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DefaultDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UnitOfWork"/>.
        /// </summary>
        /// <param name="context">El contexto de base de datos utilizado para las transacciones.</param>
        public UnitOfWork(DefaultDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Confirma de forma asincrónica todos los cambios realizados en el contexto de base de datos.
        /// </summary>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Confirma todos los cambios realizados en el contexto de base de datos.
        /// </summary>
        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}