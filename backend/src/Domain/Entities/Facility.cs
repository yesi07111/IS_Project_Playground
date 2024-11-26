//newLaura

namespace Playground.Domain.Entities
{
    public class Facility(int id, string name, string location, string type, int maximumCapacity, string usagePolicy)
    {
        public int Id { get; private set; } = id;
        public string Name { get; private set; } = name;
        public string Location { get; private set; } = location;
        public string Type { get; private set; } = type;
        public int MaximumCapacity { get; private set; } = maximumCapacity;
        public string UsagePolicy { get; private set; } = usagePolicy;
    }
}