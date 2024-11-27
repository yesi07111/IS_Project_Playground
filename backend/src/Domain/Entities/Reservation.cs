//newLaura

using Playground.Domain.Entities.Auth;

namespace Playground.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; } 
        public DateTime Date { get; set; } 
        public User ParentId { get; set; } 
        public Facility Facility { get; set; }
        public string AdditionalComments { get; set; } 
        public int AmmountOfChildren { get; set; } 
        public string ReservationState { get; set; } 
    }
}