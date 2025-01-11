using Playground.Domain.Entities.Common;

namespace Playground.Domain.Entities
{
    public class ResourceDate :IBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Resource Resource { get; set; }

        public DateOnly Date { get; set; }

        public int UseFrequency { get; set; } 

        public DateTime CreatedAt { get ; set; } = DateTime.UtcNow;

        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeletedAt { get ; set; } = null;
    }
}