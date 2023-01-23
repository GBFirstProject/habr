namespace FirstProject.Messages
{
    public class Notification : INotification
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Content { get; set; } = string.Empty;

        public string Reference { get; set; } = string.Empty;

        public DateTime TimeCreated { get; set; }

    }
}