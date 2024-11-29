using FastEndpoints;
using Playground.Application.Commands.CleanUp;
using Playground.Application.Commands.Dtos;

namespace Playground.WebApi.Endpoints;

public class CleanUpUnverifiedUsersEndpoint : Endpoint<CleanUpUnverifiedUsersCommand, CleanUpUnverifiedUsersResponse>
{
    public override void Configure()
    {
        Delete("auth/cleanup-unverified-users");
        AllowAnonymous();
    }

    public override Task<CleanUpUnverifiedUsersResponse> ExecuteAsync(CleanUpUnverifiedUsersCommand req, CancellationToken ct)
    {
        return req.ExecuteAsync(ct);
    }
}