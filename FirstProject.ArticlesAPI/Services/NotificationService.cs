using FirstProject.ArticlesAPI.Services.Interfaces;
using FirstProject.Messages;

namespace FirstProject.ArticlesAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _notificationServiceUrl;

        public NotificationService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _notificationServiceUrl = configuration.GetConnectionString("NotificationService");
        }

        public void SendArticleLiked(ArticleLiked message, CancellationToken cts)
        {
            Task.Factory.StartNew(async () =>
            {
                var client = _httpClientFactory.CreateClient();

                var request = new HttpRequestMessage(HttpMethod.Post, _notificationServiceUrl + "/Notifications" + "/commented")
                {
                    Content = JsonContent.Create(message)
                };

                await client.SendAsync(request);
            });
        }


        public void SendArticleDisliked(ArticleDisliked message, CancellationToken cts)
        {
            Task.Factory.StartNew(async () =>
            {
                var client = _httpClientFactory.CreateClient();

                var request = new HttpRequestMessage(HttpMethod.Post, _notificationServiceUrl + "/Notifications" + "/disliked")
                {
                    Content = JsonContent.Create(message)
                };

                await client.SendAsync(request);
            });
        }

        public void SendArticleApproved(ArticleApproved message, CancellationToken cts)
        {
            Task.Factory.StartNew(async () =>
            {
                var client = _httpClientFactory.CreateClient();

                var request = new HttpRequestMessage(HttpMethod.Post, _notificationServiceUrl + "/Notifications" + "/approved")
                {
                    Content = JsonContent.Create(message)
                };

                await client.SendAsync(request);
            });
        }

        public void SendArticleRejected(ArticleRejected message, CancellationToken cts)
        {
            Task.Factory.StartNew(async () =>
            {
                var client = _httpClientFactory.CreateClient();

                var request = new HttpRequestMessage(HttpMethod.Post, _notificationServiceUrl + "/Notifications" + "/rejected")
                {
                    Content = JsonContent.Create(message)
                };

                await client.SendAsync(request);
            });
        }
    }
}
