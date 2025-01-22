using FastEndpoints;
using Playground.Application.Queries.User.List;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.User;

public class ListUserEndpoint : Endpoint<ListUserQuery, ListUserResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/user/get-all");
    }

    public override async Task<ListUserResponse> ExecuteAsync(ListUserQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}