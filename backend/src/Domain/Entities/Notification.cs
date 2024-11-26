namespace Playground.Domain.Entities
{
    public class Notification
    {
        public string Sender { get; set; }
        public DateTime SentDate { get; set; }
        public string Message { get; set; }

        public Notification(string sender, string message)
        {
            Sender = sender;
            SentDate = DateTime.UtcNow;
            Message = message;
        }
    }
}