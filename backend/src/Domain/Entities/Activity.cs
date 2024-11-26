//newLaura 

using Playground.Domain.Entities.Auth;

namespace Playground.Domain.Entities
{
    public class Activity(int id, string name, DateTime date, string description, User educatorId, string type, int recommendedAge, bool itsPrivate, Facility facility)
    {
        public int Id { get; set; } = id;
        public string Name { get; private set; } = name;
        public DateTime Date { get; private set; } = date;
        public string Description { get; private set; } = description;
        public int CurrentParticipants { get; private set; } = 0;
        public User EducatorId { get; private set; } = educatorId;
        public string Type { get; private set; } = type; 
        public int RecommendedAge { get; private set; } = recommendedAge; 
        public bool ItsPrivate { get; private set; } = itsPrivate; 
        public Facility Facility { get; private set; } = facility; 

        public void AddParticipant()
        {
            if (CurrentParticipants < Facility.MaximumCapacity)
            {
                CurrentParticipants++;
            }
            else
            {
                throw new InvalidOperationException("No se pueden agregar más participantes a esta actividad.");
            }
        }
    }
}