using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Commands.ResourceDate;

namespace Playground.WebApi.Endpoints.ResourceDate;

public class CreateResourceDateEndPoint : Endpoint<CreateResourceDateCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/resourceDate/create");
    }

    public override async Task<GenericResponse> ExecuteAsync(CreateResourceDateCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}