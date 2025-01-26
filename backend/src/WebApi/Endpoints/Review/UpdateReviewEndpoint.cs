using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Commands.Review.Update;

namespace Playground.WebApi.Endpoints.Review;

public class UpdateReviewEndpoint : Endpoint<UpdateReviewCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Put("/review/update");
    }

    public override async Task<GenericResponse> ExecuteAsync(UpdateReviewCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}