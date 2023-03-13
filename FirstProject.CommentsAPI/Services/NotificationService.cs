using FirstProject.CommentsAPI.Interfaces;
using FirstProject.Messages;

namespace FirstProject.CommentsAPI.Services
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

        public void SendCommentCreated(ArticleCommented message, CancellationToken cts)
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
    }
}
