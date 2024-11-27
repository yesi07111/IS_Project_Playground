//newLaura

namespace Playground.Domain.Entities
{
    public class Resource
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public float UseFrecuency { get; set; }
        public string ResourceCondition { get; set; } = string.Empty;
        public Facility Facility { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}