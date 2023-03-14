using FirstProject.ArticlesAPI.Services.Interfaces;
using FirstProject.Messages;

namespace FirstProject.ArticlesAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _notificationServiceUrl;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<NotificationService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _notificationServiceUrl = configuration.GetConnectionString("NotificationService");
            _logger = logger;
        }

        public void SendArticleLiked(ArticleLiked message, CancellationToken cts)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError("NotificationService:SendArticleLiked", ex);
            }
        }


        public void SendArticleDisliked(ArticleDisliked message, CancellationToken cts)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError("NotificationService:SendArticleDisliked", ex);
            }
        }

        public void SendArticleApproved(ArticleApproved message, CancellationToken cts)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError("NotificationService:SendArticleApproved", ex);
            }
        }

        public void SendArticleRejected(ArticleRejected message, CancellationToken cts)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError("NotificationService:SendArticleRejected", ex);
            }
        }
    }
}
