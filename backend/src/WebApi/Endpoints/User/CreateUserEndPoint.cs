using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Commands.User.Create;

namespace Playground.WebApi.Endpoints.User;

public class CreateUserEndpoint : Endpoint<CreateUserCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/user/create");
    }

    public override async Task<GenericResponse> ExecuteAsync(CreateUserCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}

