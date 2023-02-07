using FirstProject.Messages;

using MassTransit;

using RabbitMQ.Client;

namespace FirstProject.MessageTest.Consumers
{
    public class ArticleCommentedConsumer : IConsumer<ArticleCommented>
    {
        public Task Consume(ConsumeContext<ArticleCommented> context)
        {
            Console.WriteLine("article disliked");
            return context.ConsumeCompleted;
        }
    }

    public class ArticleCommentedConsumerDefinition : ConsumerDefinition<ArticleCommentedConsumer>
    { 
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ArticleCommentedConsumer> consumerConfigurator)
        {
            endpointConfigurator.AutoStart = false;
            consumerConfigurator.UseRetry(r=>{
                r.Incremental(4, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            });
            consumerConfigurator.UseInMemoryOutbox();             
            }
        }
    }

