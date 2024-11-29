using FastEndpoints;
using Playground.Application.Queries.CheckEmail;
using Playground.Application.Queries.Dtos;

namespace Playground.WebApi.Endpoints;

public class CheckEmailEndpoint : Endpoint<CheckEmailQuery, CheckEmailResponse>
{
    public override void Configure()
    {
        Get("auth/check-email");
        AllowAnonymous();
    }

    public override Task<CheckEmailResponse> ExecuteAsync(CheckEmailQuery req, CancellationToken ct)
    {
        return req.ExecuteAsync(ct);
    }
}