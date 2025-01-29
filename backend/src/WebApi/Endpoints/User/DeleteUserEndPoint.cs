using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Commands.User.Delete;

namespace Playground.WebApi.Endpoints.User;

public class DeleteUserEndpoint : Endpoint<DeleteUserCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Delete("/user/delete");
    }

    public override async Task<GenericResponse> ExecuteAsync(DeleteUserCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}