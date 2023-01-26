using FirstProject.Messages;

using MassTransit;

namespace FirstProject.MessageTest
{
    public class TestConsumer : IConsumer<ModeratorRequested>
    {
        public Task Consume(ConsumeContext<ModeratorRequested> context)
        {
            Console.WriteLine(context.Message.Content);
            Console.WriteLine(context.Message.Reference);
            Console.WriteLine(context.Message.UserId);
            return Task.CompletedTask;
        }
    }
}
