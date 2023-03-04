using FirstProject.Messages;
using FirstProject.MessageTest.Hubs;

using MassTransit;

using Microsoft.AspNetCore.SignalR;

using RabbitMQ.Client;

namespace FirstProject.MessageTest.Consumers
{
    public class ArticleLikedConsumer : IConsumer<ArticleLiked>
    {
        private readonly IHubContext<NotificationHub> hubContext;

        public ArticleLikedConsumer(IHubContext<NotificationHub> hubContext)
        {
            this.hubContext=hubContext;
        }

        public Task Consume(ConsumeContext<ArticleLiked> context)
        {
            Console.WriteLine("article liked");
            hubContext.Clients.Client("").SendAsync("articleLikedNotify");//заглушка
            return context.ConsumeCompleted;
        }
    }

    public class ArticleLikedConsumerDefinition : ConsumerDefinition<ArticleLikedConsumer>
    {   
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ArticleLikedConsumer> consumerConfigurator)
        {
            endpointConfigurator.AutoStart = false;
            consumerConfigurator.UseRetry(r => {
                r.Incremental(4, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            });
            consumerConfigurator.UseInMemoryOutbox();
        }
    }
}
