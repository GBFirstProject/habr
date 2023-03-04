using FirstProject.Messages;

namespace FirstProject.CommentsAPI.Interfaces
{
    public interface INotificationService
    {
        void SendMessage(IMessage message, CancellationToken cts);
    }
}
