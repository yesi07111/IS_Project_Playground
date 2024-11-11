namespace Playground.Application.Services;

public interface IDateTimeService
{
    public DateTime Now { get; }
    public DateTime UtcNow { get; }
    public DateOnly Today { get; }
    public TimeOnly Time { get; }
}