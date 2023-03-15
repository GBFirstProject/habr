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


        public async Task<bool> PublishModeratorRequested(ModeratorRequested notification)
        {
            
            try
            {
               await bus.Publish(notification);

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
          
        }

        public async Task<bool> PublishArticleLiked(ArticleLiked notification)
        {
            
            try
            {
               await bus.Publish(notification);

                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }

        public async Task<bool> PublishArticleDisliked(ArticleDisliked notification)
        {
            try
            {
               await bus.Publish(notification);

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public  async Task<bool> PublishArticleCommented(ArticleCommented notification)
        {
            try
            {
                await bus.Publish(notification);

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> PublishArticleApproved(ArticleApproved notification)
        {
            try
            {
                await bus.Publish(notification);

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }


        public async Task<bool> PublishArticleRejected(ArticleRejected notification)
        {
            try
            {
                await bus.Publish(notification);

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }


    }
}
