//newLaura

namespace Playground.Domain.Entities
{
    public class Resource
    {
        public int Id { get; set; } 
        public string Name { get; set; } 
        public string Type { get; set; } 
        public string Location { get; set; } 
        public float UseFrecuency { get; set; }
        public string ResourceCondition { get; set; } 
        public Facility Facility { get; set; } 
    }
}