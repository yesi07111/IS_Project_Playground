//newLaura

namespace Playground.Domain.Entities
{
    public class Resource(int id, string name, string type, string location, float useFrecuency, string resourceCondition, Facility facility)
    {
        public int Id { get; private set; } = id;
        public string Name { get; private set; } = name;
        public string Type { get; private set; } = type;
        public string Location { get; private set; } = location;
        public float UseFrecuency { get; private set; } = useFrecuency;
        public string ResourceCondition { get; private set; } = resourceCondition;
        public Facility Facility { get; private set; } = facility;
    }
}