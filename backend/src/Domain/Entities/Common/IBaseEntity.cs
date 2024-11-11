namespace Playground.Domain.Entities.Common;

public interface IBaseEntity<T> where T : class
{
    public T Id { get; set; }
}