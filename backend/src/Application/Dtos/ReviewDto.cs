namespace Playground.Application.Dtos
{
    public class ReviewDto
    {
        public string ReviewId { get; set; } = string.Empty;
        public string ActivityId { get; set; } = string.Empty;
        public string ActivityName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}