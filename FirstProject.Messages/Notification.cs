namespace FirstProject.Messages
{
    public class Notification : INotification
    {
        public string Content { get; set; } = string.Empty;

        public string Reference { get; set; } = string.Empty;

        public int UserId { get; set; }

  

    }
}