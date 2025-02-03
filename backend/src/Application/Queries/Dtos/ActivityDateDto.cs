namespace Playground.Application.Queries.Dtos;

public class ActivityDateDto
{
    public Guid Id { get; set; }

    public DateTime DateTime { get; set; }

    public bool Pending { get; set; } = false;
}