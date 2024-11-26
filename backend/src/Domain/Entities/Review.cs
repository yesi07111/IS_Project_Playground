//newLaura

using Playground.Domain.Entities.Auth;

namespace Playground.Domain.Entities
{
    public class Review(int id, DateTime date, User parentId, Facility facility, string comments, int score) 
    {
        public int Id { get; private set; } = id;
        public DateTime Date { get; private set; } = date;
        public User ParentId { get; private set; } = parentId;
        public Facility Facility { get; private set; } = facility;
        public string Comments { get; private set; } = comments;
        public int Score { get; private set; } = score;
    }
}