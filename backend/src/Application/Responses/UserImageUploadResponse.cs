namespace Playground.Application.Responses;

public record UserImageUploadResponse(string ImageUrl, IEnumerable<string> Others);