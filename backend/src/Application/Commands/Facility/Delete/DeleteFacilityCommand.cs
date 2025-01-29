using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.Facility.Delete;

public class DeleteFacilityCommand : ICommand<GenericResponse>
{
    public string Id { get; init; } = string.Empty;
}