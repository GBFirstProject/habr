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

        public Task<bool> Notify(IModeratorRequested notification)
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

        public Task<bool> ArticleLiked(IArticleLiked notification)
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

        public Task<bool> ArticleCommented(IArticleCommented notification)
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
