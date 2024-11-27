//newLaura

using Playground.Domain.Entities.Auth;

namespace Playground.Domain.Entities
{
    public class Review
    {
        public string Id { get; set; } 
        public DateTime Date { get; set; } 
        public User ParentId { get; set; } 
        public Facility Facility { get; set; } 
        public string Comments { get; set; } 
        public int Score { get; set; } 
    }
}