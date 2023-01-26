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
        private readonly Notifier notifier;

        public NotificationsController(Notifier notifier)
        {
            this.notifier=notifier;
        }

         [HttpPost]
         
         public Task<bool> ModeratorRequested(IModeratorRequested notification)
         {
            var result = notifier.Notify(notification);
            return result;          
         }

        [HttpPost]
        [Route("/liked")]

        public Task<bool> ArticleLiked(IArticleLiked notification)
        {
            var result = notifier.ArticleLiked(notification);
            return result;
        }


        [HttpPost]
        [Route("/commented")]

        public Task<bool> ArticleCommented(IArticleCommented notification)
        {
            var result = notifier.ArticleCommented(notification);
            return result;
        }







    }
}
