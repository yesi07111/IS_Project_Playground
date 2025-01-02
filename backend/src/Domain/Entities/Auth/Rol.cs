namespace Playground.Domain.Entities.Auth;

public class Rol
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public ICollection<User> Users { get; set; } = [];
}