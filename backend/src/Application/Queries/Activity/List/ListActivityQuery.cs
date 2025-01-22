using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Queries.Activity.List;

public record ListActivityQuery : ICommand<ListActivityResponse>
{
    public string? Rating { get; init; }
    public string? StartDateTime { get; init; }
    public string? EndDateTime { get; init; }
    public string? StartTime { get; init; }
    public string? EndTime { get; init; }
    public string? Educators { get; init; }
    public string? FacilityTypes { get; init; }
    public string? ActivityTypes { get; init; }
    public int? MinAge { get; init; }
    public int? MaxAge { get; init; }
    public string? Availability { get; init; }
    public string? Today { get; init; }
    public string? Tomorrow { get; init; }
    public string? ThisWeek { get; init; }
    public string? DaysOfWeek { get; init; }
    public int? Capacity { get; init; }
    public string? IsNew { get; init; }
    public string UseCase { get; init; } = string.Empty;
}