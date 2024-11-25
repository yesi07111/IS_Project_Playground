using FastEndpoints;
using Playground.Application.Commands.Users.DeleteFailUser;
using Playground.Application.Commands.DeleteFailUser;

namespace Playground.WebApi.Endpoints;

public class DeleteFailUserEndpoint : Endpoint<DeleteFailUserCommand, DeleteFailUserResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/auth/delete-unverified-user");
    }

    public override async Task<DeleteFailUserResponse> ExecuteAsync(DeleteFailUserCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}