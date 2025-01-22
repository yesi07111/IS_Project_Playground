using FastEndpoints;
using Playground.Application.Queries.User.Get;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.User;

public class GetUserEndpoint : Endpoint<GetUserQuery, GetUserResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/user/get");
    }

    public override async Task<GetUserResponse> ExecuteAsync(GetUserQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}