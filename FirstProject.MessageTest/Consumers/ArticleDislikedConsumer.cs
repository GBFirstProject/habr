using FirstProject.Messages;

using MassTransit;

using RabbitMQ.Client;

namespace FirstProject.MessageTest.Consumers
{
    public class ArticleDislikedConsumer : IConsumer<ArticleDisliked>
    {
        public Task Consume(ConsumeContext<ArticleDisliked> context)
        {
            Console.WriteLine("article disliked");
            return context.ConsumeCompleted;
        }
    }


    public class ArticleDislikedConsumerDefinition : ConsumerDefinition<ArticleDislikedConsumer>
    {       
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ArticleDislikedConsumer> consumerConfigurator)
        {
            endpointConfigurator.AutoStart = false;
            consumerConfigurator.UseRetry(r => {
                r.Incremental(4, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            });
            consumerConfigurator.UseInMemoryOutbox();
        }
    }
}
