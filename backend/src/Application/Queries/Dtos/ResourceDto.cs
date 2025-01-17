namespace Playground.Application.Queries.Dtos
{
    public class ResourceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int UseFrequency { get; set; }
        public string Condition { get; set; } = string.Empty;
        public string FacilityName { get; set; } = string.Empty;
        public string FacilityLocation { get; set; } = string.Empty;
        public string FacilityType { get; set; } = string.Empty;
    }
}