using FirstProject.Messages;


using MassTransit;

namespace FirstProject.NotificationAPI.Producers
{
    public class ModeratorNotifier
    {
        private readonly IBus bus;

        public ModeratorNotifier(IBus bus)
        {
            this.bus=bus;
        }

        public Task<bool> Notify(Notification notification)
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
