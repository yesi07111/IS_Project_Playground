using Playground.Application.Services;

namespace Playground.Infraestructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;

    public DateTime UtcNow => DateTime.UtcNow;

    public DateOnly Today => DateOnly.FromDateTime(Now);

    public TimeOnly Time => TimeOnly.FromDateTime(Now);
}