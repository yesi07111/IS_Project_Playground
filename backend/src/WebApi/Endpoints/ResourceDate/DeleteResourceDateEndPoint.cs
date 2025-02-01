using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Commands.ResourceDate.Delete;

namespace Playground.WebApi.Endpoints.ResourceDate;

public class DeleteResourceDateEndPoint : Endpoint<DeleteResourceDateCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Delete("/resourceDate/delete");
    }

    public override async Task<GenericResponse> ExecuteAsync(DeleteResourceDateCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}