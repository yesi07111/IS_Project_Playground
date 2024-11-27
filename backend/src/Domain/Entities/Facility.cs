//newLaura

namespace Playground.Domain.Entities
{
    public class Facility
    {
        public string Id { get; set; } 
        public string Name { get; set; } 
        public string Location { get; set; } 
        public string Type { get; set; } 
        public int MaximumCapacity { get; set; } 
        public string UsagePolicy { get; set; } 
    }
}