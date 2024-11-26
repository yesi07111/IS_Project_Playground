using Microsoft.AspNetCore.Identity;

namespace Playground.Domain.Entities.Auth;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public DateTime DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    public Guid DeleteToken { get; set; }
}