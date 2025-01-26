using FastEndpoints;
using Playground.Application.Queries.Review.List;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Review;

public class ListReviewEndpoint : Endpoint<ListReviewQuery, ListReviewResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/review/get-all");
    }

    public override async Task<ListReviewResponse> ExecuteAsync(ListReviewQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}