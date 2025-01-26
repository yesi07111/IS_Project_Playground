using FastEndpoints;
using Playground.Application.Commands.CleanUp;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints;

public class CleanUpUnverifiedUsersEndpoint : EndpointWithoutRequest<CleanUpUnverifiedUsersResponse>
{
    private readonly CleanUpUnverifiedUsersCommandHandler _commandHandler;

    public CleanUpUnverifiedUsersEndpoint(CleanUpUnverifiedUsersCommandHandler commandHandler)
    {
        _commandHandler = commandHandler;
    }

    public override void Configure()
    {
        Delete("auth/cleanup-unverified-users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await _commandHandler.ExecuteAsync(ct);
        await SendAsync(response, cancellation: ct);
    }
}