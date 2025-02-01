using FastEndpoints;
using Playground.Application.Commands.Responses;

namespace Playground.Application.Commands.Resource.Create;

public record CreateResourceCommand : ICommand<GenericResponse>
{
    public string Name {get; init;} = string.Empty;
    public string Type {get; init;} = string.Empty;
    public string ResourceCondition {get; init;} = string.Empty;
    public string FacilityId {get; init;} = string.Empty;
}