using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Commands.Review.Delete;

namespace Playground.WebApi.Endpoints.Review;

public class DeleteReviewEndpoint : Endpoint<DeleteReviewCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Delete("/review/delete");
    }

    public override async Task<GenericResponse> ExecuteAsync(DeleteReviewCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}