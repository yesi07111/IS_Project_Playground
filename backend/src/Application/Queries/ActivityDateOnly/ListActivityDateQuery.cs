using FastEndpoints;
using Playground.Application.Queries.Responses;

namespace Playground.Application.Queries.ActivityDateOnly;

public record ListActivityDateQuery : ICommand<ListActivityDateResponse>
{
    public string ActivityId {get; init;} = string.Empty;
};