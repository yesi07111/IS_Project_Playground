namespace Playground.Application.Commands.Dtos;

public class ErrorResponse
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public string? Detail { get; set; }
    public int? Status { get; set; }
    public int? ClientCode { get; set; }
    public string? Instance { get; set; }
}