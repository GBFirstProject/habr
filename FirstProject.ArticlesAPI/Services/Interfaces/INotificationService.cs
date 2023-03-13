using FirstProject.Messages;

namespace FirstProject.ArticlesAPI.Services.Interfaces
{
    
        public interface INotificationService
        {
            void SendArticleLiked(ArticleLiked message, CancellationToken cts);

            void SendArticleDisliked(ArticleDisliked message, CancellationToken cts);
        void SendArticleApproved(ArticleApproved message, CancellationToken cts);
        void SendArticleRejected(ArticleRejected message, CancellationToken cts);
    }
    
}
