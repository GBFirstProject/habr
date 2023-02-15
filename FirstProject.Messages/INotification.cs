namespace FirstProject.Messages
{
    public interface INotification
    {
        string Content { get; set; }
        string Reference { get; set; }
        int UserId { get; set; }
    }
}