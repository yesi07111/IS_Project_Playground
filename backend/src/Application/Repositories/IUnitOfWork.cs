namespace Playground.Application.Repositories;
public interface IUnitOfWork
{
    Task CommitAsync();
}
