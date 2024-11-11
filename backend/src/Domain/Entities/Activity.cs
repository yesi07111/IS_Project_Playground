using System;

namespace Playground.Domain.Entities
{
    public class Activity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public DateTime Date { get; private set; }
        public string Description { get; private set; }
        public int MaxParticipants { get; private set; }
        public int CurrentParticipants { get; private set; }
        public Guid EducatorId { get; private set; }

        public Activity(string name, DateTime date, string description, int maxParticipants, Guid educatorId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Date = date;
            Description = description;
            MaxParticipants = maxParticipants;
            EducatorId = educatorId;
            CurrentParticipants = 0;
        }

        public void AddParticipant()
        {
            if (CurrentParticipants < MaxParticipants)
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