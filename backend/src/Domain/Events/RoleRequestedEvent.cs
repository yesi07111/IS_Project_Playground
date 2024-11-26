namespace Playground.Domain.Events
{
    public class RoleRequestedEvent
    {
        public string UserId { get; }
        public string Role { get; }

        public RoleRequestedEvent(string userId, string role)
        {
            UserId = userId;
            Role = role;
        }
    }
}