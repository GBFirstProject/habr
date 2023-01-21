using FirstProject.Messages;

using MassTransit;

namespace FirstProject.MessageTest
{
    public class TestConsumer : IConsumer<Notification>
    {
        public Task Consume(ConsumeContext<Notification> context)
        {
            Console.WriteLine(context.Message.Content);
            Console.WriteLine(context.Message.Reference);
            Console.WriteLine(context.Message.UserId);
            return Task.CompletedTask;
        }
    }
}
