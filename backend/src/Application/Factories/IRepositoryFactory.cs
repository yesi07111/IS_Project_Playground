using Playground.Application.Repositories;

namespace Playground.Application.Factories;
public interface IRepositoryFactory
{
    IRepository<T> CreateRepository<T>() where T : class;
}
