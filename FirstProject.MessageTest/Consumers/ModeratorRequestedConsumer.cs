using FirstProject.Messages;

using MassTransit;

using RabbitMQ.Client;

namespace FirstProject.MessageTest.Consumers
{
    public class ModeratorRequestedConsumer : IConsumer<ModeratorRequested>
    {
        public Task Consume(ConsumeContext<ModeratorRequested> context)
        {
            Console.WriteLine("moderator requested");  
            return context.ConsumeCompleted;
        }
    }
        public class ModeratorRequestedConsumerDefinition : ConsumerDefinition<ModeratorRequestedConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ModeratorRequestedConsumer> consumerConfigurator)
        {
           
            endpointConfigurator.AutoStart = false;
            consumerConfigurator.UseRetry(r =>
            {
                r.Incremental(4, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            });
            consumerConfigurator.UseInMemoryOutbox();
        }
    }
}
