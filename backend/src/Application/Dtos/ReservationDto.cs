namespace Playground.Application.Dtos
{
    public class ReservationDto
    {

        public string ActivityId { get; set; } = string.Empty;
        public string ActivityName { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string Comments { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}