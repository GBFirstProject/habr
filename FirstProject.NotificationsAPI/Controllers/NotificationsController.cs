using FirstProject.Messages;
using FirstProject.NotificationAPI.Producers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject.NotificationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ModeratorNotifier notifier;

        public NotificationsController(ModeratorNotifier notifier)
        {
            this.notifier=notifier;
        }

         [HttpPost]
         
         public Task<bool> NotifyModerator(INotification notification)
         {
            var result = notifier.Notify(notification);
            return result;          
         }



    }
}
