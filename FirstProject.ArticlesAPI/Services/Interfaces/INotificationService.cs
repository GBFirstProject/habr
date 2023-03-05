using FirstProject.Messages;

namespace FirstProject.ArticlesAPI.Services.Interfaces
{
    
        public interface INotificationService
        {
            void SendArticleLiked(ArticleLiked message, CancellationToken cts);

            void SendArticleDisliked(ArticleDisliked message, CancellationToken cts);
    }
    
}
