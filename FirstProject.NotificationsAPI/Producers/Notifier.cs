using FirstProject.Messages;


using MassTransit;

namespace FirstProject.NotificationAPI.Producers
{
    public class Notifier
    {
        private readonly IBus bus;

        public Notifier(IBus bus)
        {
            this.bus=bus;
        }

        public Task<bool> PublishModeratorRequested(ModeratorRequested notification)
        {
            try
            {
                bus.Publish(notification);

                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
                throw;
            }
          
        }

        public Task<bool> PublishArticleLiked(ArticleLiked notification)
        {
            try
            {
                bus.Publish(notification);

                return Task.FromResult(true);

            }
            catch (Exception)
            {
                return Task.FromResult(false);
                throw;
            }

        }

        public Task<bool> PublishArticleDisliked(ArticleDisliked notification)
        {
            try
            {
                bus.Publish(notification);

                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
                throw;
            }
        }



        public Task<bool> PublishArticleCommented(ArticleCommented notification)
        {
            try
            {
                bus.Publish(notification);

                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
                throw;
            }
        }
    }
}
