using FirstProject.Messages;

namespace FirstProject.CommentsAPI.Interfaces
{
    public interface INotificationService
    {
        void SendCommentCreated(ArticleCommented message, CancellationToken cts);
    }
}
