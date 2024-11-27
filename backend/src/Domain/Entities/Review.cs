//newLaura

using Playground.Domain.Entities.Auth;

namespace Playground.Domain.Entities
{
    public class Review
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Date { get; set; } 
        public User ParentId { get; set; } 
        public Facility Facility { get; set; } 
        public string Comments { get; set; } = string.Empty;
        public int Score { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}