//newLaura 

using Playground.Domain.Entities.Auth;

namespace Playground.Domain.Entities
{
    public class Activity
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } 
        public DateTime Date { get; set; } 
        public string Description { get; set; } 
        public int CurrentParticipants { get; set; } = 0;
        public User EducatorId { get; set; } 
        public string Type { get; set; }
        public int RecommendedAge { get; set; } 
        public bool ItsPrivate { get; set; } 
        public Facility Facility { get; set; } 

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