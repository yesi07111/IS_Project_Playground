using FastEndpoints;
using Playground.Application.Commands.DeleteFailUser;
using Playground.Application.Commands.Responses;

namespace Playground.WebApi.Endpoints;

public class DeleteFailUserEndpoint : Endpoint<DeleteFailUserCommand, DeleteFailUserResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Delete("/auth/delete-fail-user");
    }

    public override async Task<DeleteFailUserResponse> ExecuteAsync(DeleteFailUserCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}