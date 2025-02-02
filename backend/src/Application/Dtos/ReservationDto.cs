using System.Data.SqlTypes;

namespace Playground.Application.Dtos
{
    public class ReservationDto
    {
        public string ReservationId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string ActivityId { get; set; } = string.Empty;
        public string ActivityName { get; set; } = string.Empty;
        public string ActivityDate { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string Comments { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}