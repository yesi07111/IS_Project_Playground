using FastEndpoints;
using Playground.Application.Commands.User.Update;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.User;

public class UpdateUserEndpoint : Endpoint<UpdateUserCommand, UpdateUserResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Put("/user/edit");
    }

    public override async Task<UpdateUserResponse> ExecuteAsync(UpdateUserCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}