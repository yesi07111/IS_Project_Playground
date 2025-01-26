namespace Playground.Application.Queries.Dtos;

public class ResourceDateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; } 
    public int UseFrequency { get; set; }
}