//newLaura 

using System.Diagnostics;
using Playground.Domain.Entities.Auth;

namespace Playground.Domain.Entities
{
    public class Activity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); 
        public string Name { get; set; } = string.Empty; //nombre de la actividad
        public DateTime Date { get; set; } //dia de la actividad
        public string Description { get; set; } = string.Empty;
        public int CurrentParticipants { get; set; } = 0;
        public User EducatorId { get; set; } //educador que la gestiona
        public string Type { get; set; } = string.Empty;
        public int RecommendedAge { get; set; }
        public bool ItsPrivate { get; set; }
        public Facility Facility { get; set; } //en que instalacion se va a hacer
        public DateTime CreatedAt { get; set; } //fecha y hora de creacion de la entidad
        public DateTime UpdateAt { get; set; } //fecha y hora de actualizacion de la entidad
        public DateTime DeletedAt { get; set; } //fecha y hora de eliminacion de la entidad
        public bool IsDeleted { get; set; } 

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