//newLaura

using Playground.Domain.Entities.Auth;

namespace Playground.Domain.Entities
{
    public class Reservation(int id, DateTime date, User parentId, Facility facility, string additionalComments, int ammountOfChildren, string reservationState)
    {
        public int Id { get; private set; } = id;
        public DateTime Date { get; private set; } = date;
        public User ParentId { get; private set; } = parentId;
        public Facility Facility { get; private set; } = facility;
        public string AdditionalComments { get; private set; } = additionalComments;
        public int AmmountOfChildren { get; private set; } = ammountOfChildren;
        public string ReservationState { get; private set; } = reservationState;
    }
}